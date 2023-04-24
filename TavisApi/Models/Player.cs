namespace Tavis.Models {
  public class Player {
    public int Id {get; set;} = 0;
    public int TrueAchievementId {get;set;} = 0;
    public string? Name {get; set;}
    public string? Region {get;set;}
    public string? Area {get;set;}
    public bool IsActive {get;set;} = false;
    public DateTime? LastSync {get;set;}

    public ICollection<PlayerGame>? PlayerGames {get;set;}
    public ICollection<PlayerContest>? PlayerContests {get; set;}
    public ICollection<PlayerCompletionHistory>? PlayerCompletionHistories {get; set;}
    public BcmStat BcmStats {get; set;}
    public ICollection<BcmRgsc>? BcmRgsc {get; set;}
  }
}
