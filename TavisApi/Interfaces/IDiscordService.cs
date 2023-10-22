using Tavis.Models;

namespace TavisApi.Services;

public interface IDiscordService
{
  public Task<DiscordSigninResponse> Connect(DiscordLogin discordAuth);
}
