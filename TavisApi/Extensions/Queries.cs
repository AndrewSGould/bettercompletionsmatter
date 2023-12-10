using Tavis.Models;
using TavisApi.ContestRules;

namespace Tavis.Extensions;

public static class Queries
{
  public static bool FilterGamesForYearlies(Game game, BcmPlayerGame pGame)
  {
    if (pGame.Platform == Platform.Xbox360.Value)
      return ((game.SiteRatio + 0.5) >= BcmRule.MinimumRatio
              && game.FullCompletionEstimate > BcmRule.YearlyMinEstimate)
              || game.FullCompletionEstimate >= 15;
    else
      return (game.SiteRatio >= BcmRule.MinimumRatio
              && game.FullCompletionEstimate > BcmRule.YearlyMinEstimate)
              || game.FullCompletionEstimate >= 15;
  }

  public static bool FilterCompletedPlayerGames(BcmPlayerGame playerGame)
  {
    return playerGame.CompletionDate != null && playerGame.CompletionDate.Value.Year == DateTime.UtcNow.Year;
  }
}
