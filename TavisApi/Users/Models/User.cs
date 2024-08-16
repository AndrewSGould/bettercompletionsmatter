using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TavisApi.Authentication.Models;
using TavisApi.Discord.Models;
using TavisApi.Models;

namespace TavisApi.Users.Models;

public class User {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public ulong Id { get; set; }
	public string? Xuid { get; set; }
	public string? Gamertag { get; set; }
	public string? Avatar { get; set; }
	public string? Region { get; set; }
	public string? Area { get; set; }
	public Login? Login { get; set; }
	public DiscordLogin? DiscordLogin { get; set; }
	public Player? Player { get; set; }
	public List<UserRole> UserRoles { get; } = new();
	public List<UserRegistration> UserRegistrations { get; set; } = new();
}
