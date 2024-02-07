using TavisApi.Data;

namespace Tavis.Models;

public class WriteInOption
{
  public string Option { get; set; } = string.Empty;
  public PlayerYearlyChallenge? PlayerYearlyChallenge { get; set; }
  public YearlyChallenge? YearlyChallenge { get; set; }
}
