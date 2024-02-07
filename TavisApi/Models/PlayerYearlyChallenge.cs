using DocumentFormat.OpenXml.Drawing.Charts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tavis.Models;

public class PlayerYearlyChallenge
{
  [Key, Column(Order = 1)]
  public long YearlyChallengeId { get; set; } = 0;
  public YearlyChallenge? YearlyChallenge { get; set; }

  [Key, Column(Order = 2)]
  public long PlayerId { get; set; } = 0;
  public string? WriteIn { get; set; }
  public string? Reasoning { get; set; }
  public bool Approved { get; set; } = false;

  public int? GameId { get; set; }
  public Game? Game { get; set; }
}
