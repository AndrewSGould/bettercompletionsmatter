namespace TavisApi.User.Interfaces;

public interface IUserServiceV2 {
	public string? GetCurrentUserName();
	public User GetCurrentUser();
}
