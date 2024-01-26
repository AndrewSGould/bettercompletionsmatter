using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;
using Tavis.Models;
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

  public List<BcmPlayerGame>? GetEligibleRandoms(BcmPlayer player)
  {
    var randomGameOptions = _context.BcmPlayerGames?
            .Include(x => x.Game)
            .Where(x => x.PlayerId == player.Id
              && x.Game!.GamersCompleted > 0
              && x.Game!.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
              && !x.Game!.Unobtainables
              && !x.NotForContests
              && x.CompletionDate == null
              && x.Ownership != Ownership.NoLongerHave
              && BcmRule.RandomValidPlatforms.Contains(x.Platform!))
            .AsEnumerable() // TODO: rewrite so this stays as a query?
            .Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
            .ToList();

    var currentRandoms = _context.BcmRgsc.Where(x => !x.Rerolled && x.BcmPlayerId == player.Id);

    return randomGameOptions?
      .Where(x => !currentRandoms.Any(y => y.GameId == x.Game!.Id))
      .ToList();
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
