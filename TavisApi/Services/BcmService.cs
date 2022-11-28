using Tavis.Models;
using TavisApi.Context;

namespace TavisApi.Services;

public class BcmService : IBcmService 
{
  private TavisContext _context;

  public BcmService(TavisContext context) {
    _context = context;
  }

  public List<Player> GetPlayers() {
    var bcmPlayers = _context.PlayerContests!.Where(x => x.ContestId == 1).Select(x => x.PlayerId);
    return _context.Players!.Where(x => x.IsActive && bcmPlayers.Contains(x.Id)).OrderBy(x => x.Name).ToList();
  }
}
