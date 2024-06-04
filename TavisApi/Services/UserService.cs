using Microsoft.EntityFrameworkCore;
using Tavis.Models;
using TavisApi.Context;

namespace TavisApi.Services;

public class UserService : IUserService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly TavisContext _context;

  public UserService(IHttpContextAccessor httpContextAccessor, TavisContext context)
  {
    _httpContextAccessor = httpContextAccessor;
    _context = context;
  }

  public string? GetCurrentUserName()
  {
    var identity = _httpContextAccessor.HttpContext?.User.Identity;
    if (identity is null) return null;

    if (identity.IsAuthenticated)
      return identity.Name;

    // User is not authenticated or claim not found
    return null;
  }

  public User? GetCurrentUser()
  {
    return _context.Users.FirstOrDefault(x => x.Gamertag == GetCurrentUserName());
  }
}
