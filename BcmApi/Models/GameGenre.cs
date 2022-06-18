namespace Bcm.Models;
public class GameGenre {
  public int GameId {get;set;}
  public Game Game {get;set;}

  public GenreList GenreId {get;set;}
  public Genre Genre {get;set;}
}
