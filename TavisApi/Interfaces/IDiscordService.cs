using Discord.Rest;
using TavisApi.V2.Discord.Models;
using TavisApi.V2.Users;

namespace TavisApi.Services;

public interface IDiscordService {
	Task<RestSelfUser> Connect(DiscordConnect discordAuth, User user);
	Task AddBcmParticipantRole(User user);
}
