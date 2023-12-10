using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dotenv.net;
using Microsoft.IdentityModel.Tokens;
using TavisApi.Services;

public class TokenService : ITokenService
{
  private readonly string _apiServer;
  private readonly string _clientServer;

  public TokenService(IConfiguration config)
  {
    var serverConfigs = config.GetSection("ServerConfigs");

    _apiServer = serverConfigs["IssuerServer"] ?? throw new Exception("No IssuerServer defined");
    _clientServer = serverConfigs["AudienceServer"] ?? throw new Exception("No AudienceServer defined");
  }

  public string GenerateAccessToken(IEnumerable<Claim> claims)
  {
    var envVars = DotEnv.Read();
    var encryptionKey = envVars.TryGetValue("ENCRYPTION_KEY", out var key) ? key : null;
    if (encryptionKey is null || encryptionKey == "") encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")!;

    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey));
    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var tokenOptions = new JwtSecurityToken(
      issuer: _apiServer,
      audience: _clientServer,
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(10000), // TODO: 1 week, let's fix the token refresh at some point to lower this
      signingCredentials: signinCredentials
    );
    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    return tokenString;
  }

  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);
    }
  }

  public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var envVars = DotEnv.Read();
    var encryptionKey = envVars.TryGetValue("ENCRYPTION_KEY", out var key) ? key : null;
    if (encryptionKey is null || encryptionKey == "") encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")!;

    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = true,
      ValidateIssuer = true,
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
}
