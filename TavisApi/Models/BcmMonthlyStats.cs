using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TavisApi.V2.Bcm.Models;

namespace Tavis.Models;

public class BcmMonthlyStat {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public int Challenge { get; set; }
	public int BonusPoints { get; set; }
	public bool Participation { get; set; }
	public bool? AllBuckets { get; set; }
	public bool? CommunityBonus { get; set; }

	public long BcmPlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
}
