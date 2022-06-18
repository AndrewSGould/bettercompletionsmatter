namespace WebApi.Controllers;

using System.Diagnostics;
using Bcm.Models;
using BcmApi.ContestRules;
using BcmApi.Context;
using BcmApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BcmController : ControllerBase {
  private BcmContext _context;
  private readonly IParser _parser;

  public BcmController(BcmContext context, IParser parser) {
    _context = context;
    _parser = parser;
  }

  [HttpGet]
  [Route("getRandomGame")]
  public IActionResult GetRandomGame(int playerId)
  {
    var player = _context.Players!.First(x => x.Id == playerId);
    var bcmRandomGameOptions = _context.PlayerGames
                    .Join(_context.Games!, pg => pg.GameId, 
                      g => g.Id, (pg, g) => new {PlayersGames = pg, Games = g})
                    .Where(x => x.PlayersGames.PlayerId == playerId 
                      && x.Games.SiteRatio > BcmRule.MinimumRatio
                      && x.Games.FullCompletionEstimate != null
                      && x.Games.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
                      && x.Games.GamersCompleted > 0
                      && !x.Games.Unobtainables
                      && !x.PlayersGames.NotForContests
                      && x.PlayersGames.CompletionDate == null
                      //&& x.Games.ServerClosure == null
                      && x.PlayersGames.Ownership != Bcm.Models.Ownership.NoLongerHave
                      && BcmRule.RandomValidPlatforms.Contains(x.PlayersGames.Platform!))
                    .ToList();

    if (bcmRandomGameOptions.Count() < BcmRule.RandomMinimumEligibilityCount) {
      return BadRequest($"{player.Name} did not have enough eligible games! " +
            $"Player needs to add {50 - bcmRandomGameOptions.Count()} more games.");
    }
    
    var rand = new Random();
    var randomGame = bcmRandomGameOptions[rand.Next(bcmRandomGameOptions.Count())];

    return Ok(new {
      selectedGame = randomGame,
      potentials = bcmRandomGameOptions
    });
  }

  [HttpGet]
  [Route("verifyRandomGameEligibility")]
  public IActionResult VerifyRandomGameEligibility() {
    var playersIneligible = new List<object>();
    var players = _context.Players!.Where(x => x.Name != "zzScanMan1").ToList();

    foreach(var player in players) {
      var bcmRandomGameOptions = _context.PlayerGames
            .Join(_context.Games!, pg => pg.GameId, 
              g => g.Id, (pg, g) => new {PlayersGames = pg, Games = g})
            .Where(x => x.PlayersGames.PlayerId == player.Id
              && x.Games.SiteRatio > BcmRule.MinimumRatio
              && x.Games.FullCompletionEstimate != null
              && x.Games.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
              && x.Games.GamersCompleted > 0
              && !x.Games.Unobtainables
              && !x.PlayersGames.NotForContests
              && x.PlayersGames.CompletionDate == null
              //&& x.Games.ServerClosure == null
              && x.PlayersGames.Ownership != Bcm.Models.Ownership.NoLongerHave
              && BcmRule.RandomValidPlatforms.Contains(x.PlayersGames.Platform!))
            .ToList();

      if (bcmRandomGameOptions.Count() < BcmRule.RandomMinimumEligibilityCount) {
        playersIneligible.Add(new {
          Player = player.Name,
          EligibleCount = bcmRandomGameOptions.Count()
        });
      }
    }

    return Ok(playersIneligible);
  }
}
