using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.V2.Bcm.Players {
	[ApiController]
	[Route("v2/bcm/players")]
	public class BcmPlayersController : ControllerBase {
		private TavisContext _context;
		private IBcmService _bcmService;
		private IStatsService _statsService;

		public BcmPlayersController(TavisContext context, IBcmService bcmService, IStatsService statsService)
		{
			_context = context;
			_bcmService = bcmService;
			_statsService = statsService;
			_statsService = statsService;
		}

		[HttpGet]
		[Route("getPlayerList")]
		public IActionResult Get()
		{
			var bcmPlayers = _context.BcmPlayers.Include(x => x.User).Select(x => x.User!.Gamertag).ToList();
			return Ok(bcmPlayers);
		}

		[HttpGet, Authorize(Roles = "Admin, Bcm Admin, Sponsor, Enthusiast")]
		[Route("getCompareStats")]
		public IActionResult GetCompareStats(string player)
		{
			var p = _context.BcmPlayers
																.Where(x => x.User!.Gamertag == player)
																.Select(x => new {
																	BcmPlayer = x,
																	x.User,
																	FilteredGames = x.BcmPlayerGames
																				.Where(g => g.CompletionDate != null && g.CompletionDate.Value.Year == DateTime.Now.Year)
																				.Join(_context.Games, pg => pg.GameId, g => g.Id, (pg, g) => new { pg, g })
																				.ToList()
																})
																.FirstOrDefault();

			if (p == null) { return BadRequest("No user found"); }

			var pStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == p.BcmPlayer.Id);

			var gameWithHighestPoints = p.FilteredGames
																			.Select(completion => new {
																				Completion = completion,
																				PointValue = _bcmService.CalcBcmValue(completion.pg.Platform, completion.g.SiteRatio, completion.g.FullCompletionEstimate)
																			})
																			.OrderByDescending(x => x.PointValue)
																			.FirstOrDefault();

			double? highestPointValue = gameWithHighestPoints?.PointValue;
			var gameWithHighestPointValue = gameWithHighestPoints?.Completion;

			var stats = new BcmCompareStats {
				Avatar = p.User?.Avatar,
				Region = p.User?.Region ?? "N/A",
				TotalBaseBcmPoints = pStats?.BasePoints,
				TotalBonusPoints = pStats?.BonusPoints,
				AverageMonthlyPoints = (pStats?.BasePoints + pStats?.BonusPoints) / DateTime.Today.Month,
				HighestRatioGame = p.FilteredGames.Where(x => x.g != null).OrderByDescending(x => x.g.SiteRatio).First().g,
				LongestGame = p.FilteredGames.Where(x => x.g != null).OrderByDescending(x => x.g.FullCompletionEstimate).First().g,
				BestBcmGame = gameWithHighestPointValue?.g,
				BestBcmGamePoints = highestPointValue,
				BcmRank = pStats?.Rank ?? -1,
			};

			return Ok(stats);
		}
	}
}
