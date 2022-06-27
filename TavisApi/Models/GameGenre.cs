namespace Tavis.Models;
public class GameGenre {
  public int GameId {get;set;} = 0;
  public Game? Game {get;set;}

  public GenreList GenreId {get;set;} = GenreList.None;
  public Genre? Genre {get;set;}
}
