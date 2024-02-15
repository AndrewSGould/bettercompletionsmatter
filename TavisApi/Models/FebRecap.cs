using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tavis.Models;

public class FebRecap
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public int Rank { get; set; }
  public string Gamertag { get; set; } = string.Empty;
  public int BiCompletion { get; set; } = 0;
  public double BiPoints { get; set; } = 0;
  public int TriCompletion { get; set; } = 0;
  public double TriPoints { get; set; } = 0;
  public int QuadCompletion { get; set; } = 0;
  public double QuadPoints { get; set; } = 0;
  public int QuintCompletion { get; set; } = 0;
  public double QuintPoints { get; set; } = 0;
  public int SexCompletion { get; set; } = 0;
  public double SexPoints { get; set; } = 0;
  public int SepCompletion { get; set; } = 0;
  public double SepPoints { get; set; } = 0;
  public int OctCompletion { get; set; } = 0;
  public double OctPoints { get; set; } = 0;
  public int DecCompletion { get; set; } = 0;
  public double DecPoints { get; set; } = 0;
  public int UndeCompletion { get; set; } = 0;
  public double UndePoints { get; set; } = 0;
  public int DuodeCompletion { get; set; } = 0;
  public double DuodePoints { get; set; } = 0;
  public bool CommunityBonus { get; set; } = false;
  public bool Participation { get; set; } = false;
  public double TotalPoints { get; set; } = 0.0;

  public long PlayerId { get; set; }
  public BcmPlayer? BcmPlayer { get; set; }
}
