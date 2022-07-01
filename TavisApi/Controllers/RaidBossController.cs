using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using static TavisApi.Services.DataSync;
using static TavisApi.Services.TA_GameCollection;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RaidBossController : ControllerBase {

  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;

  public RaidBossController(TavisContext context, IParser parser, IDataSync dataSync) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
  }

  [HttpGet]
  [Route("ta_sync")]
  public IActionResult Sync()
  {
    var raidBossPlayers = _context.PlayerContests!.Where(x => x.ContestId == 2).Select(x => x.PlayerId);
    var players = _context.Players!.Where(x => x.IsActive && raidBossPlayers.Contains(x.Id)).ToList();

    var gcOptions = new TA_GC_Options {
      CompletionStatus = TAGC_CompletionStatus.Complete,
      DateCutoff = new DateTime(2022, 6, 1)
    };

    return Ok(_dataSync.DynamicSync(players, gcOptions));
  }
}
