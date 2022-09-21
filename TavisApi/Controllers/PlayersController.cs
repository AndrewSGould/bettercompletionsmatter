using System.Diagnostics;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using TavisApi.ContestRules;

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
    var players = _context.PlayerContests!.Where(x => x.ContestId == 1).Select(x => x.PlayerId);
    var bcmPlayers = _context.Players!.Where(x => x.IsActive && !x.Name!.Contains("zzScanMan1")).ToList();
    var hhPlayerList = new List<string>();
    // hhPlayerList.Add("A1exRD");
    // hhPlayerList.Add("Legohead 1977");
    // hhPlayerList.Add("Sir Paulygon");
    hhPlayerList.Add("Big Ell");
    // hhPlayerList.Add("Luke17000");
    // hhPlayerList.Add("MadLefty2097");
    hhPlayerList.Add("Whtthfgg");
    // hhPlayerList.Add("Whisperin Clown");
    // hhPlayerList.Add("Northern Lass");
    // hhPlayerList.Add("IcyThrasher");
    // hhPlayerList.Add("HegemonicHater");
    // hhPlayerList.Add("Oriole2682");
    // hhPlayerList.Add("boldyno1");
    // hhPlayerList.Add("lucas1987");
    // hhPlayerList.Add("Yinga Garten");
    // hhPlayerList.Add("TXMOOK");
    // hhPlayerList.Add("radnonnahs");
    // hhPlayerList.Add("Mtld");
    // hhPlayerList.Add("smrnov");
    // hhPlayerList.Add("Proulx");
    // hhPlayerList.Add("darkwing1232");
    // hhPlayerList.Add("Inferno118");
    // hhPlayerList.Add("Freamwhole");
    // hhPlayerList.Add("MajinFro");
    // hhPlayerList.Add("Majinbro");
    // hhPlayerList.Add("BenL72");
    hhPlayerList.Add("Matrarch");
    // hhPlayerList.Add("Hotdogmcgee");
    // hhPlayerList.Add("True Veteran");
    // hhPlayerList.Add("Erutaerc");
    // hhPlayerList.Add("Fista Roboto");
    hhPlayerList.Add("FlutteryChicken");
    // hhPlayerList.Add("ChewieOnIce");
    // hhPlayerList.Add("xLAx JesteR");
    // hhPlayerList.Add("Mental Knight 5");
    // hhPlayerList.Add("SwiftSupafly");
    // hhPlayerList.Add("Icefiretn");
    // hhPlayerList.Add("hotcurls3088");
    // hhPlayerList.Add("Fine Feat");
    // hhPlayerList.Add("NBA Kirkland");
    // hhPlayerList.Add("Team Brether");
    // hhPlayerList.Add("HawkeyeBarry20");
    hhPlayerList.Add("Mark B");
    // hhPlayerList.Add("Wakapeil");
    // hhPlayerList.Add("FreakyRO");
    // hhPlayerList.Add("eohjay");
    hhPlayerList.Add("Simpso");
    // hhPlayerList.Add("Infamous");
    // hhPlayerList.Add("CrunchyGoblin68");
    // hhPlayerList.Add("Christoph 5782");
    // hhPlayerList.Add("RetroChief1969");
    hhPlayerList.Add("WildwoodMike");
    // hhPlayerList.Add("IronFistofSnuff");
    hhPlayerList.Add("Hatton90");

    var hhPlayers = bcmPlayers.Where(x => hhPlayerList.Any(y => x.Name!.Contains(y)));

    var fullCompatResults = new List<PlayerCompatability>();
    var playersLists = new List<PlayersList>();

    // get everyone's valid games
    foreach(var player in hhPlayers) {
      var validGames = _context.PlayerGames?
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

      var typedList = new List<PlayerGamez>();
      foreach(var game in validGames!) {
        var temp = new PlayerGamez {
          Game = game.Games, 
          PlayerGame = game.PlayersGames
        };

        typedList.Add(temp);
      }

      var playersList = new PlayersList {
        Player = player!.Name!,
        List = typedList!
      };

      playersLists.Add(playersList);
    }

    foreach (var list in playersLists) {
      var startingList = list;
      var playerCompatability = new PlayerCompatability();
      playerCompatability.PlayerName = startingList.Player;
      playerCompatability.Compatability = new List<CompatabilityResult>();

      foreach (var comparedList in playersLists.Where(x => x.Player != startingList.Player)) {
        var commonalities = startingList!
                              .List
                              .Select(x => x.Game.Id)
                              .Intersect(comparedList.List.Select(x => x.Game.Id))
                              .ToList();
        
        var compatability = Decimal.Divide(commonalities.Count(), startingList!.List.Count());

        playerCompatability.Compatability.Add(new CompatabilityResult {
          ComparedPlayer = comparedList.Player,
          Compatability = compatability,
          Commonalities = commonalities
        });
      }

      playerCompatability.Compatability = playerCompatability.Compatability.OrderByDescending(x => x.Compatability).ToList();
      fullCompatResults.Add(playerCompatability);
    }
    
    fullCompatResults = fullCompatResults.OrderBy(x => x.PlayerName).ToList();
    return Ok(fullCompatResults);
  }

  public class CompatabilityResult {
    public string? ComparedPlayer {get;set;}
    public decimal? Compatability {get;set;}
    public List<int>? Commonalities {get;set;}
  }

  public class PlayersList {
    public string Player {get; set;}
    public List<PlayerGamez> List {get; set;}
  }

  public class PlayerGamez {
    public PlayerGame PlayerGame {get; set;}
    public Game Game {get; set;}
  }

  public class PlayerCompatability {
    public string PlayerName { get; set;}
    public List<CompatabilityResult> Compatability { get; set;}
  }
}
