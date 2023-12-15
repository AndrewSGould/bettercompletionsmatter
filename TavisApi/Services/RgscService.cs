using Microsoft.EntityFrameworkCore;
using TavisApi.ContestRules;
using TavisApi.Context;

namespace TavisApi.Services;

public class RgscService : IRgscService
{
  private TavisContext _context;

  public RgscService(TavisContext context)
  {
    _context = context;
  }

  public int GetUserRerollCount(long userId)
  {
    var startingCount = BcmRule.RgscStartingRerolls;

    var user = _context.Users.Include(x => x.BcmPlayer).First(x => x.Id == userId);
    var bcmPlayer = _context.BcmPlayers.Include(x => x.BcmRgscs).Include(x => x.BcmPlayerGames).FirstOrDefault(x => x.Id == user.BcmPlayer!.Id);

    var completedRgscsCount = bcmPlayer!.BcmPlayerGames?
      .Where(game => game.CompletionDate != null)
      .Select(game => game.GameId)
      .Where(gameId => bcmPlayer.BcmRgscs != null && bcmPlayer.BcmRgscs.Any(rgsc => rgsc.GameId == gameId))
      .Count() ?? 0;

    var rerollCount = bcmPlayer!.BcmRgscs?.Where(x => x.Rerolled).Count() ?? 0;

    return startingCount + completedRgscsCount - rerollCount;
  }
}
