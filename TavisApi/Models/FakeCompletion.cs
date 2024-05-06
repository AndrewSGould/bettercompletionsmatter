namespace Tavis.Models;

public class FakeCompletion
{
  public int Id { get; set; } = 0;
  public long PlayerId { get; set; }
  public int? GameId { get; set; }
  public string? Title { get; set; }
  public double? SiteRatio { get; set; }
  public double? FullCompletionEstimate { get; set; }
  public DateTime? FakeCompletionDate { get; set; }
  public double BonusPoints { get; set; }
}
