using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TavisApi.V2.Bcm.Models;

public class BcmMiscStat {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[Column(TypeName = "jsonb")]
	public string? HistoricalStats { get; set; }

	public long PlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
}
