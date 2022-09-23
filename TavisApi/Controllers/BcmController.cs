namespace WebApi.Controllers;

using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using static TavisApi.Services.TA_GameCollection;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;

[ApiController]
[Route("api/[controller]")]
public class BcmController : ControllerBase {
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;
  private readonly IBcmService _bcmService;
  private static readonly Random rand = new Random();

  public BcmController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _bcmService = bcmService;
  }

  [HttpGet]
  [Route("ta_sync")]
  public IActionResult Sync()
  {
    var hhPlayers = RaidBossController.HhPlayers;
    var playersToScan = new List<Player>();

    var players = _context.PlayerContests!.Where(x => x.ContestId == 1).Select(x => x.PlayerId);
    var bcmPlayers = _context.Players!.Where(x => x.IsActive).ToList();
    //var bcmPlayers = _bcmService.GetPlayers();

    foreach (var player in bcmPlayers) {
      if (hhPlayers.Any(x => x.Player == player.Name))
        playersToScan.Add(player);
    }

    //TODO: for now, this sync is setup to only pull random games
    //  in the future, have the random game endpoint accept a sync option
    var gcOptions = new SyncOptions {
    };

    return Ok(_dataSync.DynamicSync(playersToScan, gcOptions));
  }

  [HttpGet]
  [Route("getRandomGame")]
  public IActionResult GetRandomGame(int playerId)
  {
    var player = _context.Players!.First(x => x.Id == playerId);
    var randomGameOptions = _context.PlayerGames?
                    .Join(_context.Games!, pg => pg.GameId, 
                      g => g.Id, (pg, g) => new {PlayersGames = pg, Games = g})
                    .Where(x => x.PlayersGames.PlayerId == playerId 
                      && x.Games.SiteRatio > BcmRule.MinimumRatio
                      && (x.Games.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
                        || x.Games.FullCompletionEstimate == null)
                      && x.Games.GamersCompleted > 0
                      && !x.Games.Unobtainables
                      && !x.PlayersGames.NotForContests
                      && x.PlayersGames.CompletionDate == null
                      //&& x.Games.ServerClosure == null
                      && x.PlayersGames.Ownership != Tavis.Models.Ownership.NoLongerHave
                      && BcmRule.RandomValidPlatforms.Contains(x.PlayersGames.Platform!))
                    .ToList();

    if (randomGameOptions?.Count() < BcmRule.RandomMinimumEligibilityCount) {
      return BadRequest($"{player.Name} did not have enough eligible games! " +
            $"Player needs to add {50 - randomGameOptions.Count()} more games.");
    }
    
    var rand = new Random();
    var randomGame = randomGameOptions?[rand.Next(randomGameOptions.Count())];

    return Ok(new {
      selectedGame = randomGame,
      potentials = randomGameOptions
    });
  }

  [HttpGet]
  [Route("verifyRandomGameEligibility")]
  public IActionResult VerifyRandomGameEligibility() {
    var playersIneligible = new List<object>();
    var allPlayers = new List<object>();
    var players = _bcmService.GetPlayers().OrderBy(x => x.Name);

    foreach(var player in players) {
      var randomGameOptions = _context.PlayerGames?
            .Join(_context.Games!, pg => pg.GameId, 
              g => g.Id, (pg, g) => new {PlayersGames = pg, Games = g})
            .Where(x => x.PlayersGames.PlayerId == player.Id
              && x.Games.SiteRatio > BcmRule.MinimumRatio
              // && (x.Games.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
              //   || x.Games.FullCompletionEstimate == null)
              && x.Games.GamersCompleted > 0
              && !x.Games.Unobtainables
              && !x.PlayersGames.NotForContests
              && x.PlayersGames.CompletionDate == null
              //&& x.Games.ServerClosure == null
              && x.PlayersGames.Ownership != Tavis.Models.Ownership.NoLongerHave
              && BcmRule.RandomValidPlatforms.Contains(x.PlayersGames.Platform!))
            .ToList();

      if (randomGameOptions?.Count() < BcmRule.HeadToHeadMinimumEligibilityCount) {
        playersIneligible.Add(new {
          Player = player.Name,
          EligibleCount = randomGameOptions.Count(),
          Options = randomGameOptions.Select(x => x.Games.Title)
        });
      }

      var random = rand.Next(0, randomGameOptions.Count());

      allPlayers.Add(new {
        Player = player.Name,
        RandomGame = randomGameOptions?.Count() < BcmRule.RandomMinimumEligibilityCount ? "" : randomGameOptions?[random].Games.Title,
        EligibleCount = randomGameOptions.Count(),
        Options = randomGameOptions.Select(x => x.Games.Title)
      });
    }

    var results = new {
      Invalids = playersIneligible,
      FullList = allPlayers
    };
    return Ok(results);
  }
}
