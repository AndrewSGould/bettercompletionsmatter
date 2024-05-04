namespace Tavis.Models;

public class PlayerTopGenre
{
  public long PlayerId { get; set; } = 0;
  public int Rank { get; set; }
  public GenreList GenreId { get; set; } = GenreList.None;
}
