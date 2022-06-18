namespace Tavis.Models {
  public class Player {
    public int Id {get; set;} = 0;
    public int TrueAchievementId {get;set;} = 0;
    public string? Name {get; set;}
    public string? Region {get;set;}
    public string? Area {get;set;}
    public bool IsActive {get;set;} = false;


    public ICollection<PlayerGame> PlayerGames {get;set;} = new List<PlayerGame>();
  }
}