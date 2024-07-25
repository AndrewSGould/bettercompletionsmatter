using TavisApi.ContestRules;
using TavisApi.V2.Bcm.Models;
using TavisApi.V2.Models;
using TavisApi.V2.TrueAchievements.Models;

namespace Tavis.Extensions;

public static class Queries {
	public static bool FilterGamesForYearlies(Game game, BcmPlayerGame pGame)
	{
		if (pGame.Platform == Platform.Xbox360.Value)
			return ((game.SiteRatio + 0.5) >= BcmRule.MinimumRatio
							&& game.FullCompletionEstimate >= BcmRule.YearlyMinEstimate)
							|| game.FullCompletionEstimate >= 20;
		else
			return (game.SiteRatio >= BcmRule.MinimumRatio
							&& game.FullCompletionEstimate >= BcmRule.YearlyMinEstimate)
							|| game.FullCompletionEstimate >= 20;
	}

	public static bool FilterCompletedPlayerGames(BcmPlayerGame playerGame)
	{
		// todo: fix this and use it
		var userRegDate = DateTime.Now;  // UserRegistrations.First(x => x.RegistrationId == 1).RegistrationDate;

		return playerGame.CompletionDate != null && playerGame.CompletionDate > userRegDate;
	}
}
