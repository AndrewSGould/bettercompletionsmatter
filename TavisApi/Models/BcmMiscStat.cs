using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tavis.Models;

namespace TavisApi.Models;

public class BcmMiscStat
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Column(TypeName = "jsonb")]
  public string? HistoricalStats { get ; set; }

  public long PlayerId { get; set; }
  public BcmPlayer? BcmPlayer { get; set; }
}
