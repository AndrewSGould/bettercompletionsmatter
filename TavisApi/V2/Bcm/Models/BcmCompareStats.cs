using TavisApi.V2.Models;

namespace TavisApi.V2.Bcm.Models;

public class BcmCompareStats {
	public string? Avatar { get; set; }
	public string Region { get; set; } = "N/A";
	public double? TotalBaseBcmPoints { get; set; }
	public double? TotalBonusPoints { get; set; }
	public double? AverageMonthlyPoints { get; set; }
	public Game? HighestRatioGame { get; set; }
	public Game? LongestGame { get; set; }
	public Game? BestBcmGame { get; set; }
	public double? BestBcmGamePoints { get; set; }
	public int BcmRank { get; set; }
}

