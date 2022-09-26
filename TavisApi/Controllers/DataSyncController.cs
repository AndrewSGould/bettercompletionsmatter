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

  public DataSyncController(TavisContext context, IParser parser, IDataSync dataSync, IHubContext<SyncSignal> hub) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _hub = hub;
  }

  [HttpGet, Authorize(Roles = "Super Admin")]
  [Route("syncInfo")]
  public IActionResult SyncInfo() {
    // return players to be scanned
    // return average estimated time
    var syncs = _context.SyncHistory!.Where(x => x.Profile == SyncProfileList.Full);
    var averageHits = (syncs.Average(x => x.TaHits) / syncs.Average(x => x.PlayerCount)) * BcmController.HhPlayers.Count();
    var averageRuntime = syncs.Average(x => (x.End! - x.Start!).Value.TotalSeconds);

    return Ok(new {
      PlayerCount = BcmController.HhPlayers.Count(),
      EstimatedRuntime = averageRuntime,
      EstimatedTaHits = averageHits
    });
  }

  [HttpGet, Authorize(Roles = "Super Admin")]
  [Route("full")]
  public IActionResult Sync()
  {
    var hhPlayers = BcmController.HhPlayers;
    var playersToScan = new List<Player>();

    var players = _context.PlayerContests!.Where(x => x.ContestId == 1).Select(x => x.PlayerId);
    var bcmPlayers = _context.Players!.Where(x => x.IsActive).ToList();
    //var bcmPlayers = _bcmService.GetPlayers();

    var syncLog = new SyncHistory {
      Start = DateTime.UtcNow,
      PlayerCount = hhPlayers.Count(),
      Profile = SyncProfileList.Full
    };

    foreach (var player in bcmPlayers) {
      if (hhPlayers.Any(x => x.Player == player.Name))
        playersToScan.Add(player);
    }

    //TODO: for now, this sync is setup to only pull random games
    //  in the future, have the random game endpoint accept a sync option
    var gcOptions = new SyncOptions {
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
