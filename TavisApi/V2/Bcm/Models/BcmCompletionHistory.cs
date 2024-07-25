using TavisApi.V2.Models;

namespace TavisApi.V2.Bcm.Models;

public class BcmCompletionHistory {
	public int Id { get; set; } = 0;
	public int? GameId { get; set; }
	public double? SiteRatio { get; set; }

	public Game? Game { get; set; }
}
