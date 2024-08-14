using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TavisApi.Authentication.Interfaces;
using TavisApi.Users.Models;

namespace TavisApi.Authentication;

public class TokenServiceV2 : ITokenServiceV2 {
	private readonly TavisContext _context;
	private IUtils _utils;
	private readonly string _apiServer;
	private readonly string _clientServer;

	public TokenServiceV2(TavisContext context, IConfiguration config, IUtils utils)
	{
		var serverConfigs = config.GetSection("ServerConfigs");

		_context = context ?? throw new ArgumentNullException(nameof(context));
		_utils = utils ?? throw new ArgumentNullException(nameof(utils));
		_apiServer = serverConfigs["IssuerServer"] ?? throw new Exception("No IssuerServer defined");
		_clientServer = serverConfigs["AudienceServer"] ?? throw new Exception("No AudienceServer defined");
	}

	public string GenerateAccessToken(IEnumerable<Claim> claims)
	{
		var encryptionKey = _utils.GetEnvVar("ENCRYPTION_KEY");

		var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey));
		var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

		var tokenOptions = new JwtSecurityToken(
				issuer: _apiServer,
				audience: _clientServer,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(15),
				signingCredentials: signinCredentials
		);
		var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		return tokenString;
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using (var rng = RandomNumberGenerator.Create()) {
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var encryptionKey = _utils.GetEnvVar("ENCRYPTION_KEY");

		var tokenValidationParameters = new TokenValidationParameters {
			ValidateAudience = true,
			ValidAudience = _clientServer,
			ValidateIssuer = true,
			ValidIssuer = _apiServer,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey)),
			ValidateLifetime = false // we don't care about the token's expiration date
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		SecurityToken securityToken;
		var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
		var jwtSecurityToken = securityToken as JwtSecurityToken;
		if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			throw new SecurityTokenException("Invalid token");

		return principal;
	}

	public async Task<(string access, string refresh, List<string> roles)> RegenerateTokenWithRoles(User user)
	{
		List<Claim> claims;
		List<string> userRoles;

		claims = new List<Claim>
		{
						new Claim(ClaimTypes.Name, user.Gamertag!),
				};

		var userRolesWithDetails = user.UserRoles.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { UserRoles = ur, Roles = r });

		foreach (var role in userRolesWithDetails) {
			claims.Add(new Claim(ClaimTypes.Role, role.Roles.RoleName));
		}

		userRoles = userRolesWithDetails.Select(x => x.Roles.RoleName).ToList();

		string accessToken;
		string refreshToken;

		var login = _context.Logins.FirstOrDefault(x => x.UserId == user.Id);
		if (login is null) throw new Exception("No login found when regenerating token");

		accessToken = GenerateAccessToken(claims);
		refreshToken = GenerateRefreshToken();
		login.RefreshToken = refreshToken;
		login.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

		await _context.SaveChangesAsync();

		return (accessToken, refreshToken, userRoles);
	}

	public string? RefreshToken(string accessToken)
	{
		var principal = GetPrincipalFromExpiredToken(accessToken);
		var gamertag = principal.Identity?.Name; //this is mapped to the Name claim by default
		var user = _context.Logins.SingleOrDefault(u => u.User.Gamertag == gamertag);

		if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
			return null;

		var newAccessToken = GenerateAccessToken(principal.Claims);
		var newRefreshToken = GenerateRefreshToken();
		user.RefreshToken = newRefreshToken;

		_context.SaveChanges();

		return newAccessToken;
	}

	public ClaimsPrincipal? ValidateToken(string token)
	{
		if (token == null) return null;

		var encryptionKey = _utils.GetEnvVar("ENCRYPTION_KEY");

		var tokenValidationParameters = new TokenValidationParameters {
			ValidateAudience = true,
			ValidAudience = _clientServer,
			ValidateIssuer = true,
			ValidIssuer = _apiServer,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey)),
			ValidateLifetime = true
		};

		try {
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

			if (!IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;

			return principal;
		}
		catch (SecurityTokenExpiredException) {
			// Token is expired, attempt token refresh here
			var refreshedToken = RefreshToken(token);
			var refreshedPrincipal = ValidateToken(refreshedToken!);

			if (refreshedPrincipal != null) {
				// Token refresh successful, return the refreshed principal
				return refreshedPrincipal;
			}

			// Token refresh failed or other error handling
			return null;
		}
		catch (Exception) {
			// Token validation failed
			return null;
		}
	}

	private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
	{
		return validatedToken is JwtSecurityToken jwtToken &&
								 jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
	}

	public string? ExtractJwtTokenFromRequest(HttpRequest request)
	{
		var authHeader = request.Headers["Authorization"].FirstOrDefault();
		if (authHeader == null || !authHeader.StartsWith("Bearer ")) return null;

		return authHeader.Substring("Bearer ".Length).Trim();
	}
}
