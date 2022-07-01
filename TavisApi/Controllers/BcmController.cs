namespace WebApi.Controllers;

using System.Diagnostics;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using static TavisApi.Services.TA_GameCollection;

[ApiController]
[Route("api/[controller]")]
public class TavisController : ControllerBase {
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;

  public TavisController(TavisContext context, IParser parser, IDataSync dataSync) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
  }

  [HttpGet]
  [Route("ta_sync")]
  public IActionResult Sync()
  {
    var raidBossPlayers = _context.PlayerContests!.Where(x => x.ContestId == 1).Select(x => x.PlayerId);
    var players = _context.Players!.Where(x => x.IsActive && raidBossPlayers.Contains(x.Id)).ToList();

    var gcOptions = new TA_GC_Options { //TODO: change this to SyncOptions
      CompletionStatus = TAGC_CompletionStatus.Complete,
      DateCutoff = new DateTime(2021, 1, 1)
    };

    return Ok(_dataSync.DynamicSync(players, gcOptions));
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
    var playersEligible = new List<object>();
    var players = _context.Players!.Where(x => x.Name != "zzScanMan1").ToList();

    foreach(var player in players) {
      var randomGameOptions = _context.PlayerGames?
            .Join(_context.Games!, pg => pg.GameId, 
              g => g.Id, (pg, g) => new {PlayersGames = pg, Games = g})
            .Where(x => x.PlayersGames.PlayerId == player.Id
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
        playersIneligible.Add(new {
          Player = player.Name,
          EligibleCount = randomGameOptions.Count()
        });
      }
      else {
        playersEligible.Add(new {
          Player = player.Name,
          EligibleCount = randomGameOptions.Count()
        });
      }
    }

    var results = new {
      Invalids = playersIneligible,
      Valids = playersEligible
    };
    return Ok(results);
  }
}
