namespace Bcm.Models;

public class PlayersGame {
  public int GameId {get;set;}
  public int PlayerId {get; set;}
  public Platform? Platform {get;set;}
  public int? TrueAchievement {get;set;}
  public int? Gamerscore {get;set;}
  public int? AchievementCount {get;set;}
  public DateTime? StartedDate {get;set;}
  public DateTime? CompletionDate {get;set;}
  public DateTime? LastUnlock {get;set;}
  public Ownership? Ownership {get;set;} 
  public bool NotForContests {get; set;} = false;
}
