using System.Linq.Expressions;
using Discord;
using Discord.Rest;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
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
    string? token;
    DiscordRestClient? bot;
    DiscordRestClient? client;
    RestGuild? guild;
    RestGuildUser? member;

    // return await Task.FromException<RestSelfUser>(new Exception("Auth Token: " + discordAuth.AccessToken + " -- Token Type: " + discordAuth.TokenType));

    if (user == null)
    {
      // Create an exception or use an existing one
      var exception = new Exception("No user when trying to connect to Discord");

      // Return a faulted task with the exception
      return await Task.FromException<RestSelfUser>(exception);
    }

    try
    {
      var envVars = DotEnv.Read();
      token = envVars.TryGetValue("DISCORD_BOT_TOKEN", out var key) ? key : null;
      if (token is null || token == "") token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN")!;
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to get the Discord Bot Token" + ex.Message));
    }

    try
    {
      bot = new DiscordRestClient();
      await bot.LoginAsync(TokenType.Bot, token);
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to login the Discord Bot" + ex.Message));
    }

    try
    {
      client = new DiscordRestClient();
      await client.LoginAsync(TokenType.Bearer, discordAuth.AccessToken);
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to login the Discord User" + ex.Message));
    }

    try
    {
      var guilds = await client.GetGuildSummariesAsync().FlattenAsync();
      var guildSummary = guilds.FirstOrDefault(x => x.Name.Contains("BCMX"));

      // TODO: if they don't have BCMX server aka guildSummary is null, join it for them

      try
      {
        member = await guildSummary!.GetCurrentUserGuildMemberAsync();
      }
      catch (Exception ex)
      {
        var guildNames = guilds.Select(x => x.Name).ToList();
        return await Task.FromException<RestSelfUser>(new Exception(string.Join(", ", guildNames) + " -- " + guildSummary?.Name));
      }

      guild = await bot.GetGuildAsync(guildSummary.Id);
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to get the BCMX guild" + ex.Message));
    }

    try
    {
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
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to collect Discord roles" + ex.Message));
    }

    try
    {
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
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to update Tavis' list of Discord roles" + ex.Message));
    }

    var userWithRoles = _context.Users.Include(u => u.UserRoles).FirstOrDefault(x => x == user);

    try
    {
      foreach (var roleId in member.RoleIds)
      {
        var dbRole = _context.Roles.First(x => x.DiscordId == roleId);

        if (userWithRoles?.UserRoles.Find(x => x.RoleId == dbRole.Id) == null)
        {
          userWithRoles?.UserRoles.Add(new UserRole
          {
            Role = dbRole
          });
        }
      }

      await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Unable to update Tavis' list of Discord roles" + ex.Message));
    }

    try
    {
      return client.CurrentUser;
    }
    catch (Exception ex)
    {
      return await Task.FromException<RestSelfUser>(new Exception("Failure getting Discord Current User" + ex.Message));
    }
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
