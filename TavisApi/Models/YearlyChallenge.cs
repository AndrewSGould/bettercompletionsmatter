using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TavisApi.Data;

namespace Tavis.Models;

public class YearlyChallenge
{
  public long Id { get; set; }
  public YearlyCategory Category { get; set; }
  public string Title { get; set; } = "No Title!";
  public string Description { get; set; } = "No Description!";
}
