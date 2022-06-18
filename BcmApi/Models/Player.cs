namespace Bcm.Models {
  public class Player {
    public int Id {get; set;}
    public int TrueAchievementId {get;set;}
    public string? Name {get; set;}
    public string? Region {get;set;}
    public string? Area {get;set;}
    public bool IsActive {get;set;}


    public ICollection<PlayerGame> PlayerGames {get;set;}
  }
}