namespace WebApi.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly TavisContext _context;
  private readonly ITokenService _tokenService;

  public AuthController(TavisContext context, ITokenService tokenService)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
  }

  [HttpPost, Route("login")]
  public IActionResult Login([FromBody] Login loginModel)
  {
    if (loginModel is null)
      return BadRequest("Invalid client request");

    var user = _context.Logins.FirstOrDefault(u =>
        (u.Username == loginModel.Username) && (u.Password == loginModel.Password));

    if (user is null)
      return Unauthorized();

    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Username),
            new Claim(ClaimTypes.Role, "Super Admin")
        };

    var accessToken = _tokenService.GenerateAccessToken(claims);

    var refreshToken = _tokenService.GenerateRefreshToken();
    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    _context.SaveChanges();

    return Ok(new AuthenticatedResponse
    {
      Token = accessToken,
      RefreshToken = refreshToken
    });
  }
}
