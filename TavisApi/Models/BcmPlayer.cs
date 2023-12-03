namespace Tavis.Models;

public class BcmPlayer
{
  public long Id { get; set; }
  public long UserId { get; set; }
  public User? User { get; set; }
  public int TrueAchievementId { get; set; } = 0;
  public DateTime? LastSync { get; set; }
  public ICollection<BcmPlayerGame>? BcmPlayerGames { get; set; }
  public ICollection<BcmPlayerCompletionHistory>? BcmPlayerCompletionHistories { get; set; }
  public BcmStat? BcmStats { get; set; }
  public ICollection<BcmRgsc>? BcmRgsc { get; set; }
}
