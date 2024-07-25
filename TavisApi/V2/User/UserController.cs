using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.V2.Authentication;


namespace TavisApi.V2.Users;

[Route("/v2/user/")]
[ApiController]
public class UserControllerV2 : ControllerBase {
	private readonly TavisContext _context;
	private readonly IUserServiceV2 _userService;

	public UserControllerV2(TavisContext context, ITokenServiceV2 tokenService, IUserServiceV2 userService)
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

	[Authorize(Roles = "Guest")]
	[HttpPut, Route("updateLocation")]
	public IActionResult UpdateLocation([FromBody] Location location)
	{
		var user = _userService.GetCurrentUser();
		if (user is null) return BadRequest("User not found upon request");

		user.Region = location.Country;
		user.Area = location.State;

		_context.SaveChanges();

		return Ok();
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("getLocation")]
	public IActionResult GetLocation()
	{
		var user = _userService.GetCurrentUser();
		if (user is null) return BadRequest("User not found upon request");

		return Ok(new Location { Country = user?.Region, State = user?.Area });
	}
}
