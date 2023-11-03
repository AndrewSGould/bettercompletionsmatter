namespace WebApi.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly TavisContext _context;
  private readonly IUserService _userService;

  public UserController(TavisContext context, ITokenService tokenService, IUserService userService)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _userService = userService ?? throw new ArgumentNullException(nameof(userService));
  }

  [HttpGet, Route("availableRegions")]
  public IActionResult AvailableRegions()
  {
    return Ok(_context.Players.Select(x => x.Region).Where(x => x != null).Distinct().OrderBy(x => x));
  }

  [HttpGet, Route("availableAreas")]
  public IActionResult AvailableAreas(string selectedRegion)
  {
    return Ok(_context.Players.Where(x => x.Region.Contains(selectedRegion)).Select(x => x.Area).Where(x => x != null).Distinct().OrderBy(x => x));
  }

  [HttpGet, Route("getRoles")]
  public IActionResult GetRoles()
  {
    var currentUsername = _userService.GetCurrentUserName();
    var user = _context.Users.Include(u => u.UserRoles).FirstOrDefault(x => x.Gamertag == currentUsername);

    if (user is null) return BadRequest("no user");

    return Ok(user.UserRoles.Select(x => x.RoleName));
  }
}
