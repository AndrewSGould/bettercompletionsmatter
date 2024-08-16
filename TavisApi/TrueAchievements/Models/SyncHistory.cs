using Tavis.Models;

namespace TavisApi.TrueAchievements.Models;

public class SyncHistory {
	public ulong Id { get; set; } = 0;
	public DateTime? Start { get; set; }
	public DateTime? End { get; set; }
	public int? PlayerCount { get; set; }
	public int? TaHits { get; set; }
	public SyncProfileList? Profile { get; set; }
}
