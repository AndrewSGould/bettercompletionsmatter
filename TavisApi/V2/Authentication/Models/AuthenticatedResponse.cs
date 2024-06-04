namespace TavisApi.V2.Authentication;

public class AuthenticatedResponse {
	public string? Token { get; set; }
	public string? RefreshToken { get; set; }
	public string? Gamertag { get; set; }
	public string? Avatar { get; set; }
	public List<string>? Roles { get; set; }
}
