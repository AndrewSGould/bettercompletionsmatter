using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Office2016.Excel;
using Tavis.Models;

namespace TavisApi.ContestRules;

public class BcmRule
{
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
    new List<GenreList> { GenreList.Collection },
    new List<GenreList> { GenreList.DungeonCrawler },
    new List<GenreList> { GenreList.Management },
    new List<GenreList> { GenreList.Motocross },
    new List<GenreList> { GenreList.RunAndGun },
  };
}
