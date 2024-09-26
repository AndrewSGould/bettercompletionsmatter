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

[Route("/v2/bcm/september")]
[ApiController]
public class SeptemberController : ControllerBase {
	private TavisContext _context;
	private readonly IUserService _userService;
	private readonly IBcmService _bcmService;
	private readonly IStatsService _statsService;

	public SeptemberController(TavisContext context, IUserService userService, IBcmService bcmService, IStatsService statsService)
	{
		_context = context;
		_userService = userService;
		_bcmService = bcmService;
		_statsService = statsService;
	}

	[HttpGet, Route("leaderboard")]
	public async Task<IActionResult> GetSepLeaderboard()
	{
		var recap = await _context.SeptemberRecap.ToListAsync();
		return Ok(recap);
	}

	[Authorize(Roles = "Guest")]
	[HttpGet, Route("monthly")]
	public async Task<IActionResult> SeptemberSummary(string player)
	{
		var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("Player not found with the provided gamertag");

		var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
		if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

		var recap = await _context.SeptemberRecap.FirstOrDefaultAsync(x => x.PlayerId == bcmPlayer.Id);

		var communityStreakers = _context.SeptemberRecap.Sum(x => x.Participation ? 1 : 0);
		var communityStreakCount = _context.SeptemberRecap.Sum(x => x.StreakCount);

		return Ok(new {
			recap.StreakCount,
			recap.Participation,
			recap.TotalPoints,
			communityStreakers,
			communityStreakCount
		});
	}

	[HttpPost, Authorize(Roles = "Admin, Bcm Admin")]
	[Route("calc")]
	public IActionResult Calc()
	{
		var players = _bcmService.GetPlayers().Where(x => x.User.Gamertag.Contains("Chewie"));
		var leaderboardList = new List<Ranking>();

		_context.SeptemberRecap.RemoveRange(_context.SeptemberRecap.ToList());
		_context.MonthlyExclusions.RemoveRange(_context.MonthlyExclusions.Where(x => x.Challenge == 9));

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
																				x.CompletionDate!.Value.Month == 9)
																			.AsEnumerable();
			var test = playerCompletions.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
																				.ToList();

			var gamesCompletedThisMonth = playerCompletions.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId)
																														&& !_context.MonthlyExclusions.Any(y => y.PlayerId == player.Id && y.GameId == x.GameId)).ToList();

			CalcSeptemberBonus(player, gamesCompletedThisMonth);
		}

		var communityAchieved = CalcSeptemberCommunityProgress();

		foreach (var player in players) {
			var stats = _context.SeptemberRecap.FirstOrDefault(x => x.PlayerId == player.Id);
			if (stats != null) {
				if (communityAchieved) stats.TotalPoints += 1000;

				var ranking = _context.SeptemberRecap.OrderByDescending(x => x.TotalPoints).ToList();
				int rank = ranking.FindIndex(x => x.Id == stats.Id);
				stats.Rank = rank + 1;
			}
		}

		//_context.SaveChanges();

		return Ok();
	}

	private bool CalcSeptemberCommunityProgress()
	{
		var streakerCount = _context.SeptemberRecap.Sum(x => x.Participation ? 1 : 0);
		var gameCount = _context.SeptemberRecap.Sum(x => x.StreakCount);

		return streakerCount >= 40 && gameCount >= 150;
	}

	//private void CalcSeptemberBonus(BcmPlayer player, List<BcmPlayerGame> games)
	//{
	//	games = games.OrderBy(x => x.CompletionDate).ToList();

	//	var totalPoints = 0;

	//	var start = StartingGame(games);

	//	if (start == null) {
	//		_context.SeptemberRecap.Add(new SepRecap {
	//			PlayerId = player.Id,
	//			Gamertag = player.User!.Gamertag!,
	//			Participation = false,
	//			TotalPoints = totalPoints,
	//		});

	//		_context.SaveChanges();

	//		return;
	//	}

	//	games = games.OrderBy(x => x.CompletionDate).SkipWhile(x => x != start.Value.Game).ToList();

	//	var pos = start.Value.StreakPosition;
	//	var gameList = new List<BcmPlayerGame>();

	//	for (int i = 0; i < games.Count - 1; i++) {
	//		if (pos == "dev") {
	//			if (MatchesWord(games[i], games[i + 1])) {
	//				pos = "word";
	//				gameList.Add(games[i]);
	//				if (i == games.Count - 2) gameList.Add(games[++i]);
	//				continue;
	//			}
	//			else if (MatchesReleaseYear(games[i], games[i + 1])) {
	//				pos = "year";
	//				gameList.Add(games[i]);
	//				if (i == games.Count - 2) gameList.Add(games[++i]);
	//				continue;
	//			}
	//			break;
	//		}

	//		if (pos == "word") {
	//			if (MatchesReleaseYear(games[i], games[i + 1])) {
	//				pos = "year";
	//				gameList.Add(games[i]);
	//				if (i == games.Count - 2) gameList.Add(games[++i]);
	//				continue;
	//			}
	//			else if (MatchesDeveloper(games[i], games[i + 1])) {
	//				pos = "dev";
	//				gameList.Add(games[i]);
	//				if (i == games.Count - 2) gameList.Add(games[++i]);
	//				continue;
	//			}
	//			break;
	//		}

	//		if (pos == "year") {
	//			if (MatchesDeveloper(games[i], games[i + 1])) {
	//				pos = "dev";
	//				gameList.Add(games[i]);
	//				if (i == games.Count - 2) gameList.Add(games[++i]);
	//				continue;
	//			}
	//			if (MatchesWord(games[i], games[i + 1])) {
	//				pos = "word";
	//				gameList.Add(games[i]);
	//				if (i == games.Count - 2) gameList.Add(games[++i]);
	//				continue;
	//			}
	//			break;
	//		}
	//	}

	//	if (gameList.Count() > 2) {
	//		var streakCount = gameList.Count();

	//		foreach (var completion in gameList) {
	//			var completionValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0;

	//			var bonusValue = .0;

	//			if (streakCount == 3 || streakCount == 4) {
	//				bonusValue = .4;
	//			}

	//			if (streakCount == 5 || streakCount == 6) {
	//				bonusValue = .6;
	//			}

	//			if (streakCount >= 7) {
	//				bonusValue = .8;
	//			}

	//			var individualBonusPoints = (int)Math.Floor(completionValue * bonusValue);
	//			completion.BcmPoints = individualBonusPoints;
	//			totalPoints += individualBonusPoints;

	//			_context.MonthlyExclusions.Add(new MonthlyExclusion {
	//				Challenge = 9,
	//				GameId = completion.GameId,
	//				PlayerId = player.Id
	//			});
	//		}
	//	}

	//	_context.SeptemberRecap.Add(new SepRecap {
	//		PlayerId = player.Id,
	//		Gamertag = player.User!.Gamertag!,
	//		StreakCount = gameList.Count(),
	//		StreakedGames = string.Join(", ", gameList.Select(x => x.Game!.Title)),
	//		Participation = gameList.Count() > 2,
	//		TotalPoints = totalPoints,
	//	});

	//	_context.SaveChanges();
	//}

	//private bool MatchesDeveloper(BcmPlayerGame start, BcmPlayerGame next)
	//{
	//	return start.Game!.Developer == next.Game!.Developer;
	//}


	//private bool MatchesWord(BcmPlayerGame start, BcmPlayerGame next)
	//{
	//	var ignoredWords = new List<string> { "(windows)", "(xbox 360)", "(x|s)", "(xbox one)" };

	//	var startWords = start.Game!.Title.ToLower()
	//			.Split(' ')
	//			.Where(word => !ignoredWords.Contains(word))
	//			.ToArray();

	//	var nextWords = next.Game!.Title.ToLower()
	//			.Split(' ')
	//			.Where(word => !ignoredWords.Contains(word))
	//			.ToArray();

	//	return startWords.Any(word => nextWords.Contains(word));
	//}

	//private bool MatchesReleaseYear(BcmPlayerGame start, BcmPlayerGame next)
	//{
	//	if (start.Game!.ReleaseDate == null) return false;
	//	if (next.Game!.ReleaseDate == null) return false;

	//	return start.Game!.ReleaseDate!.Value.Year == next.Game!.ReleaseDate!.Value.Year;
	//}

	//private (BcmPlayerGame Game, string StreakPosition)? StartingGame(List<BcmPlayerGame> games)
	//{
	//	for (int i = 0; i < games.Count - 1; i++) {
	//		if (MatchesDeveloper(games[i], games[i + 1])) return (games[i], "year");
	//		if (MatchesWord(games[i], games[i + 1])) return (games[i], "dev");
	//		if (MatchesReleaseYear(games[i], games[i + 1])) return (games[i], "word");
	//	}

	//	return null;
	//}

	private void CalcSeptemberBonus(BcmPlayer player, List<BcmPlayerGame> games)
	{
		// Step 1: Group games by completion date
		var groupedGames = games
				.GroupBy(g => g.CompletionDate)
				.OrderBy(g => g.Key) // order by completion date
				.Select(g => g.ToList()) // convert to lists for permutation generation
				.ToList();

		var totalPoints = 0;
		var bestGameOrder = new List<BcmPlayerGame>();

		// Step 2: If all groups have only one game (no shared completion dates), skip permutations
		if (groupedGames.All(g => g.Count == 1)) {
			// No need for permutations, process directly
			bestGameOrder = BuildStreak(games.OrderBy(g => g.CompletionDate).ToList());
		}
		else {
			// Recursive function to evaluate all permutations
			void GeneratePermutations(List<BcmPlayerGame> current, List<List<BcmPlayerGame>> remainingGroups)
			{
				if (remainingGroups.Count == 0) {
					// Rebuild streak for the current game order
					var streakResult = BuildStreak(current);
					if (streakResult.Count > bestGameOrder.Count) {
						bestGameOrder = streakResult; // keep the best order
					}
					return;
				}

				// Generate permutations for the current group
				foreach (var permutation in GetPermutations(remainingGroups[0])) {
					GeneratePermutations(current.Concat(permutation).ToList(), remainingGroups.Skip(1).ToList());
				}
			}

			// Start generating permutations with an empty list as current order
			GeneratePermutations(new List<BcmPlayerGame>(), groupedGames);
		}

		// Now `bestGameOrder` contains the order that produces the longest streak (or original order if no shared dates).
		ApplyStreakLogic(bestGameOrder, player, ref totalPoints);
	}

	// Helper function to build a streak from a game list
	private List<BcmPlayerGame> BuildStreak(List<BcmPlayerGame> games)
	{
		var pos = StartingGame(games)?.StreakPosition ?? "dev";
		var gameList = new List<BcmPlayerGame>();

		for (int i = 0; i < games.Count - 1; i++) {
			if (pos == "dev") {
				if (MatchesWord(games[i], games[i + 1])) {
					pos = "word";
					gameList.Add(games[i]);
					if (i == games.Count - 2) gameList.Add(games[++i]);
					continue;
				}
				else if (MatchesReleaseYear(games[i], games[i + 1])) {
					pos = "year";
					gameList.Add(games[i]);
					if (i == games.Count - 2) gameList.Add(games[++i]);
					continue;
				}

				break;
			}

			if (pos == "word") {
				if (MatchesReleaseYear(games[i], games[i + 1])) {
					pos = "year";
					gameList.Add(games[i]);
					if (i == games.Count - 2) gameList.Add(games[++i]);
					continue;
				}
				else if (MatchesDeveloper(games[i], games[i + 1])) {
					pos = "dev";
					gameList.Add(games[i]);
					if (i == games.Count - 2) gameList.Add(games[++i]);
					continue;
				}

				break;
			}

			if (pos == "year") {
				if (MatchesDeveloper(games[i], games[i + 1])) {
					pos = "dev";
					gameList.Add(games[i]);
					if (i == games.Count - 2) gameList.Add(games[++i]);
					continue;
				}
				if (MatchesWord(games[i], games[i + 1])) {
					pos = "word";
					gameList.Add(games[i]);
					if (i == games.Count - 2) gameList.Add(games[++i]);
					continue;
				}

				break;
			}
		}

		return gameList;
	}

	// Apply the logic after the best permutation of games is determined
	private void ApplyStreakLogic(List<BcmPlayerGame> games, BcmPlayer player, ref int totalPoints)
	{
		if (games.Count > 2) {
			var streakCount = games.Count();

			foreach (var completion in games) {
				var completionValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0;

				var bonusValue = .0;

				if (streakCount == 3 || streakCount == 4) {
					bonusValue = .4;
				}

				if (streakCount == 5 || streakCount == 6) {
					bonusValue = .6;
				}

				if (streakCount >= 7) {
					bonusValue = .8;
				}

				var individualBonusPoints = (int)Math.Floor(completionValue * bonusValue);
				completion.BcmPoints = individualBonusPoints;
				totalPoints += individualBonusPoints;

				_context.MonthlyExclusions.Add(new MonthlyExclusion {
					Challenge = 9,
					GameId = completion.GameId,
					PlayerId = player.Id
				});
			}
		}

		// Update the recap with the final streak and points
		_context.SeptemberRecap.Add(new SepRecap {
			PlayerId = player.Id,
			Gamertag = player.User!.Gamertag!,
			StreakCount = games.Count(),
			StreakedGames = string.Join(", ", games.Select(x => x.Game!.Title)),
			Participation = games.Count() > 2,
			TotalPoints = totalPoints,
		});

		//_context.SaveChanges();
	}

	// Helper function to check if two games have the same developer
	private bool MatchesDeveloper(BcmPlayerGame start, BcmPlayerGame next)
	{
		return start.Game!.Developer == next.Game!.Developer;
	}

	// Helper function to check if two games share a word in their titles
	private bool MatchesWord(BcmPlayerGame start, BcmPlayerGame next)
	{
		var ignoredWords = new List<string> { "(windows)", "(xbox 360)", "(x|s)", "(xbox one)" };

		var startWords = start.Game!.Title.ToLower()
						.Split(' ')
						.Where(word => !ignoredWords.Contains(word))
						.ToArray();

		var nextWords = next.Game!.Title.ToLower()
						.Split(' ')
						.Where(word => !ignoredWords.Contains(word))
						.ToArray();

		return startWords.Any(word => nextWords.Contains(word));
	}

	// Helper function to check if two games were released in the same year
	private bool MatchesReleaseYear(BcmPlayerGame start, BcmPlayerGame next)
	{
		if (start.Game!.ReleaseDate == null) return false;
		if (next.Game!.ReleaseDate == null) return false;

		return start.Game!.ReleaseDate!.Value.Year == next.Game!.ReleaseDate!.Value.Year;
	}

	// Function to find the starting game and streak position
	private (BcmPlayerGame Game, string StreakPosition)? StartingGame(List<BcmPlayerGame> games)
	{
		for (int i = 0; i < games.Count - 1; i++) {
			if (MatchesDeveloper(games[i], games[i + 1])) return (games[i], "year");
			if (MatchesWord(games[i], games[i + 1])) return (games[i], "dev");
			if (MatchesReleaseYear(games[i], games[i + 1])) return (games[i], "word");
		}

		return null;
	}

	// Helper function to generate permutations of a list
	private IEnumerable<List<T>> GetPermutations<T>(List<T> list)
	{
		if (list.Count == 1)
			return new List<List<T>> { list };

		return list.SelectMany((element, index) => GetPermutations(list.Where((_, i) => i != index).ToList())
				.Select(permutation => (new List<T> { element }).Concat(permutation).ToList()));
	}


}
