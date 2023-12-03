namespace WebApi.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Route("[controller]")]
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

  [HttpGet, Route("getRoles")]
  public IActionResult GetRoles()
  {
    var currentUsername = _userService.GetCurrentUserName();
    var user = _context.Users.Include(u => u.UserRoles).FirstOrDefault(x => x.Gamertag == currentUsername);

    if (user is null) return BadRequest("no user");

    var userRolesWithDetails = user.UserRoles.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { UserRoles = ur, Roles = r });

    return Ok(userRolesWithDetails.Select(x => x.Roles.RoleName));
  }

  [Authorize(Roles = "Guest")]
  [HttpGet, Route("getRegistrations")]
  public IActionResult GetRegistrations()
  {
    var user = _userService.GetCurrentUser();
    if (user is null) return BadRequest("User not found upon request");

    var registrations = _context.Registrations
                            .Include(x => x.UserRegistrations)
                            .FirstOrDefault(x => x.UserRegistrations.Any(x => x.User == user));

    return Ok(new { registrations?.Name, Date = registrations?.UserRegistrations.FirstOrDefault()?.RegistrationDate });
  }
}
