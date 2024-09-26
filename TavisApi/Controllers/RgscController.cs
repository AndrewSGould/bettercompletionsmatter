namespace TavisApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Bcm.Rgsc.Models;

[ApiController]
[Route("[controller]")]
public class RgscController : ControllerBase {
	private TavisContext _context;
	private readonly IParser _parser;
	private readonly IDataSync _dataSync;
	private readonly IRgscService _rgscService;
	private readonly IBcmService _bcmService;

	public RgscController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService, IRgscService rgscService)
	{
		_context = context;
		_parser = parser;
		_dataSync = dataSync;
		_rgscService = rgscService;
		_bcmService = bcmService;
	}

	[HttpPost, Authorize(Roles = "Admin")]
	[Route("rollRandom")]
	public IActionResult RollRandomGame([FromBody] RandomRoll roll)
	{
		var players = _context.BcmPlayers.Include(u => u.User).Include(x => x.BcmRgscs).ToList();
		var currentBcmPlayer = players.FirstOrDefault(x => x.User!.Gamertag == roll.selectedPlayer);

		if (currentBcmPlayer is null) {
			players = players.Where(x => x.BcmRgscs == null || x.BcmRgscs.Count() == 0 || x.BcmRgscs
												.OrderByDescending(x => x.Issued)
												.First().Issued <= DateTime.UtcNow.AddDays(-1))
												.ToList();

			if (players.Count() < 1) return BadRequest("no users left to random");

			var playerIndex = new Random().Next(0, players.Count);
			currentBcmPlayer = players[playerIndex];
		}

		_context.Attach(currentBcmPlayer);

		var randomGameOptions = _rgscService.GetEligibleRandoms(currentBcmPlayer);
		var currentRandoms = _context.BcmRgsc.Where(x => !x.Rerolled && x.BcmPlayerId == currentBcmPlayer.Id);

		// if we get a game, they are rerolling an old game
		var rolledRandom = currentRandoms.FirstOrDefault(x => x.GameId == roll.selectedGameId);

		if (roll.selectedGameId != -1 && rolledRandom is not null) {
			rolledRandom.Rerolled = true;
			rolledRandom.RerollDate = DateTime.UtcNow;
		}

		var nextChallenge = 10;

		if (randomGameOptions is null || randomGameOptions?.Count() < 50) {
			if (roll.selectedGameId == -1 && rolledRandom is null) {
				// they are rerolling an invalid game, but it's still not valid
				var mostRecentRandom = currentRandoms.OrderByDescending(x => x.Challenge).First();
				mostRecentRandom.Issued = DateTime.UtcNow;
				mostRecentRandom.PoolSize = randomGameOptions?.Count() ?? 0;
			}
			else {
				_context.BcmRgsc.Add(new BcmRgsc {
					Issued = DateTime.UtcNow,
					GameId = null,
					BcmPlayerId = currentBcmPlayer.Id,
					PreviousGameId = roll.selectedGameId != -1 && roll.selectedGameId != null ? rolledRandom!.GameId : null,
					Challenge = roll.selectedGameId != -1 && roll.selectedGameId != null ? rolledRandom!.Challenge : nextChallenge,
					PoolSize = randomGameOptions?.Count() ?? 0
				});
			}

			_context.SaveChanges();

			return Ok(new { PoolSize = randomGameOptions?.Count() ?? 0, currentBcmPlayer.User });
		}

		var randomIndex = new Random().Next(0, randomGameOptions!.Count);
		var currentRandom = randomGameOptions[randomIndex];

		_context.BcmRgsc.Add(new BcmRgsc {
			Issued = DateTime.UtcNow,
			GameId = currentRandom.Game!.Id,
			BcmPlayerId = currentBcmPlayer.Id,
			Challenge = roll.selectedGameId != -1 && roll.selectedGameId != null ? rolledRandom!.Challenge : nextChallenge,
			PreviousGameId = roll.selectedGameId != -1 && roll.selectedGameId != null ? rolledRandom!.GameId : null,
			PoolSize = randomGameOptions?.Count() ?? 0
		});

		_context.SaveChanges();

		return Ok(new {
			PoolSize = randomGameOptions?.Count() ?? 0,
			currentBcmPlayer.User,
			Result = currentRandom,
			BcmValue = _bcmService.CalcBcmValue(currentRandom.Platform, currentRandom.Game.SiteRatio, currentRandom.Game.FullCompletionEstimate)
		});
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("user-summary")]
	public IActionResult RgscStats(string player)
	{
		var localuser = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("No user found with supplied gamertag");

		var playerId = localuser.BcmPlayer?.Id;
		if (playerId is null) return BadRequest("Could not get Bcm Player");

		var rgsc = _context.BcmRgsc.Where(x => x.BcmPlayerId == playerId)
																.OrderByDescending(x => x.Issued)
																.ToList();

		var playersCompletedGames = _context.BcmPlayerGames.Where(x => x.PlayerId == playerId
																											&& x.CompletionDate != null
																											&& x.CompletionDate.Value.Year == 2024).ToList();

		var rgscList = rgsc
				.GroupJoin(_context.Games,
									 rgsc => rgsc.GameId,
									 g => g.Id,
									 (rgsc, games) => new { Rgsc = rgsc, Games = games.DefaultIfEmpty() })
				.SelectMany(x => x.Games.Select(g => new { Rgsc = x.Rgsc, Game = g }));

		var rgscCompletions = rgsc.Join(playersCompletedGames, rgsc => rgsc.GameId,
																pg => pg.GameId, (rgsc, pg) => new { Rgsc = rgsc, PlayerGames = pg })
															.ToList();


		var rerollsUsed = rgsc.Count(x => x.Rerolled);

		var nonrerolledRgsc = rgsc.FirstOrDefault(x => !x.Rerolled)?.GameId;
		var currentRgscs = _context.Games.Where(x => x.Id == nonrerolledRgsc);

		return Ok(new {
			CurrentRandoms = rgscList.Where(x => !x.Rgsc.Rerolled).OrderByDescending(x => x.Rgsc.Challenge),
			RerollsRemaining = BcmRule.RgscStartingRerolls + rgscCompletions.Count() - rerollsUsed,
			RgscsCompleted = rgscCompletions.Where(x => x.Rgsc.Rerolled == false),
			RandomsRolledAway = rgscList.Where(x => x.Rgsc.Rerolled),
		});
	}

	[HttpGet]
	[Authorize(Roles = "Guest")]
	[Route("getPlayersGames")]
	public IActionResult GetPlayersGames(string player)
	{
		var user = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
		if (user is null) return BadRequest("No user found");

		var bcmPlayer = _context.BcmPlayers.Include(x => x.BcmRgscs).FirstOrDefault(x => x.Id == user.BcmPlayer!.Id);
		var rgscList = bcmPlayer?.BcmRgscs
			?.Where(x => !x.Rerolled)
			?.Select(x => new {
				GameId = x.GameId ?? -1,
				Title = _context.Games.FirstOrDefault(y => y.Id == x.GameId)?.Title ?? "No Random Drawn"
			});

		return Ok(new {
			Randoms = rgscList,
			// todo; change this to rgsc service that returns a users reroll count
			RerollsRemaining = _rgscService.GetUserRerollCount(user.Id)
		});
	}
}
