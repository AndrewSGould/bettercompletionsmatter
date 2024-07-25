using TavisApi.V2.Models;

namespace TavisApi.V2.Bcm.Models;

public class MonthlyExclusion {
	public int Challenge { get; set; }

	public long PlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }

	public int? GameId { get; set; }
	public Game? Game { get; set; }
}
