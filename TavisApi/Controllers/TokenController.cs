namespace TavisApi.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;
using Microsoft.AspNetCore.Authorization;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
  private readonly TavisContext _context;
  private readonly ITokenService _tokenService;
  public TokenController(TavisContext context, ITokenService tokenService)
  {
    this._context = context ?? throw new ArgumentNullException(nameof(context));
    this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
  }

  [HttpPost]
  [Route("refresh")]
  public IActionResult Refresh(TokenApi tokenApiModel)
  {
    if (tokenApiModel is null)
      return BadRequest("Invalid client request");

    string accessToken = tokenApiModel.AccessToken;
    string refreshToken = tokenApiModel.RefreshToken;
    var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
    var gamertag = principal.Identity.Name; //this is mapped to the Name claim by default
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

  [HttpPost, Authorize]
  [Route("revoke")]
  public IActionResult Revoke()
  {
    var gamertag = User.Identity.Name;
    var user = _context.Logins.SingleOrDefault(u => u.User.Gamertag == gamertag);

    if (user == null) return BadRequest();
    user.RefreshToken = null;

    _context.SaveChanges();

    return NoContent();
  }
}
