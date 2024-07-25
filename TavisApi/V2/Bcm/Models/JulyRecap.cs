using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TavisApi.V2.Bcm.Models;

public class JulyRecap {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public int Rank { get; set; }
	public string Gamertag { get; set; } = string.Empty;
	public int TeaCount { get; set; } = 0;
	public bool CommunityBonus { get; set; } = false;
	public bool Participation { get; set; } = false;
	public double TotalPoints { get; set; } = 0.0;

	public long PlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
}
