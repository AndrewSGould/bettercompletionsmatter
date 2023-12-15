using Tavis.Models;

namespace TavisApi.Services;

public interface IRgscService
{
  int GetUserRerollCount(long userId);
}
