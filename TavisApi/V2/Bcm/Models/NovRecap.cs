using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TavisApi.V2.Bcm.Models;

public class NovRecap {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public int Rank { get; set; }
	public string Gamertag { get; set; } = string.Empty;
	public int Podium2019_1st { get; set; } = 0;
	public int Podium2019_2nd { get; set; } = 0;
	public int Podium2019_3rd { get; set; } = 0;
	public int Podium2020_1st { get; set; } = 0;
	public int Podium2020_2nd { get; set; } = 0;
	public int Podium2020_3rd { get; set; } = 0;
	public int Podium2021_1st { get; set; } = 0;
	public int Podium2021_2nd { get; set; } = 0;
	public int Podium2021_3rd { get; set; } = 0;
	public int Podium2022_1st { get; set; } = 0;
	public int Podium2022_2nd { get; set; } = 0;
	public int Podium2022_3rd { get; set; } = 0;
	public int Podium2023_1st { get; set; } = 0;
	public int Podium2023_2nd { get; set; } = 0;
	public int Podium2023_3rd { get; set; } = 0;
	public int CommunityBonus { get; set; } = 0;
	public bool Participation { get; set; } = false;
	public double TotalPoints { get; set; } = 0.0;

	public long PlayerId { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
}
