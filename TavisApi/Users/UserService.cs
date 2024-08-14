using TavisApi.Authentication.Interfaces;
using TavisApi.Users.Interfaces;
using TavisApi.Users.Models;

namespace TavisApi.Users;

public class UserServiceV2 : IUserServiceV2 {
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ITokenServiceV2 _tokenService;
	private readonly TavisContext _context;

	public UserServiceV2(IHttpContextAccessor httpContextAccessor, TavisContext context, ITokenServiceV2 tokenService)
	{
		_httpContextAccessor = httpContextAccessor;
		_tokenService = tokenService;
		_context = context;
	}

	public string? GetCurrentUserName()
	{
		var identity = _httpContextAccessor.HttpContext?.User.Identity;
		if (identity != null && identity.IsAuthenticated) {
			return identity.Name;
		}

		// Try to rehydrate the identity using JWT token
		var token = _tokenService.ExtractJwtTokenFromRequest(_httpContextAccessor.HttpContext!.Request);
		if (token == null) return null;

		var claimsPrincipal = _tokenService.ValidateToken(token);
		if (claimsPrincipal == null) return null;

		return claimsPrincipal.Identity?.Name;
	}

	public User GetCurrentUser()
	{
		var user = _context.Users.FirstOrDefault(x => x.Gamertag == GetCurrentUserName());
		if (user == null) throw new UnauthorizedAccessException("Cant get current user");

		return user;
	}
}
