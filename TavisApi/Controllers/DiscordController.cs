namespace TavisApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using System.Linq;
using TavisApi.Services;
using Discord.Rest;
using Discord;
using Discord.Net;
using Microsoft.EntityFrameworkCore;

[Route("[controller]")]
[ApiController]
public class DiscordController : ControllerBase
{
  private readonly TavisContext _context;
  private readonly IUserService _userService;
  private readonly IDiscordService _discordService;

  public DiscordController(TavisContext context, IUserService userService, IDiscordService discordService)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    _discordService = discordService;
  }

  [HttpGet, Route("getconnection")]
  public async Task<IActionResult> GetConnection()
  {
    // pull this out into discord service
    var currentUsername = _userService.GetCurrentUserName();
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == currentUsername);

    if (localuser is null) return BadRequest("Could not find user with supplied gamertag");

    var discordLogin = _context.DiscordLogins.FirstOrDefault(x => x.UserId == localuser.Id);

    if (discordLogin is null) return NoContent();

    using var client = new DiscordRestClient();

    await client.LoginAsync(TokenType.Bearer, discordLogin.AccessToken);

    var user = await client.GetCurrentUserAsync();

    return Ok(user);
  }

  [HttpGet, Route("refreshDiscordRoles")]
  public async Task<IActionResult> RefreshDiscordRoles()
  {

    // pull this out into discord service
    var currentUsername = _userService.GetCurrentUserName();
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == currentUsername);

    if (localuser is null) return BadRequest("Could not find user with supplied gamertag");

    var discordLogin = _context.DiscordLogins.FirstOrDefault(x => x.UserId == localuser.Id);

    if (discordLogin is null) return NoContent();

    var dConnect = new Tavis.Models.DiscordConnect
    {
      TokenType = "Bearer",
      AccessToken = discordLogin.AccessToken
    };

    var discordProfile = await _discordService.Connect(dConnect, localuser);

    var user = await _context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.Id == localuser.Id);

    return Ok(user?.UserRoles.Select(x => x.Role.RoleName).OrderBy(x => x).ToList());
  }
}
