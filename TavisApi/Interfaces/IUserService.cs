using TavisApi.V2.Users;

namespace TavisApi.Services;

public interface IUserService {
	public string? GetCurrentUserName();
	public User? GetCurrentUser();
}
