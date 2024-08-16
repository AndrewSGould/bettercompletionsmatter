using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TavisApi.Users.Models;

namespace TavisApi.Discord.Models;
public class DiscordLogin {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public ulong Id { get; set; }
	public ulong DiscordId { get; set; }
	public string? TokenType { get; set; }
	[EncryptColumn]
	public string? AccessToken { get; set; }
	[EncryptColumn]
	public string? RefreshToken { get; set; }
	public ulong UserId { get; set; }
	public User? User { get; set; }
}

public class DiscordConnect {
	public string TokenType { get; set; } = "";
	public string AccessToken { get; set; } = "";
}

public class DiscordConnectV2 {
	public string? Code { get; set; }
}
