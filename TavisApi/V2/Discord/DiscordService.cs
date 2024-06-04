using Discord;
using Discord.Rest;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.V2.Users;
using TavisApi.V2.Utils;

namespace TavisApi.V2.Discord;

public class DiscordServiceV2 : IDiscordServiceV2 {
	private TavisContext _context;
	private IUtils _utils;
	private readonly string _redirectUri;
	public static readonly string CLIENT_ID = Environment.GetEnvironmentVariable("DISCORD_CLIENTID") ?? throw new Exception("Unable to get Discord Client ID");
	public static readonly string SECRET = Environment.GetEnvironmentVariable("DISCORD_OAUTH_SECRET") ?? throw new Exception("Unable to get Discord Secret");

	public DiscordServiceV2(TavisContext context, IWebHostEnvironment environment, IConfiguration configuration, IUtils utils)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_utils = utils ?? throw new ArgumentNullException(nameof(utils));

		if (environment.IsDevelopment())
			_redirectUri = configuration["RedirectUri:Local"];
		else
			_redirectUri = configuration["RedirectUri:Production"];

		if (string.IsNullOrEmpty(_redirectUri)) {
			throw new Exception("Redirect URI is not defined in the configuration.");
		}
	}

	public async Task<RestSelfUser> Connect(string accessToken, User user)
	{
		var token = _utils.GetEnvVar("DISCORD_BOT_TOKEN");

		var bot = new DiscordRestClient();
		await bot.LoginAsync(TokenType.Bot, token);

		var client = new DiscordRestClient();
		await client.LoginAsync(TokenType.Bearer, accessToken);

		var guilds = await client.GetGuildSummariesAsync().FlattenAsync();
		var guildSummary = guilds.FirstOrDefault(x => x.Name.Contains("BCMX"));

		if (guildSummary == null) {
			// TODO: if they don't have BCMX server aka guildSummary is null, join it for them
			throw new NotImplementedException();
		}

		var member = await guildSummary.GetCurrentUserGuildMemberAsync();
		var guild = await bot.GetGuildAsync(guildSummary.Id);

		UpdateLocalDiscordRoles(guild, member, user);

		return client.CurrentUser;
	}

	public async Task AddBcmParticipantRole(User user)
	{
		if (user == null) throw new Exception("No user when trying to connect to Discord");

		var dProfile = _context.DiscordLogins.FirstOrDefault(x => x.UserId == user.Id);
		if (dProfile is null || dProfile.AccessToken is null) throw new Exception("No discord profile with supplied user");

		await AddDiscordRole(dProfile.AccessToken, "Participant");
	}

	private async Task AddDiscordRole(string accessToken, string roleName)
	{
		var discordBotToken = _utils.GetEnvVar("DISCORD_BOT_TOKEN");

		using var bot = new DiscordRestClient();
		await bot.LoginAsync(TokenType.Bot, discordBotToken);

		using var client = new DiscordRestClient();
		await client.LoginAsync(TokenType.Bearer, accessToken);

		Role partipRole = _context.Roles.First(x => x.RoleName == roleName);

		var guilds = await bot.GetGuildSummariesAsync().FlattenAsync();
		var guildSummary = guilds.FirstOrDefault(x => x.Name.Contains("BCMX"));

		if (guildSummary is null || partipRole is null)
			throw new Exception("Full context unavailable when trying to update Discord Role");

		await bot.AddRoleAsync(guildSummary.Id, client.CurrentUser.Id, partipRole.DiscordId);
	}

	public async Task<(string access, string refresh)> Handshake(string code, User user)
	{
		var parameters = new Dictionary<string, string>
			{
				{ "client_id", CLIENT_ID },
				{ "client_secret", SECRET },
				{ "grant_type", "authorization_code" },
				{ "code", code },
				{ "redirect_uri", _redirectUri }
			};

		var content = new FormUrlEncodedContent(parameters);

		using var httpClient = new HttpClient();
		var response = await httpClient.PostAsync("https://discord.com/api/oauth2/token", content);

		if (!response.IsSuccessStatusCode) {
			throw new HttpRequestException($"Failed to exchange code for access token: {response.ReasonPhrase}");
		}

		var tokenResponse = await response.Content.ReadAsStringAsync();
		var tokenJson = JObject.Parse(tokenResponse);
		var accessToken = tokenJson.Value<string>("access_token");
		var refreshToken = tokenJson.Value<string>("refresh_token");

		if (accessToken == null || refreshToken == null) throw new Exception("Couldn't get tokens from Discord");

		return (accessToken, refreshToken);
	}

	public async void UpdateDiscordAuth(ulong discordId, User user, (string access, string refresh) tokens)
	{
		var dLogin = _context.DiscordLogins.FirstOrDefault(x => x.DiscordId == discordId);

		if (dLogin is null) {
			_context.DiscordLogins.Add(new DiscordLogin {
				DiscordId = discordId,
				UserId = user.Id,
				TokenType = "Bearer",
				AccessToken = tokens.access,
				RefreshToken = tokens.refresh,
			});
		}
		else {
			dLogin.AccessToken = tokens.access;
			dLogin.RefreshToken = tokens.refresh;
		}

		await _context.SaveChangesAsync();
	}

	private async void UpdateLocalDiscordRoles(RestGuild guild, RestGuildUser member, User user)
	{
		var newRoles = guild.Roles
										.Where(role => !_context.Roles.Any(userRole => userRole.DiscordId == role.Id))
										.Select(role => new { role.Id, role.Name });

		foreach (var role in newRoles) {
			_context.Roles.Add(new Role {
				DiscordId = role.Id,
				RoleName = role.Name
			});
		}
		await _context.SaveChangesAsync();

		var userWithRoles = _context.Users.Include(u => u.UserRoles).FirstOrDefault(x => x == user);

		foreach (var roleId in member.RoleIds) {
			var dbRole = _context.Roles.First(x => x.DiscordId == roleId);

			if (userWithRoles?.UserRoles.Find(x => x.RoleId == dbRole.Id) == null) {
				userWithRoles?.UserRoles.Add(new UserRole {
					Role = dbRole
				});
			}
		}

		await _context.SaveChangesAsync();
	}

	public async Task<string> RefreshAccessTokenAsync(string refreshToken)
	{
		var client = new HttpClient();

		var values = new Dictionary<string, string>
			{
						{ "client_id", CLIENT_ID },
						{ "client_secret", SECRET },
						{ "grant_type", "refresh_token" },
						{ "refresh_token", refreshToken }
				};

		var content = new FormUrlEncodedContent(values);

		var response = await client.PostAsync("https://discord.com/api/oauth2/token", content);

		if (response.IsSuccessStatusCode) {
			var responseString = await response.Content.ReadAsStringAsync();
			return responseString;
		}
		else {
			var errorResponse = await response.Content.ReadAsStringAsync();
			throw new Exception($"Error refreshing token: {response.StatusCode} - {errorResponse}");
		}
	}
}
