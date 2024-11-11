using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.Controllers;

[Route("/v2/bcm/november")]
[ApiController]
public class NovemberController : ControllerBase {
	private TavisContext _context;
	private readonly IUserService _userService;
	private readonly IBcmService _bcmService;
	private readonly IStatsService _statsService;

	public NovemberController(TavisContext context, IUserService userService, IBcmService bcmService, IStatsService statsService)
	{
		_context = context;
		_userService = userService;
		_bcmService = bcmService;
		_statsService = statsService;
	}

	[HttpGet, Route("leaderboard")]
	public async Task<IActionResult> GetNovLeaderboard()
	{
		var recap = await _context.NovemberRecap.ToListAsync();
		return Ok(recap);
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly")]
	public async Task<IActionResult> NovemberSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		var recap = await _context.NovemberRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id);

		return Ok(new {
			recap.Participation,
			recap.TotalPoints,
			CommunityPodiums = _context.NovemberRecap
				.Sum(x => (x.Podium2019_1st) + (x.Podium2019_2nd) + (x.Podium2019_3rd) +
					(x.Podium2020_1st) + (x.Podium2020_2nd) + (x.Podium2020_3rd) +
					(x.Podium2021_1st) + (x.Podium2021_2nd) + (x.Podium2021_3rd) +
					(x.Podium2022_1st) + (x.Podium2022_2nd) + (x.Podium2022_3rd) +
					(x.Podium2023_1st) + (x.Podium2023_2nd) + (x.Podium2023_3rd)
				),
			Podiums2019 = _context.NovemberRecap.Where(x => x.PlayerId == bcmPlayer.Id).Sum(x => (x.Podium2019_1st) + (x.Podium2019_2nd) + (x.Podium2019_3rd)),
			Podiums2020 = _context.NovemberRecap.Where(x => x.PlayerId == bcmPlayer.Id).Sum(x => (x.Podium2020_1st) + (x.Podium2020_2nd) + (x.Podium2020_3rd)),
			Podiums2021 = _context.NovemberRecap.Where(x => x.PlayerId == bcmPlayer.Id).Sum(x => (x.Podium2021_1st) + (x.Podium2021_2nd) + (x.Podium2021_3rd)),
			Podiums2022 = _context.NovemberRecap.Where(x => x.PlayerId == bcmPlayer.Id).Sum(x => (x.Podium2022_1st) + (x.Podium2022_2nd) + (x.Podium2022_3rd)),
			Podiums2023 = _context.NovemberRecap.Where(x => x.PlayerId == bcmPlayer.Id).Sum(x => (x.Podium2023_1st) + (x.Podium2023_2nd) + (x.Podium2023_3rd)),
		});
	}

	[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	[Route("calc")]
	public IActionResult Calc()
	{
		var players = _bcmService.GetPlayers();
		var leaderboardList = new List<Ranking>();

		_context.NovemberRecap.RemoveRange(_context.NovemberRecap.ToList());
		_context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 11));

		var podium2019_1sts = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 202 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2019).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2019_2nds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 203 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2019).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2019_3rds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 24 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2019).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();

		var podium2020_1sts = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 204 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2020).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2020_2nds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 56 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2020).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2020_3rds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 50 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2020).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();

		var podium2021_1sts = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 134 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2021).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2021_2nds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 156 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2021).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2021_3rds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 21 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2021).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();

		var podium2022_1sts = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 93 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2022).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2022_2nds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 20 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2022).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2022_3rds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 134 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2022).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();

		var podium2023_1sts = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 202 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2023).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2023_2nds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 134 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2023).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();
		var podium2023_3rds = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == 26 && x.CompletionDate != null && x.CompletionDate.Value.Year == 2023).AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).Select(x => x.GameId).ToList();


		foreach (var player in players) {
			var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
			var userRegDate = userWithReg.FirstOrDefault()?.UserRegistrations.FirstOrDefault()?.RegistrationDate;

			if (userRegDate is null) continue;

			var playerCompletions = _context.BcmPlayerGames
																			.Include(x => x.Game)
																			.Where(x => x.PlayerId == player.Id &&
																				x.CompletionDate != null &&
																				x.CompletionDate >= _bcmService.GetContestStartDate() &&
																				x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
																				x.CompletionDate!.Value.Year == 2024 &&
																				x.CompletionDate!.Value.Month == 11)
																			.AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
																			.ToList();

			var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)
																														&& !_context.MonthlyExclusions.Any(y => y.PlayerId == player.Id && y.GameId == x.GameId)).ToList();

			CalcNovemberBonus(player, gamesCompletedThisMonth, podium2019_1sts, podium2019_2nds, podium2019_3rds, podium2020_1sts, podium2020_2nds, podium2020_3rds, podium2021_1sts, podium2021_2nds,
				podium2021_3rds, podium2022_1sts, podium2022_2nds, podium2022_3rds, podium2023_1sts, podium2023_2nds, podium2023_3rds);
		}

		var communityAchieved = CalcNovemberCommunityProgress();

		foreach (var player in players) {
			var stats = _context.NovemberRecap.FirstOrDefault(x => x.PlayerId == player.Id);
			if (stats != null) {
				if (communityAchieved && stats.CommunityBonusQualified) {
					stats.TotalPoints += 1000;
				}

				var ranking = _context.NovemberRecap.OrderByDescending(x => x.TotalPoints).ToList();
				int rank = ranking.FindIndex(x => x.Id == stats.Id);
				stats.Rank = rank + 1;
			}
		}

		_context.SaveChanges();

		return Ok();
	}

	private bool CalcNovemberCommunityProgress()
	{
		var podiums = _context.NovemberRecap
		.Sum(x => (x.Podium2019_1st) + (x.Podium2019_2nd) + (x.Podium2019_3rd) +
			(x.Podium2020_1st) + (x.Podium2020_2nd) + (x.Podium2020_3rd) +
			(x.Podium2021_1st) + (x.Podium2021_2nd) + (x.Podium2021_3rd) +
			(x.Podium2022_1st) + (x.Podium2022_2nd) + (x.Podium2022_3rd) +
			(x.Podium2023_1st) + (x.Podium2023_2nd) + (x.Podium2023_3rd)
		);

		return podiums >= 400;
	}

	private void CalcNovemberBonus(BcmPlayer player, List<BcmPlayerGame> games, List<int?> podium2019_1sts, List<int?> podium2019_2nds, List<int?> podium2019_3rds,
		List<int?> podium2020_1sts, List<int?> podium2020_2nds, List<int?> podium2020_3rds, List<int?> podium2021_1sts, List<int?> podium2021_2nds, List<int?> podium2021_3rds,
		List<int?> podium2022_1sts, List<int?> podium2022_2nds, List<int?> podium2022_3rds, List<int?> podium2023_1sts, List<int?> podium2023_2nds, List<int?> podium2023_3rds)
	{
		var recap = _context.NovemberRecap.FirstOrDefault(x => x.PlayerId == player.Id);
		if (recap is null) {
			recap = new NovRecap { PlayerId = player.Id };
			_context.NovemberRecap.Add(recap);
		}

		_context.SaveChanges();

		var possiblePodiums = games.ToList();

		var player20191sts = new List<BcmPlayerGame>();
		var player20192nds = new List<BcmPlayerGame>();
		var player20193rds = new List<BcmPlayerGame>();
		var player20201sts = new List<BcmPlayerGame>();
		var player20202nds = new List<BcmPlayerGame>();
		var player20203rds = new List<BcmPlayerGame>();
		var player20211sts = new List<BcmPlayerGame>();
		var player20212nds = new List<BcmPlayerGame>();
		var player20213rds = new List<BcmPlayerGame>();
		var player20221sts = new List<BcmPlayerGame>();
		var player20222nds = new List<BcmPlayerGame>();
		var player20223rds = new List<BcmPlayerGame>();
		var player20231sts = new List<BcmPlayerGame>();
		var player20232nds = new List<BcmPlayerGame>();
		var player20233rds = new List<BcmPlayerGame>();

		var totalPoints = 0;
		var podium1stMet = false;
		var podium2ndMet = false;
		var podium3rdMet = false;

		player20191sts = possiblePodiums.Where(x => podium2019_1sts.Any(y => y == x.GameId)).ToList();

		if (player20191sts.Any())
			foreach (var completion in player20191sts) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium1stMet, .6);
				recap.Podium2019_1st += 1;
			}

		player20201sts = possiblePodiums.Where(x => podium2020_1sts.Any(y => y == x.GameId)).ToList();

		if (player20201sts.Any())
			foreach (var completion in player20201sts) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium1stMet, .6);
				recap.Podium2020_1st += 1;
			}

		player20211sts = possiblePodiums.Where(x => podium2021_1sts.Any(y => y == x.GameId)).ToList();

		if (player20211sts.Any())
			foreach (var completion in player20211sts) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium1stMet, .6);
				recap.Podium2021_1st += 1;
			}

		player20221sts = possiblePodiums.Where(x => podium2022_1sts.Any(y => y == x.GameId)).ToList();

		if (player20221sts.Any())
			foreach (var completion in player20221sts) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium1stMet, .6);
				recap.Podium2022_1st += 1;
			}

		player20231sts = possiblePodiums.Where(x => podium2023_1sts.Any(y => y == x.GameId)).ToList();

		if (player20231sts.Any())
			foreach (var completion in player20231sts) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium1stMet, .6);
				recap.Podium2023_1st += 1;
			}

		player20192nds = possiblePodiums.Where(x => podium2019_2nds.Any(y => y == x.GameId)).ToList();

		if (player20192nds.Any())
			foreach (var completion in player20192nds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium2ndMet, .4);
				recap.Podium2019_2nd += 1;
			}

		player20202nds = possiblePodiums.Where(x => podium2020_2nds.Any(y => y == x.GameId)).ToList();

		if (player20202nds.Any())
			foreach (var completion in player20202nds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium2ndMet, .4);
				recap.Podium2020_2nd += 1;
			}

		player20212nds = possiblePodiums.Where(x => podium2021_2nds.Any(y => y == x.GameId)).ToList();

		if (player20212nds.Any())
			foreach (var completion in player20212nds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium2ndMet, .4);
				recap.Podium2021_2nd += 1;
			}

		player20222nds = possiblePodiums.Where(x => podium2022_2nds.Any(y => y == x.GameId)).ToList();

		if (player20222nds.Any())
			foreach (var completion in player20222nds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium2ndMet, .4);
				recap.Podium2022_2nd += 1;
			}

		player20232nds = possiblePodiums.Where(x => podium2023_2nds.Any(y => y == x.GameId)).ToList();

		if (player20232nds.Any())
			foreach (var completion in player20232nds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium2ndMet, .4);
				recap.Podium2023_2nd += 1;
			}

		player20193rds = possiblePodiums.Where(x => podium2019_3rds.Any(y => y == x.GameId)).ToList();

		if (player20193rds.Any())
			foreach (var completion in player20193rds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium3rdMet, .2);
				recap.Podium2019_3rd += 1;
			}

		player20203rds = possiblePodiums.Where(x => podium2020_3rds.Any(y => y == x.GameId)).ToList();

		if (player20203rds.Any())
			foreach (var completion in player20203rds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium3rdMet, .2);
				recap.Podium2020_3rd += 1;
			}

		player20213rds = possiblePodiums.Where(x => podium2021_3rds.Any(y => y == x.GameId)).ToList();

		if (player20213rds.Any())
			foreach (var completion in player20213rds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium3rdMet, .2);
				recap.Podium2021_3rd += 1;
			}

		player20223rds = possiblePodiums.Where(x => podium2022_3rds.Any(y => y == x.GameId)).ToList();

		if (player20223rds.Any())
			foreach (var completion in player20223rds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium3rdMet, .2);
				recap.Podium2022_3rd += 1;
			}

		player20233rds = possiblePodiums.Where(x => podium2023_3rds.Any(y => y == x.GameId)).ToList();

		if (player20233rds.Any())
			foreach (var completion in player20233rds) {
				CalcThings(completion, ref totalPoints, ref possiblePodiums, player, ref podium3rdMet, .2);
				recap.Podium2023_3rd += 1;
			}

		recap.Gamertag = player.User!.Gamertag!;
		recap.Participation = totalPoints > 0;
		recap.CommunityBonusQualified = podium1stMet && podium2ndMet && podium3rdMet;

		var podiumBonus = 0.0;

		if (recap.Podium2019_1st > 0 && recap.Podium2019_2nd > 0 && recap.Podium2019_3rd > 0)
			podiumBonus += .5;

		if (recap.Podium2020_1st > 0 && recap.Podium2020_2nd > 0 && recap.Podium2020_3rd > 0)
			podiumBonus += .5;

		if (recap.Podium2021_1st > 0 && recap.Podium2021_2nd > 0 && recap.Podium2021_3rd > 0)
			podiumBonus += .5;

		if (recap.Podium2022_1st > 0 && recap.Podium2022_2nd > 0 && recap.Podium2022_3rd > 0)
			podiumBonus += .5;

		if (recap.Podium2023_1st > 0 && recap.Podium2023_2nd > 0 && recap.Podium2023_3rd > 0)
			podiumBonus += .5;

		totalPoints += (int)Math.Floor(totalPoints * podiumBonus);

		recap.TotalPoints = totalPoints;

		_context.SaveChanges();
	}

	private void CalcThings(BcmPlayerGame completion, ref int totalPoints, ref List<BcmPlayerGame> possiblePodiums, BcmPlayer player, ref bool podiumMet, double bonusValue)
	{
		var completionValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0;
		var individualBonusPoints = (int)Math.Floor(completionValue * bonusValue);

		completion.BcmPoints = individualBonusPoints;
		totalPoints += individualBonusPoints;

		podiumMet = true;
		possiblePodiums.Remove(completion);

		_context.MonthlyExclusions.Add(new MonthlyExclusion {
			Challenge = 11,
			GameId = completion.GameId,
			PlayerId = player.Id
		});
	}
}
