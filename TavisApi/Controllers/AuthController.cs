namespace WebApi.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly TavisContext _context;
  private readonly ITokenService _tokenService;
  private readonly IOpenXblService _oxblService;
  private readonly IDiscordService _discordService;
  private readonly IUserService _userService;

  public AuthController(TavisContext context, ITokenService tokenService, IOpenXblService oxblService,
  IDiscordService discordService, IUserService userService)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    _oxblService = oxblService ?? throw new ArgumentNullException(nameof(oxblService));
    _discordService = discordService ?? throw new ArgumentNullException(nameof(discordService));
    _userService = userService ?? throw new ArgumentNullException(nameof(userService));
  }

  [HttpPost, Route("login")]
  public async Task<IActionResult> Login([FromBody] XblLogin oxblLogin)
  {
    if (oxblLogin is null || oxblLogin.OpenXblCode is null)
      return BadRequest("Invalid client request");

    var oxblAuth = new ConnectAuth
    {
      Code = oxblLogin.OpenXblCode,
      App_Key = "eb9ab783-e41f-d428-0319-fb2d4d4a0d71" // TODO: store this elsewhere
    };

    var oxblProfile = await _oxblService.Connect(oxblAuth);
    if (oxblProfile.App_Key is null) return BadRequest("Invalid client request");

    var user = _context.Users.Include(u => u.UserRoles).Include(l => l.Login).FirstOrDefault(x => x.Gamertag == oxblProfile.Gamertag);

    if (user is null)
    {
      _context.Users.Add(new User
      {
        Xuid = oxblProfile.Xuid,
        Gamertag = oxblProfile.Gamertag
      });

      await _context.SaveChangesAsync();

      user = _context.Users.FirstOrDefault(x => x.Xuid == oxblProfile.Xuid);

      user!.Login = new Login
      {
        UserId = user.Id,
        Password = oxblProfile.App_Key
      };

      await _context.SaveChangesAsync();
    }

    List<Claim> claims = new()
      {
        new Claim(ClaimTypes.Name, oxblProfile.Gamertag),
      };

    var userRolesWithDetails = user.UserRoles.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { UserRoles = ur, Roles = r });

    if (user!.UserRoles.Count() == 0)
      claims.Add(new Claim(ClaimTypes.Role, "NoRole"));
    else
      foreach (var role in userRolesWithDetails)
      {
        claims.Add(new Claim(ClaimTypes.Role, role.Roles.RoleName));
      }

    var accessToken = _tokenService.GenerateAccessToken(claims);

    var refreshToken = _tokenService.GenerateRefreshToken();
    user.Login!.RefreshToken = refreshToken;
    user.Login.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    await _context.SaveChangesAsync();

    return Ok(new AuthenticatedResponse
    {
      Token = accessToken,
      RefreshToken = refreshToken,
      Gamertag = oxblProfile.Gamertag,
      Avatar = oxblProfile.Avatar,
      Roles = userRolesWithDetails.Select(x => x.Roles.RoleName).ToList()
    });
  }

  [HttpPost, Route("integrate-discord")]
  public async Task<IActionResult> IntegrateDiscord([FromBody] DiscordConnect dConnect)
  {
    if (dConnect is null || dConnect.AccessToken is null || dConnect.TokenType is null) return BadRequest("Invalid client request");

    var currentUsername = _userService.GetCurrentUserName();
    var user = _context.Users.Include(u => u.UserRoles).FirstOrDefault(x => x.Gamertag == currentUsername);

    if (user is null) return BadRequest("No gamertag matches current user");

    var discordProfile = await _discordService.Connect(dConnect, user);

    var newLogin = _context.DiscordLogins.FirstOrDefault(x => x.DiscordId == discordProfile.Id);

    if (newLogin is null)
    {
      _context.DiscordLogins.Add(new DiscordLogin
      {
        DiscordId = discordProfile.Id,
        UserId = user.Id,
        TokenType = dConnect.TokenType,
        AccessToken = dConnect.AccessToken,
      });
    }
    else
    {
      newLogin.AccessToken = dConnect.AccessToken;
      newLogin.TokenType = dConnect.TokenType;
    }

    await _context.SaveChangesAsync();

    var claims = new List<Claim>
      {
          new Claim(ClaimTypes.Name, user.Gamertag!),
      };

    var userRolesWithDetails = user.UserRoles.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { UserRoles = ur, Roles = r });

    foreach (var role in userRolesWithDetails)
    {
      claims.Add(new Claim(ClaimTypes.Role, role.Roles.RoleName));
    }

    var login = _context.Logins.FirstOrDefault(x => x.UserId == user.Id);

    if (login is null) return BadRequest("No login found for provided user");

    var accessToken = _tokenService.GenerateAccessToken(claims);
    var refreshToken = _tokenService.GenerateRefreshToken();
    login.RefreshToken = refreshToken;
    login.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    await _context.SaveChangesAsync();

    return Ok(new AuthenticatedResponse
    {
      Token = accessToken,
      RefreshToken = refreshToken,
      Roles = userRolesWithDetails.Select(x => x.Roles.RoleName).ToList()
    });
  }
}
