using TavisApi.Users.Models;

namespace TavisApi.Users.Interfaces;

public interface IUserServiceV2 {
	public string? GetCurrentUserName();
	public User GetCurrentUser();
}
