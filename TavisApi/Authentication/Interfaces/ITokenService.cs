using System.Security.Claims;

namespace TavisApi.Authentication.Interfaces;

public interface ITokenServiceV2 {
	/// <summary>
	/// Generates a JSON Web Token (JWT) access token.
	/// </summary>
	/// <param name="claims">A collection of claims to be included in the token.</param>
	/// <returns>A string representing the generated JWT access token.</returns>
	/// <exception cref="Exception">Thrown if the encryption key is not found in environment variables.</exception>
	string GenerateAccessToken(IEnumerable<Claim> claims);

	/// <summary>
	/// Generates a secure random refresh token.
	/// </summary>
	/// <returns>A base64 encoded string representing the refresh token.</returns>
	string GenerateRefreshToken();

	/// <summary>
	/// Extracts the <see cref="ClaimsPrincipal"/> from an expired JWT token.
	/// </summary>
	/// <param name="token">The expired JWT token.</param>
	/// <returns>The <see cref="ClaimsPrincipal"/> extracted from the token.</returns>
	/// <exception cref="SecurityTokenException">Thrown when the token is invalid.</exception>
	ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

	/// <summary>
	/// Regenerates the access and refresh tokens for a given user and includes their roles.
	/// </summary>
	/// <param name="user">The user for whom the tokens are being regenerated.</param>
	/// <returns>
	/// A tuple containing the new access token, refresh token, and a list of roles associated with the user.
	/// </returns>
	/// <exception cref="Exception">Thrown when no login is found for the user.</exception>
	Task<(string access, string refresh, List<string> roles)> RegenerateTokenWithRoles(User user);

	/// <summary>
	/// Validates a JWT token and returns the associated ClaimsPrincipal if valid.
	/// </summary>
	/// <param name="token">The JWT token to validate.</param>
	/// <returns>
	/// A <see cref="ClaimsPrincipal"/> representing the user claims if the token is valid; otherwise, <c>null</c>.
	/// </returns>
	/// <exception cref="Exception">Thrown when token validation fails.</exception>
	ClaimsPrincipal? ValidateToken(string token);

	/// <summary>
	/// Extracts the JWT token from the Authorization header in the HTTP request.
	/// </summary>
	/// <param name="request">The HTTP request containing the Authorization header.</param>
	/// <returns>
	/// The JWT token as a string if the Authorization header is present and correctly formatted; otherwise, <c>null</c>.
	/// </returns>
	string? ExtractJwtTokenFromRequest(HttpRequest request);
}