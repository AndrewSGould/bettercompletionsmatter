namespace WebApi.Controllers;

using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;
using TavisApi.ContestRules;

[ApiController]
[Route("[controller]")]
public class RgscController : ControllerBase
{
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;

  public RgscController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService)
  {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
  }

  [HttpGet]
  [Route("user-summary")]
  public IActionResult RgscStats(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
    var playerId = localuser.Id;

    var rgsc = _context.BcmRgsc.Where(x => x.PlayerId == playerId)
                                .OrderByDescending(x => x.Issued)
                                .ToList();

    var playersCompletedGames = _context.BcmPlayerGames.Where(x => x.PlayerId == playerId
                                                      && x.CompletionDate != null
                                                      && x.CompletionDate.Value.Year == DateTime.Now.Year);

    var rgscList = rgsc.Join(_context.Games, rgsc => rgsc.GameId,
                                g => g.Id, (rgsc, g) => new { Rgsc = rgsc, Game = g });

    var rgscCompletions = rgsc.Join(playersCompletedGames, rgsc => rgsc.GameId,
                                pg => pg.GameId, (rgsc, pg) => new { Rgsc = rgsc, PlayerGames = pg });

    var rgscCompletedGameCount = 0;

    // if the game was completed within the year it was issues, it scores
    foreach (var game in rgscCompletions)
    {
      if (game.PlayerGames.CompletionDate.Value.Year == game.Rgsc.Issued.Value.Year)
        rgscCompletedGameCount++;
    }

    var rerollsUsed = rgsc.Count(x => x.Rerolled);

    var nonrerolledRgsc = rgsc.FirstOrDefault(x => !x.Rerolled)?.GameId;
    var currentRgscs = _context.Games.Where(x => x.Id == nonrerolledRgsc);

    return Ok(new
    {
      CurrentRandoms = rgscList.Where(x => !x.Rgsc.Rerolled),
      RerollsRemaining = BcmRule.RgscStartingRerolls + rgscCompletedGameCount - rerollsUsed,
      RgscsCompleted = rgscCompletedGameCount,
      RandomsRolledAway = rgscList.Where(x => x.Rgsc.Rerolled),
    });
  }
}
