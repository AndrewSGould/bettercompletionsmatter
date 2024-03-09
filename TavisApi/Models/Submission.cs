using TavisApi.Data;

namespace Tavis.Models;

public class Submission
{
  public string Option { get; set; } = string.Empty;
  public PlayerYearlyChallenge? PlayerYearlyChallenge { get; set; }
  public YearlyChallenge? YearlyChallenge { get; set; }
}
