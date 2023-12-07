namespace Tavis.Models;

public class Leaderboard
{
  public BcmPlayer BcmPlayer { get; set; } = new();
  public BcmStat? BcmStats { get; set; } = new();
}
