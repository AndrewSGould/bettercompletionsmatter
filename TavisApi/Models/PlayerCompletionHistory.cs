namespace Tavis.Models;

public class BcmPlayerCompletionHistory
{
  public int Id { get; set; } = 0;
  public DateTime CompletionDate { get; set; } = DateTime.MinValue;

  public long PlayerId { get; set; }
  public BcmPlayer? BcmPlayer { get; set; }

  public int? GameId { get; set; }
  public Game? Game { get; set; }
}
