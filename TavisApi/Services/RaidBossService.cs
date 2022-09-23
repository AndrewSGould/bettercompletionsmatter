using Tavis.Models;
using TavisApi.Context;
using static WebApi.Controllers.RaidBossController;

namespace TavisApi.Services;

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
}
