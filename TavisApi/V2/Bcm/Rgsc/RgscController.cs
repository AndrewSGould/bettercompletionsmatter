using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Bcm.Rgsc.Models;

namespace TavisApi.V2.Bcm.Rgsc {
	[ApiController]
	[Route("/v2/bcm/rgsc")]
	public class RgscController : ControllerBase {
		private TavisContext _context;
		private readonly IRgscService _rgscService;
		private readonly IBcmService _bcmService;

		public RgscController(TavisContext context, IBcmService bcmService, IRgscService rgscService)
		{
			_context = context;
			_rgscService = rgscService;
			_bcmService = bcmService;
		}

		[HttpPost, Authorize(Roles = "Admin")]
		[Route("rollRandom")]
		public IActionResult GenerateRandom([FromBody] RandomRoll roll)
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

			var nextChallenge = 9;

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
	}
}
