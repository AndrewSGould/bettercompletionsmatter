namespace WebApi.Controllers;

using System.Diagnostics;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using static TavisApi.Services.DataSync;

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
    var players = _context.Players!.Where(x => x.Name != "zzScanMan1").ToList();

    var results = new List<TaParseResult>();

    foreach(var player in players) {
      var parsedPlayer = _dataSync.ParseTa(player.Id);
      results.Add(parsedPlayer);
      Console.WriteLine($"Player {player.Name} has been parsed with a processing time of {parsedPlayer.Performance}");
    }

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
    //accept a list of game ids to update
    //loop over the game ids and get the URL from DB
    //parse out the game information
    //save to DB
    _dataSync.ParseGamePages(new List<int>());

    return Ok();
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
