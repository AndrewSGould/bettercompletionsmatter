namespace Bcm.Models;

public class Game {
  public int Id {get; set;}
  public int TrueAchievementId {get;set;}
  public string? Title {get; set;}
  public int? TrueAchievement {get;set;}
  public int? Gamerscore {get;set;}
  public int? AchievementCount {get;set;}
  public string? Publisher {get;set;}
  public string? Developer {get;set;}
  public DateTime? ReleaseDate {get;set;}
  public int? GamersWithGame {get;set;}
  public int? GamersCompleted {get;set;}
  public double? BaseCompletionEstimate {get;set;}
  public double? SiteRatio {get;set;}
  public double? SiteRating {get;set;}
  public bool Unobtainables {get;set;} = false;
  public DateTime? ServerClosure {get;set;}
  public double? InstallSize {get;set;}
  public double? FullCompletionEstimate {get;set;}
}
