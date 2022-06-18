using Tavis.Models;

namespace Tavis.Models {
  public class TA_CollectionEntry {
    public int Id {get; set;}
    public int GameId {get;set;}
    public string? GameUrl {get;set;}
    public string? Title {get; set;}
    public Platform? Platform {get;set;}
    public int? PlayerTrueAchievement {get;set;}
    public int? TotalTrueAchievement {get;set;}
    public int? PlayerGamerscore {get;set;}
    public int? TotalGamerscore {get;set;}
    public int? PlayerAchievementCount {get;set;}
    public int? TotalAchievementCount {get;set;}
    public DateTime? StartedDate {get;set;}
    public DateTime? CompletionDate {get;set;}
    public DateTime? LastUnlock {get;set;}
    public Ownership? Ownership {get;set;} 
    public bool NotForContests {get; set;} = false;
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
}
