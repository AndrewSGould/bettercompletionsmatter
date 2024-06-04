using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.V2.Authentication;
using TavisApi.V2.Users;
using TavisApi.V2.Utils;

namespace TavisApi.V2.OXbl;

[Route("/v2/oxbl/")]
[ApiController]
public class OpenXblControllerV2 : ControllerBase {
	private readonly TavisContext _context;
	private readonly ITokenServiceV2 _tokenService;
	private readonly IOpenXblServiceV2 _oxblService;
	private readonly IUserServiceV2 _userService;
	private readonly IUtils _utils;

	public OpenXblControllerV2(TavisContext context, ITokenServiceV2 tokenService, IOpenXblServiceV2 oxblService, IUtils utils, IUserServiceV2 userService)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_utils = utils ?? throw new ArgumentNullException(nameof(utils));
		_tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
		_oxblService = oxblService ?? throw new ArgumentNullException(nameof(oxblService));
		_userService = userService ?? throw new ArgumentNullException(nameof(userService));
	}

	[HttpPost, Route("login")]
	public async Task<IActionResult> Login([FromBody] XblLogin oxblLogin)
	{
		if (oxblLogin is null || oxblLogin.OpenXblCode is null)
			return BadRequest("Invalid client request");

		var oxblAppKey = _utils.GetEnvVar("OXBL_APP_KEY");

		var oxblAuth = new OXblLogin {
			Code = oxblLogin.OpenXblCode,
			App_Key = oxblAppKey
		};

		var oxblProfile = await _oxblService.Connect(oxblAuth);
		if (oxblProfile.App_Key is null) return BadRequest("Invalid client request");

		var user = _context.Users.Include(u => u.UserRoles).Include(l => l.Login).FirstOrDefault(x => x.Gamertag == oxblProfile.Gamertag);

		if (user is null) {
			_context.Users.Add(new User {
				Xuid = oxblProfile.Xuid
			});

			await _context.SaveChangesAsync();

			user = _context.Users.FirstOrDefault(x => x.Xuid == oxblProfile.Xuid);

			user!.Login = new Login {
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
			foreach (var role in userRolesWithDetails) {
				claims.Add(new Claim(ClaimTypes.Role, role.Roles.RoleName));
			}

		var accessToken = _tokenService.GenerateAccessToken(claims);
		var refreshToken = _tokenService.GenerateRefreshToken();

		user.Gamertag = oxblProfile.Gamertag;
		user.Avatar = oxblProfile.Avatar;
		user.Login!.RefreshToken = refreshToken;
		user.Login.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

		await _context.SaveChangesAsync();

		return Ok(new AuthenticatedResponse {
			Token = accessToken,
			RefreshToken = refreshToken,
			Gamertag = oxblProfile.Gamertag,
			Avatar = oxblProfile.Avatar,
			Roles = userRolesWithDetails.Select(x => x.Roles.RoleName).OrderBy(x => x).ToList()
		});
	}

	[HttpGet, Route("user")]
	public async Task<IActionResult> GetXboxUser(string gamertag)
	{
		var currentUsername = _userService.GetCurrentUserName();

		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == currentUsername);
		if (localuser is null) return BadRequest("No user found with supplied Gamertag");

		var xboxLogin = _context.Logins.FirstOrDefault(x => x.UserId == localuser.Id);
		XblProfiles oxblProfiles = new();

		if (xboxLogin is null) return BadRequest("No xbox profile associated with user");

		var targetXuid = _context.Users.FirstOrDefault(x => x.Gamertag == gamertag)?.Xuid;
		if (targetXuid is null) return BadRequest("No xbox profile associated with requested user");

		var response = await _oxblService.Get(xboxLogin.Password, "account", targetXuid);

		string responseContent = await response.Content.ReadAsStringAsync();
		oxblProfiles = JsonConvert.DeserializeObject<XblProfiles>(responseContent);

		return Ok(oxblProfiles?.ProfileUsers[0]);
	}
}
