using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.Controllers;

[Route("/v2/bcm/october")]
[ApiController]
public class OctoberController : ControllerBase {
	private TavisContext _context;
	private readonly IUserService _userService;
	private readonly IBcmService _bcmService;
	private readonly IStatsService _statsService;

	public OctoberController(TavisContext context, IUserService userService, IBcmService bcmService, IStatsService statsService)
	{
		_context = context;
		_userService = userService;
		_bcmService = bcmService;
		_statsService = statsService;
	}

	[HttpGet, Route("leaderboard")]
	public async Task<IActionResult> GetOctLeaderboard()
	{
		var recap = await _context.OctoberRecap.ToListAsync();
		return Ok(recap);
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly")]
	public async Task<IActionResult> OctoberSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		var recap = await _context.OctoberRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id);

		var crimsonCurse = recap.CrimsonCurseRitual ? 1 : 0;
		var dreadCurse = recap.DreadRitual ? 1 : 0;
		var mark1Curse = recap.MarkOfTheBeast1Ritual ? 1 : 0;
		var mark2Curse = recap.MarkOfTheBeast2Ritual ? 1 : 0;
		var mark3Curse = recap.MarkOfTheBeast3Ritual ? 1 : 0;

		return Ok(new {
			recap.Participation,
			CurseCount = crimsonCurse + dreadCurse + mark1Curse + mark2Curse + mark3Curse,
			recap.BoneCount,
			CommunityBoneCount = _context.OctoberRecap.Sum(x => x.BoneCount),
			recap.TotalPoints,
		});
	}

	[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	[Route("calc")]
	public IActionResult Calc()
	{
		var players = _bcmService.GetPlayers();
		var leaderboardList = new List<Ranking>();

		_context.OctoberRecap.RemoveRange(_context.OctoberRecap.ToList());
		_context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 10));

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
																				x.CompletionDate!.Value.Month == 10)
																			.AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
																			.ToList();

			var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)
																														&& !_context.MonthlyExclusions.Any(y => y.PlayerId == player.Id && y.GameId == x.GameId)).ToList();

			CalcOctoberBonus(player, gamesCompletedThisMonth);
		}

		var communityAchieved = CalcOctoberCommunityProgress();

		foreach (var player in players) {
			var stats = _context.OctoberRecap.FirstOrDefault(x => x.PlayerId == player.Id);
			if (stats != null) {
				if (communityAchieved && stats.TotalPoints > 0) {
					stats.TotalPoints += 1000;
					stats.CommunityBonus = 1000;
				}

				var ranking = _context.OctoberRecap.OrderByDescending(x => x.TotalPoints).ToList();
				int rank = ranking.FindIndex(x => x.Id == stats.Id);
				stats.Rank = rank + 1;
			}
		}

		_context.SaveChanges();

		return Ok();
	}

	private bool CalcOctoberCommunityProgress()
	{
		return _context.OctoberRecap.Sum(x => x.BoneCount) >= 300;
	}

	private void CalcOctoberBonus(BcmPlayer player, List<BcmPlayerGame> games)
	{
		games = games.Where(x => x.Game!.InstallSize >= 1000).ToList();

		var totalPoints = 0;
		bool hasCrimsonCurse = false;
		bool hasDreadCurse = false;
		bool hasMark1Curse = false;
		bool hasMark2Curse = false;
		bool hasMark3Curse = false;

		foreach (var completion in games) {
			hasCrimsonCurse = hasCrimsonCurse ? true : EvalCrimsonCurse(completion.Game!.Title);
			hasDreadCurse = hasDreadCurse ? true : EvalDreadCurse(completion.StartedDate);
			hasMark1Curse = hasMark1Curse ? true : EvalMark1Curse(completion.Game!.AchievementCount);
			hasMark2Curse = hasMark2Curse ? true : EvalMark2Curse(completion.Game!.ReleaseDate);
			hasMark3Curse = hasMark3Curse ? true : EvalMark3Curse(completion.Game!.FullCompletionEstimate);
		}

		foreach (var completion in games) {
			var completionValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0;

			var bonusValue = .0;

			if (completion.Game!.InstallSize >= 1000 && completion.Game!.InstallSize < 4999)
				bonusValue = .20;
			else if (completion.Game!.InstallSize >= 5000 && completion.Game!.InstallSize < 9999)
				bonusValue = .40;
			else if (completion.Game!.InstallSize >= 10000)
				bonusValue = .60;

			bonusValue += hasCrimsonCurse ? .05 : .0;
			bonusValue += hasDreadCurse ? .05 : .0;
			bonusValue += hasMark1Curse ? .05 : .0;
			bonusValue += hasMark2Curse ? .05 : .0;
			bonusValue += hasMark3Curse ? .05 : .0;

			var individualBonusPoints = (int)Math.Floor(completionValue * bonusValue);
			completion.BcmPoints = individualBonusPoints;
			totalPoints += individualBonusPoints;

			_context.MonthlyExclusions.Add(new MonthlyExclusion {
				Challenge = 10,
				GameId = completion.GameId,
				PlayerId = player.Id
			});
		}

		_context.OctoberRecap.Add(new OctRecap {
			PlayerId = player.Id,
			Gamertag = player.User!.Gamertag!,
			BoneCount = games.Count(),
			CrimsonCurseRitual = hasCrimsonCurse,
			DreadRitual = hasDreadCurse,
			MarkOfTheBeast1Ritual = hasMark1Curse,
			MarkOfTheBeast2Ritual = hasMark2Curse,
			MarkOfTheBeast3Ritual = hasMark3Curse,
			Participation = games.Count() >= 1,
			TotalPoints = totalPoints,
		});

		_context.SaveChanges();
	}

	private bool EvalCrimsonCurse(string? gameTitle)
	{
		if (gameTitle == null) return false;

		var ignoredWords = new List<string> { "(windows)", "(xbox 360)", "(x|s)", "(xbox one)" };

		gameTitle = gameTitle.ToLower();
		foreach (var ignoredWord in ignoredWords) {
			if (gameTitle.Contains(ignoredWord)) {
				gameTitle = gameTitle.Replace(ignoredWord, "");
			}
		}

		var regex = new Regex("r\\s*e\\s*d");

		return regex.IsMatch(gameTitle);
	}

	private bool EvalDreadCurse(DateTime? startedDate)
	{
		if (startedDate == null) return false;
		return startedDate.Value.Year <= 2023;
	}

	private bool EvalMark1Curse(int? achCount)
	{
		if (achCount == null) return false;
		return achCount.ToString().Contains("6");
	}

	private bool EvalMark2Curse(DateTime? releaseDate)
	{
		if (releaseDate == null) return false;
		return releaseDate.Value.Day.ToString().Contains("6") || releaseDate.Value.Year.ToString().Contains("6");
	}

	private bool EvalMark3Curse(double? completionEstimate)
	{
		if (completionEstimate == null) return false;
		return completionEstimate.ToString().Contains("6");
	}
}
