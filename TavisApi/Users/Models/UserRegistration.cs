namespace TavisApi.Users.Models;

public class UserRegistration {
	public ulong UserId { get; set; }
	public User User { get; set; } = new();

	public DateTime? RegistrationDate { get; set; }
}
