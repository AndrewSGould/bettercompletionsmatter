namespace TavisApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Models;
using TavisApi.Services;
using TavisApi.V2.Bcm.Models;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase {
	private TavisContext _context;
	private readonly IBcmService _bcmService;
	private readonly IStatsService _statsService;

	public StatsController(TavisContext context, IBcmService bcmService, IStatsService statsService)
	{
		_context = context;
		_bcmService = bcmService;
		_statsService = statsService;
	}

	[HttpGet]
	[Route("getBcmLeaderboardList")]
	public IActionResult BcmLeaderboardList()
	{
		try {
			var players = _bcmService.GetPlayers();

			foreach (var player in players) {
				var bcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id);
			}

			return Ok(players.OrderBy(x => x.BcmStats?.Rank ?? 999));
		}
		catch (Exception ex) {
			return BadRequest(ex);
		}
	}

	[HttpGet, Authorize(Roles = "Guest")]
	[Route("miscSummary")]
	public IActionResult GetMiscSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		var miscStats = _context.BcmMiscStats.FirstOrDefault(x => x.PlayerId == bcmPlayer.Id);
		if (miscStats is null) return Ok();

		var historicalStats = JsonConvert.DeserializeObject<List<BcmHistoricalStats>>(miscStats.HistoricalStats!);

		return Ok(historicalStats);
	}

	[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	[Route("recalcBcmLeaderboard")]
	public async Task<IActionResult> RecalcBcmLeaderboard()
	{
		var players = _bcmService.GetPlayers();

		var leaderboardList = new List<Ranking>();

		foreach (var player in players) {
			var playerBcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id);

			if (playerBcmStats == null) {
				playerBcmStats = new BcmStat();
				_context.BcmStats.Add(playerBcmStats);
			}

			playerBcmStats.PlayerId = player.Id;

			var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
			var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

			var playerCompletions = _context.BcmPlayerGames
																			.Include(x => x.Game)
																			.Where(x => x.PlayerId == player.Id && (
																				x.CompletionDate != null &&
																				x.CompletionDate >= _bcmService.GetContestStartDate() &&
																				x.CompletionDate >= userRegDate!.Value.AddDays(-1)));

			var gamesCompletedThisYear = playerCompletions.ToList();

			var completedGamesCount = gamesCompletedThisYear.Count();
			var ratioOfGames = gamesCompletedThisYear.Select(x => x.Game!.SiteRatio);
			var estimateOfGames = gamesCompletedThisYear.Select(x => x.Game!.FullCompletionEstimate);

			playerBcmStats.Completions = completedGamesCount;
			playerBcmStats.AverageRatio = ratioOfGames.DefaultIfEmpty(0).Average();
			playerBcmStats.HighestRatio = ratioOfGames.DefaultIfEmpty(0).Max();
			playerBcmStats.AverageTimeEstimate = estimateOfGames.DefaultIfEmpty(0).Average();
			playerBcmStats.HighestTimeEstimate = estimateOfGames.DefaultIfEmpty(0).Max();

			double? basePoints = 0.0;
			foreach (var completion in gamesCompletedThisYear) {
				var pointValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game.FullCompletionEstimate);
				if (pointValue != null)
					basePoints += pointValue;
			}

			playerBcmStats.BasePoints = basePoints;
			playerBcmStats.AveragePoints = completedGamesCount != 0 ? basePoints / completedGamesCount : 0;

			var rgscBonus = _statsService.ScoreRgscCompletions(player, gamesCompletedThisYear);
			var oddJobProgress = await _bcmService.GetOddJobChallengeProgress(player.Id);
			var oddJobBonus = oddJobProgress.Count() == 5 ? 1000 : 0;
			var playersChallenges = _context.PlayerYearlyChallenges.Include(x => x.YearlyChallenge)
																.Where(x => x.PlayerId == player.Id && x.Approved);

			var progress = await _bcmService.GetAlphabetChallengeProgress(player.Id);
			var abcChallenge = progress.Count() >= 25 ? 2500 : 0;

			var communityStarBonus = playersChallenges.Where(x => x.YearlyChallenge!.Category == Data.YearlyCategory.CommunityStar && x.Approved).Count() == 20
																	? 5000 : 0;

			var tavisBonus = playersChallenges.Where(x => x.YearlyChallenge!.Category == Data.YearlyCategory.TheTAVIS && x.Approved).Count() == 20
																	? 5000 : 0;

			var retirementBonus = playersChallenges.Where(x => x.YearlyChallenge!.Category == Data.YearlyCategory.RetirementParty && x.Approved).Count() == 10
																	? 2500 : 0;

			// TODO: Add the 750 for anyone who has all participation

			var janBonus = _context.JanRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var febBonus = _context.FebRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var marBonus = _context.MarRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var aprBonus = _context.AprRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var mayBonus = _context.MayRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var junBonus = _context.JunRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var julBonus = _context.JulyRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var sepBonus = _context.SeptemberRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;
			var octBonus = _context.OctoberRecap.FirstOrDefault(x => x.PlayerId == player.Id)?.TotalPoints ?? 0;

			playerBcmStats.BonusPoints = rgscBonus + oddJobBonus + abcChallenge +
																		janBonus + febBonus + marBonus + aprBonus +
																		mayBonus + junBonus + julBonus + sepBonus + octBonus +
																		tavisBonus + communityStarBonus + retirementBonus;
			playerBcmStats.TotalPoints = basePoints + playerBcmStats.BonusPoints;

			leaderboardList.Add(new Ranking {
				PlayerId = player.Id,
				BcmPoints = playerBcmStats.TotalPoints
			});
		}

		_context.SaveChanges();

		// after saving point calculations, lets order the leaderboard and save again for the rankings
		leaderboardList = leaderboardList.OrderByDescending(x => x.BcmPoints).ToList();

		foreach (var player in players) {
			var playerBcmStats = _context.BcmStats.First(x => x.PlayerId == player.Id);
			var previousRanking = playerBcmStats.Rank;
			var newRanking = leaderboardList.FindIndex(x => x.PlayerId == player.Id) + 1;

			playerBcmStats.Rank = newRanking;
			playerBcmStats.RankMovement = previousRanking - newRanking;
		}

		_context.SaveChanges();

		return Ok();
	}

	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("calcMonthlyBonusFeb")]
	//public IActionResult CalcMonthlyBonusFeb()
	//{
	//  var players = _bcmService.GetPlayers();
	//  var leaderboardList = new List<Ranking>();

	//  var test = new List<Game>();

	//  var allFebCompletions = _context.BcmPlayerGames
	//      .Include(x => x.Game)
	//      .Where(x => x.CompletionDate != null
	//          && x.CompletionDate.Value.Year == 2024
	//          && x.CompletionDate.Value.Month == 2)
	//      .ToList()
	//        .Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId))
	//        .GroupBy(x => x.Game)
	//        .Select(g => Tuple.Create(g.Key, g.Count()))
	//        .ToList();

	//  var communityBonusReached = allFebCompletions.Where(x => x.Item2 > 10).Count() >= 3;

	//  _context.FebRecap.RemoveRange(_context.FebRecap.ToList());

	//  foreach (var player in players)
	//  {
	//    var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
	//    var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

	//    var playerCompletions = _context.BcmPlayerGames
	//                                    .Include(x => x.Game)
	//                                    .Where(x => x.PlayerId == player.Id &&
	//                                      x.CompletionDate != null &&
	//                                      x.CompletionDate >= _bcmService.GetContestStartDate() &&
	//                                      x.CompletionDate >= userRegDate!.Value.AddDays(-1) && 
	//                                      x.CompletionDate.Value.Year == 2024 &&
	//                                      x.CompletionDate.Value.Month == 2)
	//                                    .ToList();

	//    var gamesCompletedThisMonth = playerCompletions
	//            .Where(x =>
	//                !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId) ||
	//                !_context.MonthlyExclusions.Any(y => y.PlayerId == player.Id && y.GameId == x.GameId))
	//            .ToList();

	//    _statsService.CalcFebBonus(player, gamesCompletedThisMonth, allFebCompletions!, communityBonusReached);
	//  }

	//  foreach (var player in players)
	//  {
	//    var febStats = _context.FebRecap.FirstOrDefault(x => x.PlayerId == player.Id);
	//    if (febStats != null)
	//    {
	//      var febRanking = _context.FebRecap.OrderByDescending(x => x.TotalPoints).ToList();
	//      int rank = febRanking.FindIndex(x => x.Id == febStats.Id);
	//      febStats.Rank = rank + 1;
	//    }
	//  }

	//  _context.SaveChanges();

	//  return Ok();
	//}

	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("calcMonthlyBonusMar")]
	//public IActionResult CalcMonthlyBonusMar()
	//{
	//  var players = _bcmService.GetPlayers();
	//  var leaderboardList = new List<Ranking>();

	//  var communityBonusReached = _statsService.CalcMarCommunityGoal();

	//  _context.MarRecap.RemoveRange(_context.MarRecap.ToList());
	//  _context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 3));
	//var communityBounties = _statsService.CommunityBounties();

	//foreach (var player in players)
	//  {
	//    var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
	//    var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

	//    var playerCompletions = _context.BcmPlayerGames
	//                                    .Include(x => x.Game)
	//                                    .Where(x => x.PlayerId == player.Id &&
	//                                      x.CompletionDate != null &&
	//                                      x.CompletionDate >= _bcmService.GetContestStartDate() &&
	//                                      x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
	//                                      x.CompletionDate!.Value.Year == 2024 &&
	//                                      x.CompletionDate!.Value.Month == 3).ToList();

	//    var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)).ToList();
	//    _statsService.CalcMarBonus(player, gamesCompletedThisMonth, communityBonusReached, communityBounties);
	//  }

	//  foreach (var player in players)
	//  {
	//    var marStats = _context.MarRecap.FirstOrDefault(x => x.PlayerId == player.Id);
	//    if (marStats != null)
	//    {
	//      var marRanking = _context.MarRecap.OrderByDescending(x => x.TotalPoints).ToList();
	//      int rank = marRanking.FindIndex(x => x.Id == marStats.Id);
	//      marStats.Rank = rank + 1;
	//    }
	//  }

	//  _context.SaveChanges();

	//  return Ok();
	//}

	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("xxxxxxxxxx")]
	//public IActionResult SavePlayerTopGenres()
	//{

	//	foreach (var player in _context.BcmPlayers.Include(x => x.User).ToList())
	//	{
	//		var pgs = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.BcmPlayer == player && x.CompletionDate != null);

	//		var genreStats = _context.Genres
	//			.GroupJoin(
	//					_context.GameGenres,
	//					genre => genre.Id,
	//					gameGenre => gameGenre.GenreId,
	//					(genre, gameGenres) => new
	//					{
	//						GenreId = genre.Id,
	//						GenreName = genre.Name,
	//						GenreCount = gameGenres
	//						.Join(
	//								_context.BcmPlayerGames
	//										.Where(bpg => bpg.PlayerId == player.Id && bpg.CompletionDate != null),
	//								gg => gg.GameId,
	//								bpg => bpg.GameId,
	//								(gg, bpg) => gg // Use gg instead of 1
	//						)
	//						.Count()
	//					}
	//			)
	//			.OrderByDescending(result => result.GenreCount)
	//			.ThenBy(result => result.GenreName)
	//			.Select(x => new
	//			{
	//				Id = x.GenreId,
	//				Name = x.GenreName,
	//				Value = x.GenreCount
	//			})
	//			.ToList()
	//			.Take(5);

	//		var i = 0;

	//		foreach (var genre in genreStats)
	//		{
	//			_context.PlayerTopGenres.Add(new PlayerTopGenre
	//			{
	//				PlayerId = player.Id,
	//				GenreId = genre.Id,
	//				Rank = ++i,
	//			});
	//		}

	//		_context.SaveChanges();
	//	}

	//	return Ok();
	//}

	[HttpGet, Authorize(Roles = "Participant")]
	[Route("getPlayersTopGenres")]
	public IActionResult GetTopGenres(string player)
	{
		var localuser = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("No user found with supplied gamertag");

		var playerId = localuser.BcmPlayer?.Id;
		if (playerId is null) return BadRequest("Could not get Bcm Player");

		return Ok(_context.PlayerTopGenres.Where(x => x.PlayerId == playerId).ToList());
	}

	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("calcMonthlyBonus")]
	//public IActionResult CalcMonthlyBonusApr()
	//{
	//	var players = _bcmService.GetPlayers();
	//	var leaderboardList = new List<Ranking>();

	//	var communityBonus = 1000;

	//	_context.FakeCompletions.RemoveRange(_context.FakeCompletions.ToList());

	//	foreach (var player in players) {
	//		var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
	//		var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

	//		var playerCompletions = _context.BcmPlayerGames
	//																		.Include(x => x.Game)
	//																		.Where(x => x.PlayerId == player.Id &&
	//																			x.CompletionDate != null &&
	//																			x.CompletionDate >= _bcmService.GetContestStartDate() &&
	//																			x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
	//																			x.CompletionDate!.Value.Year == 2024 &&
	//																			x.CompletionDate!.Value.Month == 4).ToList();

	//		var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)).ToList();
	//		_statsService.CalcAprBonus(player, gamesCompletedThisMonth, communityBonus);
	//	}

	//	//foreach (var player in players) {
	//	//	var aprStats = _context.AprRecap.FirstOrDefault(x => x.PlayerId == player.Id);
	//	//	if (aprStats != null) {
	//	//		var aprRanking = _context.AprRecap.OrderByDescending(x => x.TotalPoints).ToList();
	//	//		int rank = aprRanking.FindIndex(x => x.Id == aprStats.Id);
	//	//		aprStats.Rank = rank + 1;
	//	//	}
	//	//}

	//	_context.SaveChanges();

	//	return Ok();
	//}


	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("calcMonthlyBonus2")]
	//public IActionResult CalcMonthlyBonusJun()
	//{
	//	var players = _bcmService.GetPlayers();
	//	var leaderboardList = new List<Ranking>();

	//	var communityBonus = _statsService.CalcJunCommunityGoal();

	//	_context.JunRecap.RemoveRange(_context.JunRecap.ToList());
	//	_context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 6));

	//	foreach (var player in players) {
	//		var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
	//		var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

	//		var playerCompletions = _context.BcmPlayerGames
	//																		.Include(x => x.Game)
	//																		.Where(x => x.PlayerId == player.Id &&
	//																			x.CompletionDate != null &&
	//																			x.CompletionDate >= _bcmService.GetContestStartDate() &&
	//																			x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
	//																			x.CompletionDate!.Value.Year == 2024 &&
	//																			x.CompletionDate!.Value.Month == 6)
	//																		.AsEnumerable()
	//																		.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
	//																		.ToList();

	//		var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)
	//																													&& !_context.MonthlyExclusions.Any(y => y.PlayerId == player.Id && y.GameId == x.GameId)).ToList();

	//		_statsService.CalcJunBonus(player, gamesCompletedThisMonth, communityBonus);
	//	}

	//	foreach (var player in players) {
	//		var junStats = _context.JunRecap.FirstOrDefault(x => x.PlayerId == player.Id);
	//		if (junStats != null) {
	//			var junRanking = _context.JunRecap.OrderByDescending(x => x.TotalPoints).ToList();
	//			int rank = junRanking.FindIndex(x => x.Id == junStats.Id);
	//			junStats.Rank = rank + 1;
	//		}
	//	}

	//	_context.SaveChanges();

	//	return Ok();
	//}

	[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	[Route("calcMonthlyBonus")]
	public IActionResult CalcMonthlyBonusJuly()
	{
		var players = _bcmService.GetPlayers();
		var leaderboardList = new List<Ranking>();

		var communityBonus = _statsService.CalcJulyCommunityProgress() >= 342;

		_context.JulyRecap.RemoveRange(_context.JulyRecap.ToList());
		_context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 7));

		foreach (var player in players) {
			var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
			var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

			var playerCompletions = _context.BcmPlayerGames
																			.Include(x => x.Game)
																			.Where(x => x.PlayerId == player.Id &&
																				x.CompletionDate != null &&
																				x.CompletionDate >= _bcmService.GetContestStartDate() &&
																				x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
																				x.CompletionDate!.Value.Year == 2024 &&
																				x.CompletionDate!.Value.Month == 7)
																			.AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
																			.ToList();

			var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)
																														&& !_context.MonthlyExclusions.Any(y => y.PlayerId == player.Id && y.GameId == x.GameId)).ToList();

			_statsService.CalcJulyBonus(player, gamesCompletedThisMonth, communityBonus);
		}

		foreach (var player in players) {
			var stats = _context.JulyRecap.FirstOrDefault(x => x.PlayerId == player.Id);
			if (stats != null) {
				var ranking = _context.JulyRecap.OrderByDescending(x => x.TotalPoints).ToList();
				int rank = ranking.FindIndex(x => x.Id == stats.Id);
				stats.Rank = rank + 1;
			}
		}

		_context.SaveChanges();

		return Ok();
	}

	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("calcMonthlyBonus2")]
	//public IActionResult CalcMonthlyBonusMay()
	//{
	//	var players = _context.BcmPlayers!.Include(u => u.User).Include(m => m.MayRecap).ToList();
	//	var leaderboardList = new List<Ranking>();

	//	_context.MayRecap.RemoveRange(_context.MayRecap.ToList());
	//	_context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 5));

	//	foreach (var player in players) {
	//		var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
	//		var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

	//		var playerCompletions = _context.BcmPlayerGames
	//																		.Include(x => x.Game)
	//																		.Include(x => x.Game.GameGenres)
	//																		.Where(x => x.PlayerId == player.Id &&
	//																			x.CompletionDate != null &&
	//																			x.CompletionDate >= _bcmService.GetContestStartDate() &&
	//																			x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
	//																			x.CompletionDate!.Value.Year == 2024 &&
	//																			x.CompletionDate!.Value.Month == 5).ToList();

	//		var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)).ToList();
	//		gamesCompletedThisMonth = gamesCompletedThisMonth.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).ToList();
	//		_statsService.CalcMayBonus(player, gamesCompletedThisMonth, false);
	//	}

	//	var floorCount = _context.MayRecap.Sum(x => x.Floors);

	//	if (floorCount >= 150) {
	//		foreach (var recap in _context.MayRecap.ToList()) {
	//			recap.CommunityBonus = recap.Floors > 1 ? true : false;
	//			recap.TotalPoints = recap.Floors > 1 ? recap.TotalPoints + 1000 : recap.TotalPoints;
	//		}
	//	}

	//	_context.SaveChanges();

	//	foreach (var player in players) {
	//		var mayStats = _context.MayRecap.FirstOrDefault(x => x.PlayerId == player.Id);
	//		if (mayStats != null) {
	//			var mayRanking = _context.MayRecap.OrderByDescending(x => x.TotalPoints).ToList();
	//			int rank = mayRanking.FindIndex(x => x.Id == mayStats.Id);
	//			mayStats.Rank = rank + 1;
	//		}
	//	}

	//	_context.SaveChanges();

	//	return Ok();
	//}

	//[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	//[Route("calcMonthlyBonus3")]
	//public IActionResult CalcMonthlyBonus3()
	//{
	//  var players = _bcmService.GetPlayers();
	//  var leaderboardList = new List<Ranking>();

	//  var communityBonusReached = true;

	//  _context.JanRecap.RemoveRange(_context.JanRecap.ToList());

	//  foreach (var player in players)
	//  {
	//    var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
	//    var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

	//    var playerCompletions = _context.BcmPlayerGames
	//                                    .Include(x => x.Game)
	//                                    .Where(x => x.PlayerId == player.Id &&
	//                                      x.CompletionDate != null &&
	//                                      x.CompletionDate >= _bcmService.GetContestStartDate() &&
	//                                      x.CompletionDate >= userRegDate!.Value.AddDays(-1));

	//    var gamesCompletedThisMonth = playerCompletions.Where(x => x.CompletionDate!.Value.Year == 2024 && x.CompletionDate!.Value.Month == 1).ToList();

	//    _statsService.CalcJanBonus(player, gamesCompletedThisMonth, communityBonusReached);
	//  }

	//  foreach (var player in players)
	//  {
	//    var janStats = _context.JanRecap.FirstOrDefault(x => x.PlayerId == player.Id);
	//    if (janStats != null)
	//    {
	//      var janRanking = _context.JanRecap.OrderByDescending(x => x.TotalPoints).ToList();
	//      int rank = janRanking.FindIndex(x => x.Id == janStats.Id);
	//      janStats.Rank = rank + 1;
	//    }
	//  }

	//  _context.SaveChanges();

	//  return Ok();
	//}
}
