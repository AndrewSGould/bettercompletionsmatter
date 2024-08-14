using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Authentication.Interfaces;
using TavisApi.Authentication.Models;

namespace TavisApi.Authentication;

[Route("/v2/token/")]
[ApiController]
public class TokenControllerV2 : ControllerBase
{
    private readonly TavisContext _context;
    private readonly ITokenServiceV2 _tokenService;

    public TokenControllerV2(TavisContext context, ITokenServiceV2 tokenService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenApi tokenApiModel)
    {
        if (tokenApiModel is null || tokenApiModel.RefreshToken is null || tokenApiModel.AccessToken is null)
            return BadRequest("Invalid client request");

        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var gamertag = principal.Identity?.Name; //this is mapped to the Name claim by default
        var user = _context.Logins.SingleOrDefault(u => u.User.Gamertag == gamertag);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;

        _context.SaveChanges();

        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize(Roles = "Admin")]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var gamertag = User.Identity?.Name;
        var user = _context.Logins.SingleOrDefault(u => u.User.Gamertag == gamertag);

        if (user == null) return BadRequest();
        user.RefreshToken = null;

        _context.SaveChanges();

        return NoContent();
    }
}
