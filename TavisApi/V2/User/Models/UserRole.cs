namespace TavisApi.V2.Users;

public class UserRole {
	public long UserId { get; set; }
	public User User { get; set; } = new();

	public long RoleId { get; set; }
	public Role Role { get; set; } = new();
}
