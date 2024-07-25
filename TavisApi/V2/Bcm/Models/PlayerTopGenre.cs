using TavisApi.V2.TrueAchievements.Models;

namespace TavisApi.V2.Bcm.Models;

public class PlayerTopGenre {
	public long PlayerId { get; set; } = 0;
	public int Rank { get; set; }
	public GenreList GenreId { get; set; } = GenreList.None;
}
