using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tavis.Models;

public class BcmStat
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public int? Rank { get; set; }
  public int? RankMovement { get; set; }
  public int? Completions { get; set; }
  public double? AverageRatio { get; set; }
  public double? HighestRatio { get; set; }
  public double? AverageTimeEstimate { get; set; }
  public double? HighestTimeEstimate { get; set; }
  public double? AveragePoints { get; set; }
  public double? BasePoints { get; set; }
  public double? BonusPoints { get; set; }
  public double? TotalPoints { get; set; }

  public long PlayerId { get; set; }
  public BcmPlayer? BcmPlayer { get; set; }
}
