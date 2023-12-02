using Tavis.Models;

namespace TavisApi.Services;

public interface IUserService
{
  public string? GetCurrentUserName();
  public User? GetCurrentUser();
}
