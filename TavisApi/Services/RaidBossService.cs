using Tavis.Models;
using TavisApi.Context;
using static WebApi.Controllers.RaidBossController;

namespace TavisApi.Services;

public interface IRaidBossService
{
  List<Player> GetPlayers();
  double DetermineDamage(IQueryable<PlayerGameProfile> games);
}

public class RaidBossService : IRaidBossService 
{
  private TavisContext _context;

  public RaidBossService(TavisContext context) {
    _context = context;
  }

  public List<Player> GetPlayers() {
    var raidBossPlayers = _context.PlayerContests!.Where(x => x.ContestId == 2).Select(x => x.PlayerId);
    return _context.Players!.Where(x => x.IsActive && raidBossPlayers.Contains(x.Id)).ToList();
  }

  public double DetermineDamage(IQueryable<PlayerGameProfile> games) {
    if (games.Count() < 1) return 0;
    
    var totalNonBcDamage = X360NonBcDamage(games);
    var totalx360Damage = X360Damage(games);

    return totalNonBcDamage + totalx360Damage;
  }

  private double X360NonBcDamage(IQueryable<PlayerGameProfile> games) {
    var nonBcGames = games.Join(_context.FeatureLists!, g => g.Games!.Id, fl => fl.FeatureListOfGameId, (g, fl) =>
            new { PlayerGames = g, FeatureList = fl})
          .Where(x => x.FeatureList.NotBackwardsCompat);

    if (nonBcGames.Count() < 1) return 0;

    var tier1Games = nonBcGames.Where(x => x.PlayerGames.Games!.SiteRatio < 1.5).Count();
    var tier2Games = nonBcGames.Where(x => x.PlayerGames.Games!.SiteRatio >= 1.5 
                                  && x.PlayerGames.Games!.SiteRatio < 2.0).Count();
    var tier3Games = nonBcGames.Where(x => x.PlayerGames.Games!.SiteRatio >= 2.0 
                                  && x.PlayerGames.Games!.SiteRatio < 3.0).Count();
    var tier4Games = nonBcGames.Where(x => x.PlayerGames.Games!.SiteRatio >= 3.0 
                                  && x.PlayerGames.Games!.SiteRatio < 4.0).Count();
    var tier5Games = nonBcGames.Where(x => x.PlayerGames.Games!.SiteRatio >= 4.0).Count();

    var tier1Damage = tier1Games * 5;
    var tier2Damage = tier2Games * 10;
    var tier3Damage = tier3Games * 25;
    var tier4Damage = tier4Games * 35;
    var tier5Damage = tier5Games * 50;

    return (tier1Damage + tier2Damage + tier3Damage + tier4Damage + tier5Damage) * 1.5;
  }

  private double X360Damage(IQueryable<PlayerGameProfile> games) {
    var x360Games = games.Join(_context.FeatureLists!, g => g.Games!.Id, fl => fl.FeatureListOfGameId, (g, fl) =>
        new { PlayerGames = g, FeatureList = fl})
      .Where(x => !x.FeatureList.NotBackwardsCompat);

    if (x360Games.Count() < 1) return 0;

    var tier1Games = x360Games.Where(x => x.PlayerGames.Games!.SiteRatio < 1.5).Count();
    var tier2Games = x360Games.Where(x => x.PlayerGames.Games!.SiteRatio >= 1.5 
                                  && x.PlayerGames.Games!.SiteRatio < 2.0).Count();
    var tier3Games = x360Games.Where(x => x.PlayerGames.Games!.SiteRatio >= 2.0 
                                  && x.PlayerGames.Games!.SiteRatio < 3.0).Count();
    var tier4Games = x360Games.Where(x => x.PlayerGames.Games!.SiteRatio >= 3.0 
                                  && x.PlayerGames.Games!.SiteRatio < 4.0).Count();
    var tier5Games = x360Games.Where(x => x.PlayerGames.Games!.SiteRatio >= 4.0).Count();

    var tier1Damage = tier1Games * 5;
    var tier2Damage = tier2Games * 10;
    var tier3Damage = tier3Games * 25;
    var tier4Damage = tier4Games * 35;
    var tier5Damage = tier5Games * 50;

    return tier1Damage + tier2Damage + tier3Damage + tier4Damage + tier5Damage;
  }
}
