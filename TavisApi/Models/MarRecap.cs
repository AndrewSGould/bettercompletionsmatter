using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tavis.Models;

public class MarRecap
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public int Rank { get; set; }
  public string Gamertag { get; set; } = string.Empty;
  public string BestBounty { get; set; } = string.Empty;
  public int BountyCount { get; set; } = 0;
  public string BountiesClaimed { get; set; } = string.Empty;
  public bool CommunityBonus { get; set; } = false;
  public bool Participation { get; set; } = false;
  public double TotalPoints { get; set; } = 0.0;

  public long PlayerId { get; set; }
  public BcmPlayer? BcmPlayer { get; set; }
}
