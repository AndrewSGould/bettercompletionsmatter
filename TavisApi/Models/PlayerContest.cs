namespace Tavis.Models;
public class PlayerContest {
  public int? PlayerId {get;set;}
  public Player? Player {get;set;}

  public int? ContestId {get;set;}
  public Contest? Contest {get;set;}
}
