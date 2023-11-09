namespace Tavis.Models;

public class RgscResult
{
  public string Player { get; set; }
  public string RandomGame { get; set; }
  public int EligibleCount { get; set; }
  public List<string> GameList { get; set; }
}
