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
    new Game { Id = 335 },
    new Game { Id = 5588 },
    new Game { Id = 9285 },
    new Game { Id = 8335 },
    new Game { Id = 4632 },
    new Game { Id = 1430 },
    new Game { Id = 6592 },
    new Game { Id = 3235 },
    new Game { Id = 5897 },
    new Game { Id = 9588 },
    new Game { Id = 9547 },
    new Game { Id = 7206 },
    new Game { Id = 2711 },
    new Game { Id = 9528 },
    new Game { Id = 9545 },
    new Game { Id = 9532 },
    new Game { Id = 9067 },
    new Game { Id = 9029 },
    new Game { Id = 6256 },
    new Game { Id = 3172 },
    new Game { Id = 9435 },
    new Game { Id = 6282 },
    new Game { Id = 5328 },
    new Game { Id = 6281 },
    new Game { Id = 5907 },
    new Game { Id = 4630 },
    new Game { Id = 5895 },
    new Game { Id = 9470 },
    new Game { Id = 3254 },
    new Game { Id = 9219 },
    new Game { Id = 7531 },
    new Game { Id = 8416 },
    new Game { Id = 8765 },
    new Game { Id = 4601 },
    new Game { Id = 6283 },
    new Game { Id = 5315 },
    new Game { Id = 7558 },
    new Game { Id = 9021 },
    new Game { Id = 8766 },
  };
}
