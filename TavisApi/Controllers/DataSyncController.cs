namespace WebApi.Controllers;

using System.Diagnostics;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using static TavisApi.Services.DataSync;
using static TavisApi.Services.TA_GameCollection;

[ApiController]
[Route("api/[controller]")]
public class DataSyncController : ControllerBase {
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;

  public DataSyncController(TavisContext context, IParser parser, IDataSync dataSync) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
  }

  [HttpGet]
  [Route("syncAllData")]
  public IActionResult SyncAll()
  {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    // ScanMan is an alt account that was autoadding all games to collection
    // useful for benchmarking, has over 7,000 games on the profile 
    var players = _context.Players!
                    .Join(_context.PlayerContests!, p => p.Id, pc => pc.PlayerId, (p, pc) => 
                      new { Player = p, PlayerContest = pc })
                    .Where(x => x.Player.IsActive && x.PlayerContest.ContestId == 2).ToList();

    Console.WriteLine($"Beginning data sync with {players.Count()} player(s) at {DateTime.Now}");

    var results = new List<TaParseResult>();
    var gcOptions = new SyncOptions {
      CompletionStatus = SyncOption_CompletionStatus.Complete
    };

    foreach(var player in players) {
      var parsedPlayer = _dataSync.ParseTa(player.Player.Id, gcOptions);
      results.Add(parsedPlayer);
      player.Player.LastSync = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
      Console.WriteLine($"Player {player.Player.Name} has been parsed with a processing time of {parsedPlayer.Performance}");
    }

    _context.SaveChanges();

    var totalHits = 0;
    TimeSpan totalTimeHittingTa = new TimeSpan();
    foreach (var result in results) {
      totalHits = totalHits + result.TaHits;
      totalTimeHittingTa = totalTimeHittingTa.Add(result.TimeHittingTa);
    }

    stopWatch.Stop();

    TimeSpan ts = stopWatch.Elapsed;

    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    string totalTaHitTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        totalTimeHittingTa.Hours, totalTimeHittingTa.Minutes, totalTimeHittingTa.Seconds,
        totalTimeHittingTa.Milliseconds / 10);

    Console.WriteLine($"Completed data sync at {DateTime.Now}");

    return Ok(new {
      OverallTime = elapsedTime,
      TotalTaHits = totalHits,
      TotalTimeHittingTa = totalTaHitTime,
      PerPlayerTime = results
    });
  }


  //TODO: this is here just to test
  //eventually, pull this out to the service only
  //and ONLY call this when the user requests it
  [HttpGet]
  [Route("testSyncGameInfo")]
  public IActionResult SyncGameInfo() {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    Console.WriteLine($"Beginning game sync with at {DateTime.Now}");
    //accept a list of game ids to update
    //loop over the game ids and get the URL from DB
    //parse out the game information
    //save to DB
    _dataSync.ParseGamePages(new List<int>());
    Console.WriteLine($"Games have been sync'd, finished at {DateTime.Now}");
    stopWatch.Stop();

    TimeSpan ts = stopWatch.Elapsed;

    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);
    return Ok(elapsedTime);
  }

  [HttpGet]
  [Route("testGwgParse")]
  public IActionResult ParseGwg() {
    //TODO: havent tested this yet
    var pageStart = 1;
    _dataSync.ParseGamesWithGold(ref pageStart);
    _context.SaveChanges();

    return Ok();
  }
}
