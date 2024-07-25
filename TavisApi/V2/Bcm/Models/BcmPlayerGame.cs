using TavisApi.V2.Models;
using TavisApi.V2.TrueAchievements.Models;

namespace TavisApi.V2.Bcm.Models;

public class BcmPlayerGame {
	public Platform? Platform { get; set; }
	public int? TrueAchievement { get; set; }
	public int? Gamerscore { get; set; }
	public int? AchievementCount { get; set; }
	public DateTime? StartedDate { get; set; }
	public DateTime? CompletionDate { get; set; }
	public DateTime? LastUnlock { get; set; }
	public Ownership? Ownership { get; set; }
	public bool NotForContests { get; set; } = false;
	public double? BcmPoints { get; set; }


	public long PlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }

	public int? GameId { get; set; }
	public Game? Game { get; set; }
}
