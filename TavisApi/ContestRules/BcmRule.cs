using System.Collections.ObjectModel;
using TavisApi.V2.Models;
using TavisApi.V2.TrueAchievements.Models;

namespace TavisApi.ContestRules;

public class BcmRule {
	public const int MaximumGameScore = 1500;
	public const double MinimumRatio = 1.5;
	public const int YearlyMinEstimate = 6;
	public const int RandomMaxEstimate = 200;
	public const int RandomMinimumEligibilityCount = 50;
	public const int RgscStartingRerolls = 5;
	public static readonly ReadOnlyCollection<Platform> RandomValidPlatforms =
			new ReadOnlyCollection<Platform>(
				new List<Platform> {
					Platform.Xbox360,
					Platform.XboxOne,
					Platform.XboxSeriesXS,
					Platform.Windows
				});

	public static readonly List<List<GenreList>> OddJobs = new List<List<GenreList>>
	{
		new List<GenreList> { GenreList.ArcadeRacing },
		new List<GenreList> { GenreList.CardAndBoard },
		new List<GenreList> { GenreList.OnRails },
		new List<GenreList> { GenreList.RealTime },
		new List<GenreList> { GenreList.Stealth },
	};

	// List of Games that recieved TU/DLC updates to exclude from monthly points
	public static readonly List<Game> UpdateExclusions = new List<Game>
	{
		new Game { Id = 0 },
	};

}
