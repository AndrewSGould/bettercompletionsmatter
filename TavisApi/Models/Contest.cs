namespace Tavis.Models;
public class Contest
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  //ParseProfile?
  public ICollection<PlayerContest>? PlayerContests { get; set; }
}
