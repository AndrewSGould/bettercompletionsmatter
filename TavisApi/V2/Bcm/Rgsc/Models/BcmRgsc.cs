using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.V2.Bcm.Rgsc.Models;

public class BcmRgsc {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public DateTime? Issued { get; set; }
	public int PoolSize { get; set; }
	public bool Rerolled { get; set; }
	public DateTime? RerollDate { get; set; }
	public int? GameId { get; set; }
	public int? PreviousGameId { get; set; }
	public int? Challenge { get; set; }

	public long BcmPlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
}
