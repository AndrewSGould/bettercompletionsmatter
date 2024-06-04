using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tavis.Models;
using TavisApi.V2.Authentication;

namespace TavisApi.V2.Users;

public class User {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public string? Xuid { get; set; }
	public string? Gamertag { get; set; }
	public string? Avatar { get; set; }
	public string? Region { get; set; }
	public string? Area { get; set; }
	public Login? Login { get; set; }
	public DiscordLogin? DiscordLogin { get; set; }
	public BcmPlayer? BcmPlayer { get; set; }
	public List<UserRole> UserRoles { get; } = new();
	public List<UserRegistration> UserRegistrations { get; set; } = new();
}
