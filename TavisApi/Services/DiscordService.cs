using System.Security.Claims;
using System.Text;
using Discord;
using Discord.Rest;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
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

  public async Task<RestSelfUser> Connect(DiscordLogin discordAuth, User user)
  {
    if (user == null)
    {
      // Create an exception or use an existing one
      var exception = new Exception("No user when trying to connect to Discord");

      // Return a faulted task with the exception
      return await Task.FromException<RestSelfUser>(exception);
    }

    var envVars = DotEnv.Read();
    var token = envVars["DISCORD_BOT_TOKEN"];

    using var client = new DiscordRestClient();
    await client.LoginAsync(TokenType.Bearer, discordAuth.AccessToken);

    using var bot = new DiscordRestClient();
    await bot.LoginAsync(TokenType.Bot, token);

    var guilds = await client.GetGuildSummariesAsync().FlattenAsync();
    var guildSummary = guilds.FirstOrDefault(x => x.Name.Contains("BCMX"));
    var member = await guildSummary.GetCurrentUserGuildMemberAsync();

    var guild = await bot.GetGuildAsync(guildSummary.Id);

    // find any new roles and add them to the DB
    var newRoles = guild.Roles
                    .Where(role => !_context.UserRoles.Any(userRole => userRole.DiscordId == role.Id))
                    .Select(role => new { role.Id, role.Name });

    foreach (var role in newRoles)
    {
      _context.UserRoles.Add(new UserRole
      {
        DiscordId = role.Id,
        RoleName = role.Name
      });
    }

    await _context.SaveChangesAsync();

    // find any potentially updated roles and update them
    var updatedRoles = guild.Roles
        .Where(role => _context.UserRoles.Any(userRole => userRole.DiscordId == role.Id))
        .Select(role => new { role.Id, role.Name });

    var dbRoles = _context.UserRoles;

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
      // Check if the UserRole with DiscordId already exists
      var existingUserRole = _context.UserRoles.FirstOrDefault(ur => ur.DiscordId == roleId);

      if (existingUserRole == null)
      {
        // If it doesn't exist, create a new UserRole
        existingUserRole = new UserRole
        {
          DiscordId = roleId,
          RoleName = guild.Roles.First(x => x.Id == roleId).Name
        };

        // Add the new UserRole to the UserRoles collection
        _context.UserRoles.Add(existingUserRole);
      }

      // Check if the relationship already exists in UserUserRole table
      if (!user.UserRoles.Any(ur => ur.DiscordId == roleId))
      {
        // Create the relationship in the UserUserRole table
        user.UserRoles.Add(existingUserRole);
      }
    }

    await _context.SaveChangesAsync();

    return client.CurrentUser;
  }
}
