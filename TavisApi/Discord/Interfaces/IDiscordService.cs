using Discord.Rest;

namespace TavisApi.Discord.Interfaces;

public interface IDiscordServiceV2 {
	/// <summary>
	/// Connects a user to the Discord server, retrieves their guild and roles, and updates the database with any new roles.
	/// </summary>
	/// <param name="accessToken">The access token for the user to connect to Discord.</param>
	/// <param name="user">The user attempting to connect to Discord.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the <see cref="RestSelfUser"/> object representing the connected user.
	/// </returns>
	/// <exception cref="Exception">Thrown if the bot token is not available or if the guild summary is null and joining the guild fails.</exception>
	Task<RestSelfUser> Connect(string accessToken, User user);

	/// <summary>
	/// Adds the "Participant" role to the specified user in Discord.
	/// </summary>
	/// <param name="user">The user to whom the role will be added.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <exception cref="Exception">Thrown when the user is null or no corresponding Discord profile is found.</exception>
	Task AddBcmParticipantRole(User user);

	/// <summary>
	/// Exchanges the authorization code obtained from the Discord authorization endpoint for access and refresh tokens.
	/// </summary>
	/// <param name="code">The authorization code obtained from the Discord authorization endpoint.</param>
	/// <param name="user">The user requesting the connection.</param>
	/// <returns>
	/// A tuple containing the access and refresh tokens.
	/// The access token is used to authenticate API requests to Discord on behalf of the user.
	/// The refresh token is used to obtain a new access token when the current access token expires.
	/// </returns>
	/// <exception cref="HttpRequestException">Thrown when an error occurs during the HTTP request.</exception>
	/// <exception cref="Exception">Thrown if either the access token or refresh token is null.</exception>
	Task<(string access, string refresh)> Handshake(string code, User user);

	/// <summary>
	/// Updates the Discord authentication tokens for a user in the database.
	/// </summary>
	/// <param name="discordId">The Discord ID of the user.</param>
	/// <param name="user">The user associated with the Discord authentication.</param>
	/// <param name="tokens">A tuple containing the access and refresh tokens.</param>
	/// <remarks>This method does not return a value.</remarks>
	void UpdateDiscordAuth(ulong discordId, User user, (string access, string refresh) tokens);

	Task<string> RefreshAccessTokenAsync(string refreshToken);
}