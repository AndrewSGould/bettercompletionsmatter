using TavisApi.V2.TrueAchievements.Models;

namespace TavisApi.V2.Models;

public class Genre {
	public GenreList? Id { get; set; }
	public string? Name { get; set; }


	public IList<GameGenre>? GameGenres { get; set; }
}
