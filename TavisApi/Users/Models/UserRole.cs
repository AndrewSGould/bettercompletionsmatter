namespace TavisApi.Users.Models;

public class UserRole {
	public ulong UserId { get; set; }
	public User User { get; set; } = new();

	public ulong RoleId { get; set; }
	public Role Role { get; set; } = new();
}
