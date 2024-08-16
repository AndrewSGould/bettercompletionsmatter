using TavisApi.TrueAchievements.Models;

namespace TavisApi.Models;

public class Genre {
	public GenreList? Id { get; set; }
	public string? Name { get; set; }


	public IList<GameGenre>? GameGenres { get; set; }
}
