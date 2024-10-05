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
		var players = _bcmService.GetPlayers();
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
																			.AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
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

		_context.SaveChanges();

		return Ok();
	}

	private bool CalcSeptemberCommunityProgress()
	{
		var streakerCount = _context.SeptemberRecap.Sum(x => x.Participation ? 1 : 0);
		var gameCount = _context.SeptemberRecap.Sum(x => x.StreakCount);

		return streakerCount >= 40 && gameCount >= 150;
	}
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

		// Step 2: Identify groups that need permutations (more than one game in a group)
		var fixedGroups = groupedGames.Where(g => g.Count == 1).ToList(); // Groups with only 1 game
		var permutableGroups = groupedGames.Where(g => g.Count > 1).ToList(); // Groups with more than 1 game

		// Recursive function to evaluate all permutations
		void GeneratePermutations(List<BcmPlayerGame> current, List<List<BcmPlayerGame>> remainingPermutableGroups)
		{
			if (remainingPermutableGroups.Count == 0) {
				// Add the fixed groups in their order after permutable ones are placed
				var fullOrder = current.Concat(fixedGroups.SelectMany(g => g)).OrderBy(x => x.CompletionDate).ToList();

				// Rebuild streak for the current game order
				var streakResult = BuildStreak(fullOrder);
				if (streakResult.Count > bestGameOrder.Count) {
					bestGameOrder = streakResult; // keep the best order
				}
				return;
			}

			// Generate permutations for the current permutable group
			foreach (var gamePermutation in GetPermutations(remainingPermutableGroups[0])) {
				// Continue generating permutations with the current permutation concatenated to the list
				GeneratePermutations(current.Concat(gamePermutation).ToList(), remainingPermutableGroups.Skip(1).ToList());
			}
		}

		// Start generating permutations with an empty list as the current order, only permutable groups are considered for permutations
		GeneratePermutations(new List<BcmPlayerGame>(), permutableGroups);

		// Now `bestGameOrder` contains the order that produces the longest streak
		ApplyStreakLogic(bestGameOrder, player, ref totalPoints);
	}

	// Helper function to generate all permutations of a list
	private IEnumerable<List<BcmPlayerGame>> GetPermutations(List<BcmPlayerGame> list)
	{
		if (list.Count == 1) {
			yield return list;
		}
		else {
			foreach (var game in list) {
				var remaining = list.Where(g => !g.Equals(game)).ToList();
				foreach (var permutation in GetPermutations(remaining)) {
					yield return new List<BcmPlayerGame> { game }.Concat(permutation).ToList();
				}
			}
		}
	}

	// Helper function to build a streak from a game list
	private List<BcmPlayerGame> BuildStreak(List<BcmPlayerGame> games)
	{
		var pos = StartingGame(games)?.StreakPosition ?? "dev";
		var gameList = new List<BcmPlayerGame>();
		BcmPlayerGame? gameToStreakWith = null;

		for (int i = 0; i < games.Count - 1; i++) {
			gameToStreakWith = gameToStreakWith ?? games[i];

			if (pos == "dev") {
				if (MatchesWord(gameToStreakWith, games[i + 1])) {
					pos = "word";
					gameList.Add(gameToStreakWith);
					gameList.Add(games[i + 1]);
					gameToStreakWith = null;
					continue;
				}
				else if (MatchesReleaseYear(gameToStreakWith, games[i + 1])) {
					pos = "year";
					gameList.Add(gameToStreakWith);
					gameList.Add(games[i + 1]);
					gameToStreakWith = null;
					continue;
				}
			}

			if (pos == "word") {
				if (MatchesReleaseYear(gameToStreakWith, games[i + 1])) {
					pos = "year";
					gameList.Add(gameToStreakWith);
					gameList.Add(games[i + 1]);
					gameToStreakWith = null;
					continue;
				}
				else if (MatchesDeveloper(gameToStreakWith, games[i + 1])) {
					pos = "dev";
					gameList.Add(gameToStreakWith);
					gameList.Add(games[i + 1]);
					gameToStreakWith = null;
					continue;
				}
			}

			if (pos == "year") {
				if (MatchesDeveloper(gameToStreakWith, games[i + 1])) {
					pos = "dev";
					gameList.Add(gameToStreakWith);
					gameList.Add(games[i + 1]);
					gameToStreakWith = null;
					continue;
				}
				if (MatchesWord(gameToStreakWith, games[i + 1])) {
					pos = "word";
					gameList.Add(gameToStreakWith);
					gameList.Add(games[i + 1]);
					gameToStreakWith = null;
					continue;
				}
			}
		}

		return gameList.Distinct().ToList();
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

		_context.SaveChanges();
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
