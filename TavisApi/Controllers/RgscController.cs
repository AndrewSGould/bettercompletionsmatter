namespace WebApi.Controllers;

using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;
using TavisApi.ContestRules;
using Microsoft.EntityFrameworkCore;
using Tavis.Models;

[ApiController]
[Route("[controller]")]
public class RgscController : ControllerBase
{
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;
  private readonly IRgscService _rgscService;

  public RgscController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService, IRgscService rgscService)
  {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _rgscService = rgscService;
  }

  [HttpGet]
  [Route("user-summary")]
  public IActionResult RgscStats(string player)
  {
    var localuser = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
    if (localuser is null) return BadRequest("No user found with supplied gamertag");

    var playerId = localuser.BcmPlayer?.Id;
    if (playerId is null) return BadRequest("Could not get Bcm Player");

    var rgsc = _context.BcmRgsc.Where(x => x.BcmPlayerId == playerId)
                                .OrderByDescending(x => x.Issued)
                                .ToList();

    var playersCompletedGames = _context.BcmPlayerGames.Where(x => x.PlayerId == playerId
                                                      && x.CompletionDate != null
                                                      && x.CompletionDate.Value.Year == DateTime.UtcNow.Year);

    var rgscList = rgsc.Join(_context.Games, rgsc => rgsc.GameId,
                                g => g.Id, (rgsc, g) => new { Rgsc = rgsc, Game = g });

    var rgscCompletions = rgsc.Join(playersCompletedGames, rgsc => rgsc.GameId,
                                pg => pg.GameId, (rgsc, pg) => new { Rgsc = rgsc, PlayerGames = pg });

    var rgscCompletedGameCount = 0;

    // if the game was completed within the year it was issues, it scores
    foreach (var game in rgscCompletions)
    {
      if (game.PlayerGames.CompletionDate!.Value.Year == game.Rgsc.Issued!.Value.Year)
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

  [HttpGet]
  [Route("getPlayersGames")]
  public IActionResult GetPlayersGames(string player)
  {
    var user = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
    if (user is null) return BadRequest("No user found");

    var bcmPlayer = _context.BcmPlayers.Include(x => x.BcmRgscs).FirstOrDefault(x => x.Id == user.BcmPlayer!.Id);
    var rgscList = bcmPlayer?.BcmRgscs
      ?.Where(x => !x.Rerolled)
      ?.Select(x => new
      {
        GameId = x.GameId ?? -1,
        Title = _context.Games.FirstOrDefault(y => y.Id == x.GameId)?.Title ?? "No Random Drawn"
      });

    return Ok(new
    {
      Randoms = rgscList,
      // todo; change this to rgsc service that returns a users reroll count
      RerollsRemaining = _rgscService.GetUserRerollCount(user.Id)
    });
  }
}
