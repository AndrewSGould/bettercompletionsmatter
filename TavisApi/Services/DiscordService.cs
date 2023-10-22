using System.Text;
using Newtonsoft.Json;
using Tavis.Models;

namespace TavisApi.Services;

public class DiscordService : IDiscordService
{
  public async Task<DiscordSigninResponse> Connect(DiscordLogin discordAuth)
  {
    DiscordSigninResponse discordSignin = new();

    string url = "https://discord.com/api/users/@me";

    using (HttpClient _client = new())
    {
      _client.DefaultRequestHeaders.Add("Authorization", $"{discordAuth.TokenType} {discordAuth.AccessToken}");

      var response = await _client.GetStringAsync(url);
      discordSignin = JsonConvert.DeserializeObject<DiscordSigninResponse>(response);
    }

    return discordSignin;
  }
}
