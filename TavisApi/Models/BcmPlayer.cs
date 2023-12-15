using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tavis.Models;

public class BcmPlayer
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public long UserId { get; set; }
  public User? User { get; set; }
  public int TrueAchievementId { get; set; } = 0;
  public DateTime? LastSync { get; set; }
  public ICollection<BcmPlayerGame>? BcmPlayerGames { get; set; }
  public ICollection<BcmPlayerCompletionHistory>? BcmPlayerCompletionHistories { get; set; }
  public BcmStat? BcmStats { get; set; }
  public ICollection<BcmRgsc>? BcmRgscs { get; set; }
}
