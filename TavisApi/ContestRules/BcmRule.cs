using System.Collections.ObjectModel;
using Tavis.Models;

namespace TavisApi.ContestRules;

public class BcmRule {
  public const double MinimumRatio = 1.2;
  public const int RandomMaxEstimate = 100;
  public const int RandomMinimumEligibilityCount = 50;
  public static readonly ReadOnlyCollection<Platform> RandomValidPlatforms = 
      new ReadOnlyCollection<Platform>(
        new List<Platform> {
          Platform.Xbox360,
          Platform.XboxOne,
          Platform.XboxSeriesXS,
          Platform.Windows
        });
}
