using TavisApi.TrueAchievements.Models;

namespace TavisApi.Models;

public class GameGenre
{
    public int GameId { get; set; } = 0;
    public Game? Game { get; set; }
    public DateTime? LastSync { get; set; }

    public GenreList GenreId { get; set; } = GenreList.None;
    public Genre? Genre { get; set; }
}
