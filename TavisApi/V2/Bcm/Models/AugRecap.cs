using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TavisApi.V2.Bcm.Models;

public class AugRecap {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public int Rank { get; set; }
	public string Gamertag { get; set; } = string.Empty;
	public int? AchievementCount { get; set; }
	public bool BloodAngelTribute { get; set; } = false;
	public bool ImperialFistTribute { get; set; } = false;
	public bool SpaceWolvesTribute { get; set; } = false;
	public bool UltramarinesTribute { get; set; } = false;
	public bool DeathwatchTribute { get; set; } = false;
	public int CommunityBonus { get; set; }
	public bool Participation { get; set; } = false;
	public double TotalPoints { get; set; } = 0.0;

	public long PlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
}
