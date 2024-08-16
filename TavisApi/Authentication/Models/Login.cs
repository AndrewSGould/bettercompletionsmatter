using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TavisApi.Users.Models;

namespace TavisApi.Authentication.Models;

public class Login {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public ulong Id { get; set; }
	[EncryptColumn]
	public string? Password { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
	public long UserId { get; set; }
	public User User { get; set; } = new();
}
