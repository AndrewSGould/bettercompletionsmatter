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
	public static readonly List<Game> UpdateExclusions = new List<Game> {
		new Game { Id = 326 },
		new Game { Id = 346 },
		new Game { Id = 2999 },
		new Game { Id = 3104 },
		new Game { Id = 3106 },
		new Game { Id = 3159 },
		new Game { Id = 3203 },
		new Game { Id = 3251 },
		new Game { Id = 3688 },
		new Game { Id = 8154 },
		new Game { Id = 9411 },
		new Game { Id = 10343 },
		new Game { Id = 10374 },
		new Game { Id = 10435 },
		new Game { Id = 10439 },
		new Game { Id = 10559 },
		new Game { Id = 10560 },
		new Game { Id = 10561 },
		new Game { Id = 10634 },
		new Game { Id = 10642 },
		new Game { Id = 10644 },
		new Game { Id = 10645 },
		new Game { Id = 10646 },
		new Game { Id = 10649 },
		new Game { Id = 10650 },
		new Game { Id = 10674 },
		new Game { Id = 10675 },
		new Game { Id = 10676 },
		new Game { Id = 10677 },
		new Game { Id = 10679 },
		new Game { Id = 10680 },
		new Game { Id = 10682 },
		new Game { Id = 10683 },
		new Game { Id = 10685 },
		new Game { Id = 10689 },
		new Game { Id = 10690 },
		new Game { Id = 10691 },
		new Game { Id = 10692 },
		new Game { Id = 10694 },
		new Game { Id = 10695 },
		new Game { Id = 10696 },
		new Game { Id = 10697 },
		new Game { Id = 10698 },
		new Game { Id = 10703 },
		new Game { Id = 10705 },
		new Game { Id = 10706 },
		new Game { Id = 10707 },
		new Game { Id = 10708 },
		new Game { Id = 10710 },
		new Game { Id = 10711 },
		new Game { Id = 10712 },
		new Game { Id = 10713 },
		new Game { Id = 10714 },
		new Game { Id = 10733 },
		new Game { Id = 10742 },
		new Game { Id = 10743 },
		new Game { Id = 10752 },
		new Game { Id = 10753 },
		new Game { Id = 10754 },
		new Game { Id = 10755 },
		new Game { Id = 10756 },
		new Game { Id = 10766 },
		new Game { Id = 10767 },
		new Game { Id = 10768 },
		new Game { Id = 10769 },
		new Game { Id = 10771 },
		new Game { Id = 10782 },
		new Game { Id = 10785 },
		new Game { Id = 10786 },
		new Game { Id = 10787 },
		new Game { Id = 10788 },
		new Game { Id = 10789 },
		new Game { Id = 10790 },
		new Game { Id = 10791 },
		new Game { Id = 10793 },
		new Game { Id = 10795 },
		new Game { Id = 10802 },
		new Game { Id = 10808 },
		new Game { Id = 10813 },
		new Game { Id = 10820 },
	};


}
