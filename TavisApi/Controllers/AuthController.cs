namespace WebApi.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
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

    var user = _context.Users.FirstOrDefault(u => u.Gamertag == oxblProfile.Gamertag);

    if (user is null)
    {
      _context.Users.Add(new User
      {
        Xuid = oxblProfile.Xuid,
        Gamertag = oxblProfile.Gamertag
      });

      await _context.SaveChangesAsync();

      _context.Logins.Add(new Login
      {
        UserId = _context.Users.FirstOrDefault(x => x.Xuid == oxblProfile.Xuid).Id,
        Password = oxblProfile.App_Key
      });

      await _context.SaveChangesAsync();
      // ensure the password / app_key is encrypted
    }
    else user.Login = _context.Logins.FirstOrDefault(x => x.UserId == user.Id);

    var newLogin = _context.Logins.FirstOrDefault(u => u.Password == oxblProfile.App_Key);

    if (newLogin is null)
    {
      var login = _context.Logins.FirstOrDefault(x => x.UserId == user.Id);
      login.Password = oxblProfile.App_Key;
      newLogin = login;
    }

    // we'll create another auth'd only method that connects your discord
    // the roles discord returns will create a new accesstoken - model it after refresh token
    var claims = new List<Claim>
      {
          new Claim(ClaimTypes.Name, oxblProfile.Gamertag),
          // TODO: here, we should only issue basic access. later when we connect to discord we'll
          // scan the roles and issue a new JWT
          new Claim(ClaimTypes.Role, "Super Admin") // TODO: roles need to be enums
      };

    var accessToken = _tokenService.GenerateAccessToken(claims);

    // TODO: what do we do with the user avatar url? caching? localstorage? download ourselves and rehost?

    var refreshToken = _tokenService.GenerateRefreshToken();
    newLogin.RefreshToken = refreshToken;
    newLogin.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    await _context.SaveChangesAsync();

    return Ok(new AuthenticatedResponse
    {
      Token = accessToken,
      RefreshToken = refreshToken,
      Gamertag = oxblProfile.Gamertag,
      Avatar = oxblProfile.Avatar
    });
  }

  [HttpPost, Route("integrate-discord")]
  [Authorize(Roles = "Super Admin")]
  public async Task<IActionResult> IntegrateDiscord([FromBody] DiscordLogin dLogin)
  {
    if (dLogin is null || dLogin.AccessToken is null || dLogin.TokenType is null) return BadRequest("Invalid client request");

    var discordProfile = await _discordService.Connect(dLogin);
    var currentUsername = _userService.GetCurrentUserName();
    var user = _context.Users.FirstOrDefault(x => x.Gamertag == currentUsername);

    var newLogin = _context.DiscordLogins.FirstOrDefault(x => x.DiscordId == discordProfile.Id);

    if (newLogin is null)
    {
      _context.DiscordLogins.Add(new DiscordLogin
      {
        DiscordId = discordProfile.Id,
        UserId = user.Id,
        TokenType = dLogin.TokenType,
        AccessToken = dLogin.AccessToken,
      });
    }
    else
    {
      newLogin.AccessToken = dLogin.AccessToken;
      newLogin.TokenType = dLogin.TokenType;
    }

    await _context.SaveChangesAsync();

    return Ok();
  }
}
