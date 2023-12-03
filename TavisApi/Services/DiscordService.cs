using Discord;
using Discord.Rest;
using dotenv.net;
using Tavis.Models;
using TavisApi.Context;

namespace TavisApi.Services;

public class DiscordService : IDiscordService
{
  private TavisContext _context;

  public DiscordService(TavisContext context)
  {
    _context = context;
  }

  public async Task<RestSelfUser> Connect(DiscordConnect discordAuth, User user)
  {
    if (user == null)
    {
      // Create an exception or use an existing one
      var exception = new Exception("No user when trying to connect to Discord");

      // Return a faulted task with the exception
      return await Task.FromException<RestSelfUser>(exception);
    }

    var envVars = DotEnv.Read();
    var token = envVars.TryGetValue("DISCORD_BOT_TOKEN", out var key) ? key : null;
    if (token is null || token == "") token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN")!;

    using var bot = new DiscordRestClient();
    await bot.LoginAsync(TokenType.Bot, token);

    using var client = new DiscordRestClient();
    await client.LoginAsync(TokenType.Bearer, discordAuth.AccessToken);

    var guilds = await client.GetGuildSummariesAsync().FlattenAsync();
    var guildSummary = guilds.FirstOrDefault(x => x.Name.Contains("BCMX"));

    // TODO: if they don't have BCMX server aka guildSummary is null, join it for them

    var member = await guildSummary!.GetCurrentUserGuildMemberAsync();

    var guild = await bot.GetGuildAsync(guildSummary.Id);

    // find any new roles and add them to the DB
    var newRoles = guild.Roles
                    .Where(role => !_context.Roles.Any(userRole => userRole.DiscordId == role.Id))
                    .Select(role => new { role.Id, role.Name });

    foreach (var role in newRoles)
    {
      _context.Roles.Add(new Role
      {
        DiscordId = role.Id,
        RoleName = role.Name
      });
    }

    await _context.SaveChangesAsync();

    // find any potentially updated roles and update them
    var updatedRoles = guild.Roles
        .Where(role => _context.Roles.Any(userRole => userRole.DiscordId == role.Id))
        .Select(role => new { role.Id, role.Name });

    var dbRoles = _context.Roles;

    foreach (var updatedRole in updatedRoles)
    {
      var dbRole = dbRoles.FirstOrDefault(role => role.DiscordId == updatedRole.Id);

      if (dbRole != null && dbRole.RoleName != updatedRole.Name)
        dbRole.RoleName = updatedRole.Name;
    }

    // delete the users roles and rehydrate
    user.UserRoles.Clear();
    foreach (var roleId in member.RoleIds)
    {
      var dbRole = _context.Roles.First(x => x.DiscordId == roleId);

      user.UserRoles.Add(new UserRole
      {
        Role = dbRole
      });
    }

    await _context.SaveChangesAsync();

    return client.CurrentUser;
  }

  public async Task AddBcmParticipantRole(User user)
  {
    if (user == null)
      throw new Exception("No user when trying to connect to Discord");

    var dProfile = _context.DiscordLogins.FirstOrDefault(x => x.UserId == user.Id);

    if (dProfile is null)
      throw new Exception("No discord profile with supplied user");

    await AddDiscordRole(dProfile.AccessToken, "Participant");
  }

  private async Task AddDiscordRole(string accessToken, string roleName)
  {
    var envVars = DotEnv.Read();
    var token = envVars.TryGetValue("DISCORD_BOT_TOKEN", out var key) ? key : null;
    if (token is null || token == "") token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN")!;

    using var bot = new DiscordRestClient();
    await bot.LoginAsync(TokenType.Bot, token);

    using var client = new DiscordRestClient();
    await client.LoginAsync(TokenType.Bearer, accessToken);

    Role partipRole = _context.Roles.First(x => x.RoleName == roleName);

    var guilds = await bot.GetGuildSummariesAsync().FlattenAsync();
    var guildSummary = guilds.FirstOrDefault(x => x.Name.Contains("BCMX"));

    if (guildSummary is null || partipRole is null)
      throw new Exception("Full context unavailable when trying to update Discord Role");

    await bot.AddRoleAsync(guildSummary.Id, client.CurrentUser.Id, partipRole.DiscordId);
  }
}
