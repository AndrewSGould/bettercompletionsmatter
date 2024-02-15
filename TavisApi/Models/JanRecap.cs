using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tavis.Models;

public class JanRecap
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public int Rank { get; set; }
  public string Gamertag { get; set; } = string.Empty;
  public double? Bucket1Points { get; set; }
  public int? Bucket1Comps { get; set; }
  public double? Bucket2Points { get; set; }
  public int? Bucket2Comps { get; set; }
  public double? Bucket3Points { get; set; }
  public int? Bucket3Comps { get; set; }
  public double? Bucket4Points { get; set; }
  public int? Bucket4Comps { get; set; }
  public bool AllBuckets { get; set; }
  public bool CommunityBonus { get; set; }
  public double TotalPoints { get; set; } = 0.0;

  public long PlayerId { get; set; }
  public BcmPlayer? BcmPlayer { get; set; }
}
