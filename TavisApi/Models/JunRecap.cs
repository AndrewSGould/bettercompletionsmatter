using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tavis.Models;

namespace TavisApi.Models {
	public class JunRecap {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		public int Rank { get; set; }
		public string Gamertag { get; set; } = string.Empty;
		public int QualifiedGames { get; set; } = 0;
		public bool CommunityBonus { get; set; } = false;
		public bool Participation { get; set; } = false;
		public double TotalPoints { get; set; } = 0.0;

		public long PlayerId { get; set; }
		public BcmPlayer? BcmPlayer { get; set; }
	}
}
