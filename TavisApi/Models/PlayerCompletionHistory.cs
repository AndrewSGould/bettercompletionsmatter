namespace Tavis.Models;

public class PlayerCompletionHistory
{
  public int Id { get; set; } = 0;
  public DateTime CompletionDate { get; set; } = DateTime.MinValue;

  public int? PlayerId { get; set; }
  public Player? Player { get; set; }

  public int? GameId { get; set; }
  public Game? Game { get; set; }
}
