namespace TavisApi.Services;

public class UserService : IUserService
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserService(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public string GetCurrentUserName()
  {
    // Get the current user's identity
    var identity = _httpContextAccessor.HttpContext.User.Identity;

    if (identity.IsAuthenticated)
    {
      // Get the user's ID claim (you may need to adjust this based on your claim setup)
      return identity.Name;
    }

    return null; // User is not authenticated or claim not found
  }
}
