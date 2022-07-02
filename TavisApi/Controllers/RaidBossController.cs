using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tavis.Models;
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
  private readonly IRaidBossService _raidBoss;

  public RaidBossController(TavisContext context, IParser parser, IDataSync dataSync, IRaidBossService raidBoss) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _raidBoss = raidBoss;
  }

  [HttpGet]
  [Route("ta_sync")]
  public IActionResult Sync()
  {
    var raidBossPlayers = _context.PlayerContests!.Where(x => x.ContestId == 2).Select(x => x.PlayerId);
    var players = _context.Players!.Where(x => x.IsActive && raidBossPlayers.Contains(x.Id)).ToList();

    var gcOptions = new TA_GC_Options {
      CompletionStatus = TAGC_CompletionStatus.Complete,
      DateCutoff = _context.Contests!.First(x => x.Id == 2).StartDate,
      TimeZone = "Greenwich%20Mean%20Time"
    };

    return Ok(_dataSync.DynamicSync(players, gcOptions));
  }

  [HttpGet]
  [Route("calculateDamage")]
  public IActionResult CalculateDamage() {
    var raidBossPlayerList = _raidBoss.GetPlayers();

    var damages = new List<DamageResponse>();

    foreach(var player in raidBossPlayerList) {
    var raidBossContest = _context.Contests!.First(x => x.Id == 2);
    var applicableGames = _context.PlayerGames!
                            .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => 
                              new PlayerGameProfile { Games = g, PlayerGames = pg})
                            .Where(x => x.PlayerGames!.PlayerId == player.Id 
                              && x.PlayerGames.CompletionDate > raidBossContest.StartDate
                              && x.PlayerGames.Platform == Platform.Xbox360);  

      damages.Add(new DamageResponse{
        PlayerName = player.Name,
        TotalDamage = _raidBoss.DetermineDamage(applicableGames),
        TotalAttacks = applicableGames.Count(),
        CompletedGames = applicableGames.Select(x => x.Games!.Title).ToList()
      });
    }

    return Ok(damages);
  }

  private class DamageResponse {
    public string? PlayerName {get;set;}
    public double? TotalDamage {get;set;}
    public int? TotalAttacks {get;set;}
    public List<string?>? CompletedGames {get;set;}
  }

  public class PlayerGameProfile {
    public PlayerGame? PlayerGames {get;set;}
    public Game? Games {get;set;}
  }
}
