using System.Diagnostics;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase 
{
  private TavisContext _context;
  private Parser _parse = new Parser();

  public PlayersController(TavisContext context) {
    _context = context;
  }

  [HttpGet]
  [Route("getAll")]
  public IActionResult GetAll()
  {
    var players = _context.Players?.Where(x => x.Name != null);
    return Ok(players);
  }

  [HttpGet]
  [Route("getPlayersGames")]
  public IActionResult GetPlayersGames(int playerId)
  {
    if (playerId == 0) return BadRequest();

    var completedGames = _context.Players!
        .Join(_context.PlayerGames!, player => player.Id, complGame => complGame.PlayerId, 
            (player, complGame) => new { Player = player, ComplGame = complGame})
        .Where(x => x.Player.Id == playerId)
        .Join(_context.Games!, complGame => complGame.ComplGame.GameId, game => game.Id,
            (complGame, game) => new {ComplGame = complGame, Game = game})
        .Select(x => new {
          Title = x.Game.Title,
          Ratio = x.Game.SiteRatio,
          Time = x.Game.FullCompletionEstimate
        });

    return Ok(completedGames);
  }

  [HttpGet]
  [Route("getFullPlayerCompatability")]
  public IActionResult GetFullPlayerCompatability() {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    var players = _context.Players!.Where(x => x.Name != "zzScanMan1").ToList();

    var fullCompatResults = new List<CompatabilityResult>();

    foreach(var startingPlayer in players) {
      var startingPlayersGames = _context.PlayerGames!.Where(x => x.PlayerId == startingPlayer.Id 
                                                            && x.CompletionDate == null
                                                            && x.NotForContests == false);

      foreach(var comparingPlayer in players.Where(x => x != startingPlayer)) {
        var comparingPlayersGames = _context.PlayerGames!.Where(x => x.PlayerId == comparingPlayer.Id 
                                                            && x.CompletionDate == null
                                                            && x.NotForContests == false);

        var commonalities = startingPlayersGames
                              .Select(x => x.GameId)
                              .Intersect(comparingPlayersGames.Select(x => x.GameId))
                              .ToList();

        var compatability = Decimal.Divide(commonalities.Count(), startingPlayersGames.Count());

        fullCompatResults.Add(new CompatabilityResult {
          InitialPlayer = startingPlayer,
          ComparedPlayer = comparingPlayer,
          Compatability = compatability,
          Commonalities = commonalities
        });

        fullCompatResults = fullCompatResults.OrderBy(x => x.Compatability).ToList();
      } 
    }
    
    return Ok(fullCompatResults);
  }

  public class CompatabilityResult {
    public Player? InitialPlayer {get;set;}
    public Player? ComparedPlayer {get;set;}
    public decimal? Compatability {get;set;}
    public List<int>? Commonalities {get;set;}
  }
}
