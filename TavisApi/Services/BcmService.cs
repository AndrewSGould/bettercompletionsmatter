using Tavis.Models;
using TavisApi.Context;

namespace TavisApi.Services;

public class BcmService : IBcmService 
{
  private TavisContext _context;
  private int _bcmContestId;

  public BcmService(TavisContext context) {
    _context = context;
    _bcmContestId = GetContestId();
  }

  public List<Player> GetPlayers() {
    var bcmPlayers = _context.PlayerContests!.Where(x => x.ContestId == _bcmContestId).Select(x => x.PlayerId);
    return _context.Players!.Where(x => x.IsActive && bcmPlayers.Contains(x.Id)).OrderBy(x => x.Name).ToList();
  }

  public DateTime? GetContestStartDate() {
    return _context.Contests.Where(x => x.Id == _bcmContestId).Select(x => x.StartDate).FirstOrDefault();
  }

  public double? CalcBcmValue(double? ratio, double? estimate) {
    var rawPoints = Math.Pow((double)ratio, 1.5) * estimate;
    return rawPoints >= 1500 ? 1500 : rawPoints;
  }

  private int GetContestId() {
    return _context.Contests.Where(x => x.Name.Contains("Better Completions Matter")).First().Id;
  }
}
