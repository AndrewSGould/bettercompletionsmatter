namespace TavisApi.V2.Users;

public interface IUserServiceV2 {
	public string? GetCurrentUserName();
	public User GetCurrentUser();
}
