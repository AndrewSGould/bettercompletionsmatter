using Tavis.Models;
using TavisApi.ContestRules;

namespace Tavis.Extensions;

public static class Queries
{
  public static bool FilterGamesForYearlies(Game game)
  {
    return game.SiteRatio > BcmRule.MinimumRatio
              && (game.FullCompletionEstimate > BcmRule.YearlyMinEstimate || game.FullCompletionEstimate == null)
              || game.FullCompletionEstimate >= 20;
  }

  public static bool FilterCompletedPlayerGames(PlayerGame playerGame)
  {
    return playerGame.CompletionDate != null && playerGame.CompletionDate.Value.Year == DateTime.Now.Year;
  }
}
