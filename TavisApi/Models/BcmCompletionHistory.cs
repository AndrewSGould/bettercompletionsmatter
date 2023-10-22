namespace Tavis.Models;

public class BcmCompletionHistory
{
  public int Id { get; set; } = 0;
  public int? GameId { get; set; }
  public double? SiteRatio { get; set; }

  public Game? Game { get; set; }
}
