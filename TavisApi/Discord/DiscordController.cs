using Discord;
using Discord.Rest;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Authentication.Interfaces;
using TavisApi.Discord.Interfaces;
using TavisApi.Discord.Models;
using TavisApi.User.Interfaces;
using TavisApi.V2.Authentication;

namespace TavisApi.Discord;

[Route("/v2/discord/")]
[ApiController]
public class DiscordControllerV2 : ControllerBase
{
    private readonly ITokenServiceV2 _tokenService;
    private readonly IDiscordServiceV2 _discordService;
    private readonly IUserServiceV2 _userService;
    private readonly TavisContext _context;

    public DiscordControllerV2(ITokenServiceV2 tokenService, IDiscordServiceV2 discordService, IUserServiceV2 userService, TavisContext context)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _discordService = discordService ?? throw new ArgumentNullException(nameof(discordService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpPost("revoke")]
    public IActionResult RevokeDiscordToken()
    {
        // TODO: add ability to revoke Discord tokens
        throw new NotImplementedException();
    }

    [HttpPost("login")]
    public async Task<IActionResult> IntegrateDiscord([FromBody] DiscordCode discordLogin)
    {
        if (discordLogin.Code is null) return BadRequest("Invalid client request");

        try
        {
            var user = _userService.GetCurrentUser();
            var tokens = await _discordService.Handshake(discordLogin.Code, user);

            var discordProfile = await _discordService.Connect(tokens.access, user);
            _discordService.UpdateDiscordAuth(discordProfile.Id, user, tokens);

            var token = await _tokenService.RegenerateTokenWithRoles(user);

            return Ok(new AuthenticatedResponse
            {
                Token = token.access,
                RefreshToken = token.refresh,
                Roles = token.roles
            });
        }
        catch (Exception ex)
        {
            return BadRequest("Existence is pain: " + ex.Message);
        }
    }

    [HttpPost("resyncroles")]
    public async Task<IActionResult> ResyncDiscordRoles()
    {
        try
        {
            var user = _userService.GetCurrentUser();
            var dLogin = _context.DiscordLogins.FirstOrDefault(x => x.UserId == user.Id);

            if (dLogin is null || dLogin.AccessToken is null) return Unauthorized();

            var discordProfile = await _discordService.Connect(dLogin.AccessToken, user);
            var token = await _tokenService.RegenerateTokenWithRoles(user);

            return Ok(new AuthenticatedResponse
            {
                Token = token.access,
                RefreshToken = token.refresh,
                Roles = token.roles
            });
        }
        catch (Exception ex)
        {
            return BadRequest("Existence is pain: " + ex.Message);
        }
    }

    [HttpGet("ping")]
    public async Task<IActionResult> PingDiscord()
    {
        var user = _userService.GetCurrentUser();
        var dLogin = _context.DiscordLogins.FirstOrDefault(x => x.UserId == user.Id);

        if (dLogin == null) return BadRequest();

        using var client = new DiscordRestClient();
        await client.LoginAsync(TokenType.Bearer, dLogin.AccessToken);

        if (client.LoginState == LoginState.LoggedIn)
        {
            return Ok();
        }

        var refresh_token = dLogin.RefreshToken;
        var refreshedToken = await _discordService.RefreshAccessTokenAsync(refresh_token);

        dLogin.AccessToken = refreshedToken;
        await _context.SaveChangesAsync();

        // if successful, return Ok
        // if unsuccessful, return Unauth'd

        return Ok();
    }
}
