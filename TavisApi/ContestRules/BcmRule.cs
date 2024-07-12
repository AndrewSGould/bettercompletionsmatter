using System.Collections.ObjectModel;
using Tavis.Models;

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
		new Game { Id = 2962 },
		new Game { Id = 2906 },
		new Game { Id = 9556 },
		new Game { Id = 9611 },
		new Game { Id = 10281 },
		new Game { Id = 9844 },
		new Game { Id = 9985 },
		new Game { Id = 9989 },
		new Game { Id = 3242 },
		new Game { Id = 10091 },
		new Game { Id = 10188 },
		new Game { Id = 5330 },
		new Game { Id = 10096 },
		new Game { Id = 10097 },
		new Game { Id = 10189 },
		new Game { Id = 3173 },
		new Game { Id = 9067 },
		new Game { Id = 10250 },
		new Game { Id = 10225 },
		new Game { Id = 10331 },
		new Game { Id = 10192 },
		new Game { Id = 10092 },
		new Game { Id = 10124 },
		new Game { Id = 7517 },
		new Game { Id = 2502 },
		new Game { Id = 9149 },
		new Game { Id = 10187 },
		new Game { Id = 10122 },
		new Game { Id = 10186 },
		new Game { Id = 10126 },
		new Game { Id = 10121 },
		new Game { Id = 10120 },
		new Game { Id = 10278 },
		new Game { Id = 10277 },
		new Game { Id = 10190 },
		new Game { Id = 7 },
		new Game { Id = 269 },
		new Game { Id = 2448 },
		new Game { Id = 352 },
		new Game { Id = 174 },
		new Game { Id = 295 },
		new Game { Id = 8695 },
		new Game { Id = 9632 },
		new Game { Id = 9792 },
		new Game { Id = 10247 },
		new Game { Id = 9979 },
		new Game { Id = 10279 },
		new Game { Id = 10280 },
		new Game { Id = 10276 },
		new Game { Id = 9133 },
		new Game { Id = 9956 },
		new Game { Id = 10248 },
		new Game { Id = 4640 },
		new Game { Id = 10141 },
		new Game { Id = 10135 },
		new Game { Id = 3059 },
		new Game { Id = 9381 },
		new Game { Id = 10341 },
	};
}
