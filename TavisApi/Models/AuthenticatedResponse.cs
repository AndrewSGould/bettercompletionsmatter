namespace Tavis.Models;

public class AuthenticatedResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Gamertag { get; set; }
    public string Avatar { get; set; }
}
