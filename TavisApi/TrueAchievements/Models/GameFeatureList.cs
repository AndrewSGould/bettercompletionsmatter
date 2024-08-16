using TavisApi.Models;

namespace TavisApi.TrueAchievements.Models;

public class FeatureList {
	public ulong Id { get; set; } = 0;
	public bool OneXEnhanced { get; set; } = false;
	public bool BackwardsCompat { get; set; } = false;
	public bool NotBackwardsCompat { get; set; } = false;
	public bool PlayAnywhere { get; set; } = false;
	public bool SmartDelivery { get; set; } = false;
	public bool OptimizedForSeries { get; set; } = false;
	public bool Crossplay { get; set; } = false;
	public bool Hdr { get; set; } = false;
	public bool xCloudTouch { get; set; } = false;
	public bool GamePass { get; set; } = false;
	public bool CloudGaming { get; set; } = false;
	public bool PcGamePass { get; set; } = false;
	public bool EaPlay { get; set; } = false;
	public bool GamePreview { get; set; } = false;
	public bool IdAtXbox { get; set; } = false;
	public bool OnSteam { get; set; } = false;
	public bool GamesWithGold { get; set; } = false;
	public bool TransferableProgress { get; set; } = false;


	public int? FeatureListOfGameId { get; set; }
	public Game? Game { get; set; }
}
