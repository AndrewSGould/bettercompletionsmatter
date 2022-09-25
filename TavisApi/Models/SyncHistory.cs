namespace Tavis.Models;

public class SyncHistory {
  public int Id {get; set;} = 0;
  public DateTime? Start {get; set;}
  public DateTime? End {get; set;}
  public int? PlayerCount {get; set;}
  public int? TaHits {get; set;}
  public SyncProfileList? Profile {get; set;}
}
