using Discord.Rest;
using Tavis.Models;

namespace TavisApi.Services;

public interface IDiscordService
{
  public Task<RestSelfUser> Connect(DiscordLogin discordAuth, User user);
}
