namespace TavisApi.Controllers;

using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Bcm.Models;
using TavisApi.V2.Users;

[ApiController]
[Route("[controller]")]
public class BcmController : ControllerBase {
	private TavisContext _context;
	private readonly IParser _parser;
	private readonly IDataSync _dataSync;
	private readonly IBcmService _bcmService;
	private readonly IUserService _userService;
	private readonly IDiscordService _discordService;
	private readonly IStatsService _statsService;
	private static readonly Random rand = new Random();

	public BcmController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService, IUserService userService, IDiscordService discordService, IStatsService statsService)
	{
		_context = context;
		_parser = parser;
		_dataSync = dataSync;
		_bcmService = bcmService;
		_userService = userService;
		_discordService = discordService;
		_statsService = statsService;
	}

	[HttpGet, Authorize(Roles = "Guest")]
	[Route("getPlayerList")]
	public IActionResult GetPlayerList()
	{
		return Ok(_bcmService.GetPlayers().Select(x => x.User!.Gamertag).ToList());
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("getBcmPlayer")]
	public IActionResult BcmPlayer(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

		if (localuser is null) return BadRequest("No gamertag found with provided player");

		var bcmPlayer = _context.BcmPlayers.First(x => x.UserId == localuser.Id);

		if (bcmPlayer == null) return BadRequest("Player not found");

		var playerBcmStats = _context.BcmStats?.FirstOrDefault(x => x.PlayerId == bcmPlayer.Id);

		return Ok(bcmPlayer);
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("getBcmPlayerWithGames")]
	public IActionResult BcmPlayerWithGames(string player)
	{
		var localuser = _context.Users.Include(x => x.UserRegistrations).FirstOrDefault(x => x.Gamertag == player);

		if (localuser is null) return BadRequest("No gamertag found with provided player");

		var bcmPlayer = _context.BcmPlayers.First(x => x.UserId == localuser.Id);

		if (bcmPlayer == null) return BadRequest("Player not found");

		var registrations = _context.Registrations
				.Include(x => x.UserRegistrations)
				.Where(x => x.UserRegistrations.Any(ur => ur.UserId == localuser.Id))
				.ToList();

		var bcmRegDate = registrations.First(x => x.Id == 1).StartDate;

		var userRegDate = localuser.UserRegistrations.First(x => x.RegistrationId == 1).RegistrationDate; // TODO: BCM

		var playerBcmGames = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.BcmPlayer == bcmPlayer
																																								&& (x.CompletionDate != null
																																								&& x.CompletionDate > userRegDate!.Value.AddDays(-1)
																																								&& x.CompletionDate > bcmRegDate));

		var pointedGames = new List<object>();
		var bonusGamesUsed = _context.MonthlyExclusions.Where(x => x.PlayerId == bcmPlayer.Id).ToList();

		foreach (var game in playerBcmGames) {
			var newGame = new {
				Game = game,
				Bonus = bonusGamesUsed.FirstOrDefault(x => x.GameId == game.GameId)?.Challenge,
				Points = _bcmService.CalcBcmValue(game.Platform, game.Game!.SiteRatio, game.Game.FullCompletionEstimate),
			};

			pointedGames.Add(newGame);
		}

		var fakeCompletions = _context.FakeCompletions.Where(x => x.PlayerId == bcmPlayer.Id).ToList();

		foreach (var fakeCompletion in fakeCompletions) {
			var game = _context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == fakeCompletion.GameId && x.PlayerId == bcmPlayer.Id);
			game.CompletionDate = game.LastUnlock;

			var newGame = new {
				Game = game,
				Bonus = 4
			};

			pointedGames.Add(newGame);
		}

		var avgRatio = playerBcmGames
				.Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate!.Value.AddDays(-1) && x.CompletionDate > bcmRegDate)
				.Select(x => x.Game.SiteRatio)
				.AsEnumerable()
				.DefaultIfEmpty(0)
				.Average();
		var avgTime = playerBcmGames
				.Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate!.Value.AddDays(-1) && x.CompletionDate > bcmRegDate)
				.Select(x => x.Game.FullCompletionEstimate)
				.AsEnumerable()
				.DefaultIfEmpty(0)
				.Average();
		var highestTime = playerBcmGames
				.Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate!.Value.AddDays(-1) && x.CompletionDate > bcmRegDate)
				.Select(x => x.Game.FullCompletionEstimate)
				.AsEnumerable()
				.DefaultIfEmpty(0)
				.Max();
		var highestRatio = playerBcmGames
				.Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate!.Value.AddDays(-1) && x.CompletionDate > bcmRegDate)
				.Select(x => x.Game.SiteRatio)
				.AsEnumerable()
				.DefaultIfEmpty(0)
				.Max();

		return Ok(new {
			Player = bcmPlayer,
			Games = pointedGames,
			AvgRatio = avgRatio,
			AvgTime = avgTime,
			HighestTime = highestTime,
			HighestRatio = highestRatio
		});
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("yearlySummary")]
	public async Task<IActionResult> GetPlayerYearlySummary(string player)
	{
		var localuser = await _context.Users.FirstOrDefaultAsync(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = await _context.BcmPlayers.FirstOrDefaultAsync(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		return Ok(_bcmService.GetParticipationProgress(bcmPlayer));
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("player/abcSummary")]
	public async Task<IActionResult> GetPlayerAbcSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);

		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		return Ok(await _bcmService.GetAlphabetChallengeProgress(bcmPlayer.Id));
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("player/oddjobSummary")]
	public async Task<IActionResult> GetPlayerOddjobSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);

		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		return Ok(await _bcmService.GetOddJobChallengeProgress(bcmPlayer.Id));
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("player/miscstats")]
	public async Task<IActionResult> GetPlayerMiscStats(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);

		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		return Ok();
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly/feb")]
	public async Task<IActionResult> FebSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		return Ok(await _context.FebRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id));
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly/mar")]
	public async Task<IActionResult> MarSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		return Ok(await _context.MarRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id));
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly/jun")]
	public async Task<IActionResult> JunSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		var recap = await _context.JunRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id);
		var progress = _context.BcmPlayerGames.Include(x => x.Game)
																			.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Month == 6 && x.CompletionDate.Value.Year == 2024
																								&& x.Game!.SiteRating >= 4.2 && x.Game!.GamersWithGame >= 2000).ToList();
		var filteredProgress = progress.Where(x => Queries.FilterGamesForYearlies(x.Game!, x));

		return Ok(new {
			recap?.Participation,
			CommunityProgress = filteredProgress.Count(),
			recap?.QualifiedGames,
			recap?.TotalPoints,
		});
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly/jul")]
	public async Task<IActionResult> JulySummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		var recap = await _context.JulyRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id);
		var communityGames = _context.BcmPlayerGames.Include(x => x.Game)
																			.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Month == 7 && x.CompletionDate.Value.Year == 2024
																								&& x.Game.FeatureList.IdAtXbox && x.Game.GamersCompleted <= 1773 && x.Game.Title.ToLower().Contains("t")).ToList();

		communityGames = communityGames.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).ToList();

		var progress = _statsService.CalcJulyCommunityProgress();

		return Ok(new {
			recap?.Participation,
			CommunityProgress = progress,
			recap?.TeaCount,
			recap?.TotalPoints,
		});
	}

	[Authorize(Roles = "Guest")]
	[HttpPost, Route("registerUser")]
	public async Task<IActionResult> RegisterUser()
	{
		try {
			User? user = _userService.GetCurrentUser();
			if (user is null) return BadRequest("Could not determine user");

			var bcmReg = _context.Registrations.Find(_bcmService.GetRegistrationId()) ?? throw new Exception("Unable to get Registration ID for BCM");

			user.UserRegistrations.Add(new UserRegistration { Registration = bcmReg, RegistrationDate = DateTime.UtcNow });

			_context.BcmPlayers.Add(new BcmPlayer {
				UserId = user.Id,
			});

			_context.SaveChanges();

			try {
				await _discordService.AddBcmParticipantRole(user);
				var userInfo = _context.Users.Include(x => x.UserRegistrations)
																	.FirstOrDefault(x => x.UserRegistrations.Any(x => x.User == user && x.Registration.Name == "Better Completions Matter"));

				return Ok(new { RegDate = userInfo?.UserRegistrations.FirstOrDefault()?.RegistrationDate });
			}
			catch {
				return BadRequest("Something went wrong trying to register for BCM");
			}
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[Authorize(Roles = "Participant")]
	[HttpPost, Route("unregisterUser")]
	public async Task<IActionResult> UnregisterUser()
	{
		throw new NotImplementedException();
	}

	[Authorize(Roles = "Participant")]
	[HttpGet, Route("getPlayersGenres")]
	public async Task<IActionResult> GetPlayersGenres(string player)
	{
		var localuser = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("No user found with supplied gamertag");

		var playerId = localuser.BcmPlayer?.Id;
		if (playerId is null) return BadRequest("Could not get Bcm Player");

		var pgs = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.BcmPlayer == localuser.BcmPlayer && x.CompletionDate != null);

		var genreStats = await _context.Genres
			.GroupJoin(
					_context.GameGenres,
					genre => genre.Id,
					gameGenre => gameGenre.GenreId,
					(genre, gameGenres) => new {
						GenreId = genre.Id,
						GenreName = genre.Name,
						GenreCount = gameGenres
						.Join(
								_context.BcmPlayerGames
										.Where(bpg => bpg.PlayerId == playerId && bpg.CompletionDate != null),
								gg => gg.GameId,
								bpg => bpg.GameId,
								(gg, bpg) => gg // Use gg instead of 1
						)
						.Count()
					}
			)
			.OrderByDescending(result => result.GenreCount)
			.Select(x => new {
				Name = x.GenreName,
				Value = x.GenreCount
			})
			.ToListAsync();

		return Ok(genreStats);
	}

	[HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
	[Route("produceStatReport")]
	public IActionResult StatReport()
	{
		var bcmPlayers = _bcmService.GetPlayers();
		var statSpread = new List<object>();

		foreach (var player in bcmPlayers) {
			var user = _context.Users.FirstOrDefault(x => x.Id == player.UserId);

			var playerGames = _context.BcmPlayerGames.Where(x => x.PlayerId == player.Id);

			var gamerscoreTotal = playerGames.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Year == 2024 && x.CompletionDate.Value.Month == DateTime.Now.AddMonths(-1).Month)
																				.Sum(x => x.Gamerscore);
			var trueachievementTotal = playerGames.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Year == 2024 && x.CompletionDate.Value.Month == DateTime.Now.AddMonths(-1).Month)
																						.Sum(x => x.TrueAchievement);
			var completions = playerGames.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Year == 2024 && x.CompletionDate.Value.Month == DateTime.Now.AddMonths(-1).Month)
																		.Count(x => x.CompletionDate != null);

			var stats = new {
				Player = user!.Gamertag,
				Gamerscore = gamerscoreTotal,
				TrueAchievement = trueachievementTotal,
				Ratio = trueachievementTotal == 0 ? 0 : Math.Round((decimal)((decimal)trueachievementTotal! / gamerscoreTotal!), 4),
				TAD = trueachievementTotal == 0 ? 0 : trueachievementTotal - gamerscoreTotal,
				Completions = completions
			};

			statSpread.Add(stats);
		}

		return Ok(statSpread);
	}
}
