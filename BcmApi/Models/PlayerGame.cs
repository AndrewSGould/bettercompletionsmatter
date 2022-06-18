namespace Bcm.Models;

public class PlayerGame {
  public Platform? Platform {get;set;}
  public int? TrueAchievement {get;set;}
  public int? Gamerscore {get;set;}
  public int? AchievementCount {get;set;}
  public DateTime? StartedDate {get;set;}
  public DateTime? CompletionDate {get;set;}
  public DateTime? LastUnlock {get;set;}
  public Ownership? Ownership {get;set;} 
  public bool NotForContests {get; set;} = false;


  public int PlayerId {get;set;} = 0;
  public Player Player {get;set;} = new Player();

  public int GameId {get;set;} = 0;
  public Game Game {get;set;} = new Game();
}
