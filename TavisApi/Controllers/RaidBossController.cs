using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tavis.Models;
using TavisApi.ContestRules;
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
    var raidBossPlayerList = _raidBoss.GetPlayers();

    Console.WriteLine($"Tavis has begun the scan at {DateTime.Now} EST");
    Console.WriteLine($"Scanning {raidBossPlayerList.Count()} player(s)");
    Console.WriteLine("-----------------");

    var gcOptions = new SyncOptions {
      CompletionStatus = SyncOption_CompletionStatus.Complete,
      DateCutoff = new DateTime(2022,5,30), //_context.Contests!.First(x => x.Id == 2).StartDate,
      TimeZone = "Greenwich%20Mean%20Time"
    };

    _dataSync.DynamicSync(raidBossPlayerList, gcOptions);

    CalculateDamage();

    return Ok();
  }

  [HttpGet]
  [Route("calculateDamage")]
  public IActionResult CalculateDamage() {
    var raidBossPlayerList = _raidBoss.GetPlayers();
    var raidBossIds = raidBossPlayerList.Select(x => x.Id).ToList();

    var damages = new List<DamageResponse>();
    var todaysHits = new List<object>();
    var absoluteStart = DateTime.Today;
    var raidBossContest = _context.Contests!.First(x => x.Id == 2);

    foreach(var player in raidBossPlayerList) {
      var raidBossPlayer = new RaidBossRule().GenerateRaidPlayer(player);
      var applicableGames = _context.PlayerGames!
                              .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => 
                                new PlayerGameProfile { Games = g, PlayerGames = pg})
                              .Where(x => x.PlayerGames!.PlayerId == player.Id 
                                && x.PlayerGames.CompletionDate > raidBossContest.StartDate
                                && x.PlayerGames.Platform == Platform.Xbox360);  

      var dmgDoneToday = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate >= absoluteStart.AddDays(-1)));

      damages.Add(new DamageResponse{
        PlayerName = player.Name,
        TotalDamage = _raidBoss.DetermineDamage(applicableGames),
        DmgWithLevel = new RaidBossRule().CalcDamageWithLevelModifier(_raidBoss.DetermineDamage(applicableGames), raidBossPlayer),
        TotalAttacks = applicableGames.Count(),
        CompletedGames = applicableGames.Select(x => x.Games!.Title).ToList(),
        DamageDoneToday = dmgDoneToday
      });
      
      var gamesCompletedToday = applicableGames.Where(x => x.PlayerGames!.CompletionDate >= absoluteStart.AddDays(-1)).OrderBy(x => x.Games!.Title).Select(x => x.Games!.Title);

      if (gamesCompletedToday.Count() > 0) {
        var prettiedGamesCompleted = string.Join(", ", gamesCompletedToday.Take(gamesCompletedToday.Count() - 1)) +
                                      (gamesCompletedToday.Count() > 1 ? " and " : "") + gamesCompletedToday.LastOrDefault();
      

        var totalDailyDamage = new RaidBossRule().CalcDamageWithLevelModifier(dmgDoneToday, raidBossPlayer);
        Console.WriteLine($"{raidBossPlayer.Name} {raidBossPlayer.AttackApproach} {totalDailyDamage} damage! ({prettiedGamesCompleted})\n");
      }
    }

    var playersDamageWithLevel = damages.Select(x => x.DmgWithLevel);
    double? bossDamage = 0.0;
    foreach(var lvlDmg in playersDamageWithLevel) {
      bossDamage += lvlDmg;
    }

    Console.WriteLine("-----------------");
    Console.WriteLine($"Scan completed at {DateTime.Now} EST");
    Console.WriteLine($"The boss now has {RaidBossRule.RaidBossHp - bossDamage} HP remaining!");

    return Ok(new {
      Full = damages.OrderBy(x => x.PlayerName),
      HitsOnly = damages.Where(x => x.TotalAttacks != 0).OrderBy(x => x.PlayerName),
      StrongestHitsOfTheDay = damages.OrderBy(x => x.DamageDoneToday)
    });

    // var applicableGames = _context.PlayerGames!
    //                     .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => 
    //                       new PlayerGameProfile { Games = g, PlayerGames = pg})
    //                     .Where(x => 
    //                       x.PlayerGames!.CompletionDate > raidBossContest.StartDate
    //                       && raidBossIds.Contains(x.PlayerGames.PlayerId!.Value)
    //                       && x.PlayerGames!.CompletionDate != null
    //                       && x.PlayerGames.Platform == Platform.Xbox360);

    // var first = _raidBoss.DetermineDamage(applicableGames.Where(x => DateTime.Compare(x.PlayerGames!.CompletionDate.Value, new DateTime(2022, 7, 1)) == 0));
    // var second = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 2)));
    // var third = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 3)));
    // var fourth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 4)));
    // var fifth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 5)));
    // var sixth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 6)));
    // var seventh = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 7)));
    // var eighth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 8)));
    // var ninth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 9)));
    // var tenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 10)));
    // var eleventh = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 11)));
    // var twelth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 12)));
    // var thirteenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 13)));
    // var fourteenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 14)));
    // var fifteenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 15)));
    // var sixteenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 16)));
    // var seventeeth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 17)));
    // var eighteenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 18)));
    // var ninteenth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 19)));
    // var twentieth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 20)));
    // var twentyfirst = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 21)));
    // var twentysecond = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 22)));
    // var twentythird = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 23)));
    // var twentyfourth = _raidBoss.DetermineDamage(applicableGames.Where(x => x.PlayerGames!.CompletionDate == new DateTime(2022, 7, 24)));

    // return Ok();
  }

  private class DamageResponse {
    public string? PlayerName {get;set;}
    public double? TotalDamage {get;set;}
    public int? TotalAttacks {get;set;}
    public double? DmgWithLevel {get;set;}
    public List<string?>? CompletedGames {get;set;}
    public double? DamageDoneToday {get;set;}
  }

  public class PlayerGameProfile {
    public PlayerGame? PlayerGames {get;set;}
    public Game? Games {get;set;}
  }
}
