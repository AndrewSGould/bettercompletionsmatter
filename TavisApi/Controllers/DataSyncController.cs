namespace WebApi.Controllers;

using System.Diagnostics;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using static TavisApi.Services.DataSync;
using static TavisApi.Services.TA_GameCollection;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class DataSyncController : ControllerBase {
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;
  private readonly IHubContext<SyncSignal> _hub;
  private readonly IBcmService _bcmService;

  public DataSyncController(TavisContext context, IParser parser, IDataSync dataSync, IHubContext<SyncSignal> hub, IBcmService bcmService) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _hub = hub;
    _bcmService = bcmService;
  }

  // Gets the stats of the scan, runtime, players scanned, etc
  [HttpGet, Authorize(Roles = "Super Admin")]
  [Route("syncInfo")]
  public IActionResult SyncInfo() {
    var playersToScan = _bcmService.GetPlayers().Count();

    var syncs = _context.SyncHistory!.Where(x => x.Profile == SyncProfileList.Full);
    // var averageHits = (syncs.Average(x => x.TaHits) / syncs.Average(x => x.PlayerCount)) * playersToScan;
    // var averageRuntime = (syncs.Average(x => (x.End! - x.Start!).Value.TotalSeconds) / syncs.Average(x => x.PlayerCount) * playersToScan);

    return Ok(new {
      PlayerCount = playersToScan,
      // EstimatedRuntime = averageRuntime,
      // EstimatedTaHits = averageHits
    });
  }

  // Does a full scan of, currently, BCM players
  // parameters can be customized to limit game collection size
  // to reduce the amount of hits on TA
  [HttpGet, Authorize(Roles = "Super Admin")]
  [Route("full")]
  public IActionResult Sync()
  {
    var playersToScan = _bcmService.GetPlayers();

    var syncLog = new SyncHistory {
      Start = DateTime.UtcNow,
      PlayerCount = playersToScan.Count(),
      Profile = SyncProfileList.Full
    };

    // no options, sync everything
    var gcOptions = new SyncOptions {
    };

    var results = _dataSync.DynamicSync(playersToScan.OrderBy(x => x.Name).ToList(), gcOptions, syncLog, _hub);

    syncLog.End = DateTime.UtcNow;
    _context.SyncHistory!.Add(syncLog);
    _context.SaveChanges();

    return Ok();
  }

  // does a scan of completed games within the past month only
  [HttpGet, Authorize(Roles = "Super Admin")]
  [Route("lastmonthscompletions")]
  public IActionResult SyncLastMonthsCompletions() 
  {
    var playersToScan = _bcmService.GetPlayers();

    var syncLog = new SyncHistory {
      Start = DateTime.UtcNow,
      PlayerCount = playersToScan.Count(),
      Profile = SyncProfileList.LastMonthsCompleted
    };

    // TODO: move this to private method and unit test
    var now = DateTime.Now;    
    var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
    var lastDayLastMonth = firstDayCurrentMonth.AddMonths(-1).AddDays(-1);

    var gcOptions = new SyncOptions {
      ContestStatus = SyncOption_ContestStatus.All,
      LastUnlockCutoff = lastDayLastMonth,
      TimeZone = SyncOption_Timezone.EST
    };

    var results = _dataSync.DynamicSync(playersToScan.OrderBy(x => x.Name).ToList(), gcOptions, syncLog, _hub);

    syncLog.End = DateTime.UtcNow;
    _context.SyncHistory!.Add(syncLog);
    _context.SaveChanges();

    return Ok();
  }

  //TODO: this is here just to test
  //eventually, pull this out to the service only
  //and ONLY call this when the user requests it
  //---------------------
  //this goes to all game pages to update the ancillary info
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

  // goes to a special TA page and marks
  // all games present as GwG
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
