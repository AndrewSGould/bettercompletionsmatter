using System.Collections.ObjectModel;
using Tavis.Models;

namespace TavisApi.ContestRules;

public class BcmRule
{
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
		new Game { Id = 5186 },
		new Game { Id = 5601 },
		new Game { Id = 3166 },
		new Game { Id = 9884 },
		new Game { Id = 5104 },
		new Game { Id = 5882 },
		new Game { Id = 2451 },
		new Game { Id = 3208 },
		new Game { Id = 2425 },
		new Game { Id = 5891 },
		new Game { Id = 369 },
		new Game { Id = 9470 },
		new Game { Id = 9381 },
		new Game { Id = 9633 },
		new Game { Id = 9791 },
		new Game { Id = 9797 },
		new Game { Id = 5901 },
		new Game { Id = 7540 },
		new Game { Id = 5325 },
		new Game { Id = 5903 },
		new Game { Id = 5260 },
		new Game { Id = 9795 },
		new Game { Id = 2 },
		new Game { Id = 1651 },
		new Game { Id = 10 },
		new Game { Id = 2427 },
		new Game { Id = 5553 },
		new Game { Id = 2566 },
		new Game { Id = 9029 },
		new Game { Id = 9285 },
		new Game { Id = 8414 },
		new Game { Id = 9050 },
		new Game { Id = 2437 },
		new Game { Id = 2434 },
		new Game { Id = 9700 },
		new Game { Id = 9677 },
		new Game { Id = 9701 },
		new Game { Id = 2906 },
		new Game { Id = 3203 },
		new Game { Id = 3670 },
		new Game { Id = 8823 },
		new Game { Id = 9883 },
		new Game { Id = 7227 },
		new Game { Id = 5588 },
		new Game { Id = 3251 },
		new Game { Id = 7326 },
		new Game { Id = 4647 },
		new Game { Id = 5300 },
		new Game { Id = 2423 },
		new Game { Id = 335 },
		new Game { Id = 2829 },
		new Game { Id = 8183 },
		new Game { Id = 9384 },
		new Game { Id = 9413 }
	};
}
