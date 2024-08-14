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
		new Game { Id = 9809 },
		new Game { Id = 10129 },
		new Game { Id = 9700 },
		new Game { Id = 9908 },
		new Game { Id = 9387 },
		new Game { Id = 10247 },
		new Game { Id = 7317 },
		new Game { Id = 4591 },
		new Game { Id = 10405 },
		new Game { Id = 9634 },
		new Game { Id = 8826 },
		new Game { Id = 10339 },
		new Game { Id = 9382 },
		new Game { Id = 10373 },
		new Game { Id = 9545 },
		new Game { Id = 8141 },
		new Game { Id = 9701 },
		new Game { Id = 9422 },
		new Game { Id = 9632 },
		new Game { Id = 9697 },
		new Game { Id = 9979 },
		new Game { Id = 9989 },
		new Game { Id = 10340 },
		new Game { Id = 9383 },
		new Game { Id = 2906 },
		new Game { Id = 9581 },
		new Game { Id = 6281 },
		new Game { Id = 2829 },
		new Game { Id = 7558 },
		new Game { Id = 8416 },
		new Game { Id = 8765 },
		new Game { Id = 8825 },
		new Game { Id = 9542 },
		new Game { Id = 9677 },
		new Game { Id = 9528 },
		new Game { Id = 10337 },
		new Game { Id = 10381 },
		new Game { Id = 5347 },
		new Game { Id = 5892 },
		new Game { Id = 5601 },
		new Game { Id = 4636 },
		new Game { Id = 4639 },
		new Game { Id = 8766 },
		new Game { Id = 1430 },
		new Game { Id = 3251 },
		new Game { Id = 4633 },
		new Game { Id = 2962 },
		new Game { Id = 3172 },
		new Game { Id = 3231 },
		new Game { Id = 5186 },
		new Game { Id = 5328 },
		new Game { Id = 5359 },
		new Game { Id = 5381 },
		new Game { Id = 4601 },
		new Game { Id = 6282 },
		new Game { Id = 6283 },
		new Game { Id = 3670 },
		new Game { Id = 3688 },
		new Game { Id = 6285 },
		new Game { Id = 9021 },
		new Game { Id = 9044 },
		new Game { Id = 9956 },
		new Game { Id = 9985 },
		new Game { Id = 10096 },
		new Game { Id = 10397 },
		new Game { Id = 5189 },
		new Game { Id = 5299 },
		new Game { Id = 5907 },
		new Game { Id = 7531 },
};

}
