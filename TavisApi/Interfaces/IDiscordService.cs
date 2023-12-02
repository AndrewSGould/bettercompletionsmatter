using Discord.Rest;
using Tavis.Models;

namespace TavisApi.Services;

public interface IDiscordService
{
  Task<RestSelfUser> Connect(DiscordConnect discordAuth, User user);
  Task AddBcmParticipantRole(User user);
}
