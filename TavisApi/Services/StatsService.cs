using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Models;

namespace TavisApi.Services;

public class StatsService : IStatsService {
	private TavisContext _context;
	private readonly IBcmService _bcmService;

	public StatsService(TavisContext context, IBcmService bcmService)
	{
		_context = context;
		_bcmService = bcmService;
	}

	public int ScoreRgscCompletions(BcmPlayer player, List<BcmPlayerGame> completedGames)
	{
		var rgsc = _context.BcmRgsc.Where(x => x.BcmPlayerId == player.Id)
																.OrderByDescending(x => x.Issued)
																.ToList();

		var rgscCompletions = rgsc.Join(completedGames, rgsc => rgsc.GameId,
																pg => pg.GameId, (rgsc, pg) => new { Rgsc = rgsc, PlayerGames = pg })
															.Where(x => x.Rgsc.Rerolled == false)
															.ToList();

		var fullCompletionBonus = rgscCompletions.Count() == 11 ? 1000 : 0;

		return fullCompletionBonus + rgscCompletions.Count() * 100;
	}

	public bool CalcJanCommunityGoal()
	{
		// defaulting to this because this was already scored
		// and we want this to lock in
		return true;

		var janCompletions = _context.BcmPlayerGames
				.Include(x => x.Game)
				.Where(x => x.CompletionDate != null
										&& x.CompletionDate.Value.Year == 2024
										&& x.CompletionDate.Value.Month == 1
										&& x.Game!.ReleaseDate != null
										&& x.Game.ReleaseDate.Value.Year != 2024)
				.ToList();

		var filteredCompletionGroups = janCompletions
				.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
				.GroupBy(x => x.Game!.ReleaseDate!.Value.Year)
				.ToList();

		foreach (var group in filteredCompletionGroups)
			if (group.Count() < 3) return false;

		return true;
	}

	public void CalcJanBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityGoalReached)
	{
		var filteredCompletions = completedGames.Where(x => Queries.FilterGamesForYearlies(x.Game!, x)).ToList();

		var firstMonthOfYear = new DateTime(DateTime.Now.Year, 1, 1);
		filteredCompletions = filteredCompletions.Where(x => x.CompletionDate!.Value.Month == firstMonthOfYear.Month).ToList();

		var bonusPoints = 0;

		bonusPoints += ScoreJanBaseBonus(player, filteredCompletions);

		var hasAllBuckets = HasBucketBonus(filteredCompletions);
		bonusPoints += hasAllBuckets ? 250 : 0;

		var hasCompleted360Game = filteredCompletions.Count(x => x.Platform == Platform.Xbox360) > 0;
		bonusPoints += communityGoalReached && hasCompleted360Game ? 500 : 0;

		_context.SaveChanges();
	}

	private bool HasBucketBonus(List<BcmPlayerGame> completedGames)
	{
		var bucket1 = completedGames.Where(x => x.Game!.ReleaseDate!.Value.Year >= 2005 && x.Game!.ReleaseDate!.Value.Year <= 2009).Count() > 0;
		var bucket2 = completedGames.Where(x => x.Game!.ReleaseDate!.Value.Year >= 2010 && x.Game!.ReleaseDate!.Value.Year <= 2014).Count() > 0;
		var bucket3 = completedGames.Where(x => x.Game!.ReleaseDate!.Value.Year >= 2015 && x.Game!.ReleaseDate!.Value.Year <= 2019).Count() > 0;
		var bucket4 = completedGames.Where(x => x.Game!.ReleaseDate!.Value.Year >= 2020 && x.Game!.ReleaseDate!.Value.Year <= 2023).Count() > 0;

		return bucket1 && bucket2 && bucket3 && bucket4;
	}

	private int ScoreJanBaseBonus(BcmPlayer player, List<BcmPlayerGame> completedGames)
	{
		var gameBonusPoints = 0;

		var bucket1Points = 0;
		var bucket1Comps = 0;
		var bucket2Points = 0;
		var bucket2Comps = 0;
		var bucket3Points = 0;
		var bucket3Comps = 0;
		var bucket4Points = 0;
		var bucket4Comps = 0;

		foreach (var completion in completedGames) {
			var rawPoints = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate);

			if (completion.Game.ReleaseDate!.Value.Year >= 2005 && completion.Game.ReleaseDate!.Value.Year <= 2009 && rawPoints != null) {
				var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .5));
				gameBonusPoints += bonusPoints;
				bucket1Points += bonusPoints;
				bucket1Comps++;
			}

			if (completion.Game.ReleaseDate!.Value.Year >= 2010 && completion.Game.ReleaseDate!.Value.Year <= 2014 && rawPoints != null) {
				var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .25));
				gameBonusPoints += bonusPoints;
				bucket2Points += bonusPoints;
				bucket2Comps++;
			}

			if (completion.Game.ReleaseDate!.Value.Year >= 2015 && completion.Game.ReleaseDate!.Value.Year <= 2019 && rawPoints != null) {
				var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .15));
				gameBonusPoints += bonusPoints;
				bucket3Points += bonusPoints;
				bucket3Comps++;
			}

			if (completion.Game.ReleaseDate!.Value.Year >= 2020 && completion.Game.ReleaseDate!.Value.Year <= 2023 && rawPoints != null) {
				var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .10));
				gameBonusPoints += bonusPoints;
				bucket4Points += bonusPoints;
				bucket4Comps++;
			}

			if (completion.Game.ReleaseDate!.Value.Year <= 2023)
				_context.MonthlyExclusions.Add(new MonthlyExclusion {
					GameId = completion.GameId,
					PlayerId = player.Id,
					Challenge = 1
				});
		}

		var hasCompleted360Game = completedGames.Count(x => x.Platform == Platform.Xbox360) > 0;

		var playerJanStats = _context.JanRecap.Add(new Tavis.Models.JanRecap {
			Gamertag = player.User!.Gamertag!,
			Bucket1Points = bucket1Points,
			Bucket1Comps = bucket1Comps,
			Bucket2Points = bucket2Points,
			Bucket2Comps = bucket2Comps,
			Bucket3Points = bucket3Points,
			Bucket3Comps = bucket3Comps,
			Bucket4Points = bucket4Points,
			Bucket4Comps = bucket4Comps,
			AllBuckets = HasBucketBonus(completedGames),
			CommunityBonus = hasCompleted360Game,
			TotalPoints = gameBonusPoints + (HasBucketBonus(completedGames) ? 250 : 0) + (hasCompleted360Game ? 500 : 0),
			PlayerId = player.Id
		});

		_context.SaveChanges();

		return gameBonusPoints;
	}

	public IEnumerable<BcmPlayerGame> CommunityBounties()
	{
		var allMarCompletions = _context.BcmPlayerGames
				.Include(x => x.Game)
				.Where(x => x.CompletionDate != null
						&& x.CompletionDate.Value.Year == 2024
						&& x.CompletionDate.Value.Month == 3)
				.ToList();

		return allMarCompletions.Where(x => Bounties().Contains(x.Game!)).ToList();
	}

	public void CalcAprBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, int communityBonus)
	{
		var fakeCompletions = new List<BcmPlayerGame>();

		if (player.User?.Gamertag == "eohjay")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1385 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Scarovese3367")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 9464 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "dubdeetwothree")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 4729 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "IronFistofSnuff")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 103 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "smrnov") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 8023 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 4796 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "SwiftSupafly") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 2024 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 237 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "HawkeyeBarry20")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 4592 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "wellingtonbalbo")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 5821 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Wakapeil")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 103 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Christoph 5782")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 5048 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "WildwoodMike")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 3575 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "JimbotUK") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1317 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6982 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "omgeezus")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 9464 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "MrGompers")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 5807 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "meanmachine832") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 4729 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 103 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 3845 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1848 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "Kez001")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 96 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "BAD T0AST")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1385 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "nuttywray") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 3024 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6982 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 5736 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 7027 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "J Battlestar")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1815 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Mephisto4thewin") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6982 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6826 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "DanTheWhale")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1204 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Luke17000")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1541 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Porygon Zzz")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 3748 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Fine Feat") {
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6826 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 102 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 1317 && x.BcmPlayer!.Id == player.Id));
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6982 && x.BcmPlayer!.Id == player.Id));
		}

		if (player.User?.Gamertag == "DudeWithTheFace")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 208 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Matthewh00")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 250 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "WEagleScout")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 103 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Tatersoup19")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 4876 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "ROGUE 1992")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6982 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Messilover449")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 6863 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "ResKR19")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 3835 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "Anti Champ")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 2856 && x.BcmPlayer!.Id == player.Id));

		if (player.User?.Gamertag == "xTMx Voytek")
			fakeCompletions.Add(_context.BcmPlayerGames.Include(x => x.Game).First(x => x.GameId == 4447 && x.BcmPlayer!.Id == player.Id));

		var participated = false;
		var totalPoints = 0;

		foreach (var fakeCompletion in fakeCompletions) {
			_context.FakeCompletions.Add(new FakeCompletion {
				PlayerId = player.Id,
				GameId = fakeCompletion.GameId,
				Title = fakeCompletion.Game.Title,
				SiteRatio = fakeCompletion.Game.SiteRatio,
				FullCompletionEstimate = fakeCompletion.Game.FullCompletionEstimate,
				FakeCompletionDate = fakeCompletion.LastUnlock,
				BonusPoints = (_bcmService.CalcBcmValue(fakeCompletion.Platform, fakeCompletion.Game!.SiteRatio, fakeCompletion.Game!.FullCompletionEstimate) ?? 0) * .8
			});
		}

		_context.SaveChanges();
	}

	public void CalcMayBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonus)
	{
		var topGenres = _context.PlayerTopGenres.Where(x => x.PlayerId == player.Id).ToList();

		var filteredGames = completedGames
			.Where(game => game.Game != null && game.Game.GameGenres != null && game.Game.GameGenres
				.Any(gameGenre => topGenres
					.Any(topGenre => topGenre.GenreId == gameGenre.GenreId)))
						.ToList();

		filteredGames = filteredGames.ToList().OrderBy(x => x.CompletionDate).ToList();
		var results = DoTheThing(filteredGames, topGenres);

		if (results != null) {
			var totalPoints = 0;

			foreach (var towerfloor in results.Games) {
				if (towerfloor.Game.Game.SiteRatio >= 1 && towerfloor.Game.Game.SiteRatio < 2)
					towerfloor.BcmPoints = (int)Math.Floor(towerfloor.BcmPoints * (.10 + (6 * results.FloorCount) / 100.0));
				else if (towerfloor.Game.Game.SiteRatio >= 2 && towerfloor.Game.Game.SiteRatio < 3)
					towerfloor.BcmPoints = (int)Math.Floor(towerfloor.BcmPoints * (.20 + (5 * results.FloorCount) / 100.0));
				else if (towerfloor.Game.Game.SiteRatio >= 3 && towerfloor.Game.Game.SiteRatio < 4)
					towerfloor.BcmPoints = (int)Math.Floor(towerfloor.BcmPoints * (.30 + (4 * results.FloorCount) / 100.0));
				else if (towerfloor.Game.Game.SiteRatio >= 4 && towerfloor.Game.Game.SiteRatio < 6)
					towerfloor.BcmPoints = (int)Math.Floor(towerfloor.BcmPoints * (.40 + (3 * results.FloorCount) / 100.0));
				else if (towerfloor.Game.Game.SiteRatio >= 6 && towerfloor.Game.Game.SiteRatio < 8)
					towerfloor.BcmPoints = (int)Math.Floor(towerfloor.BcmPoints * (.50 + (2 * results.FloorCount) / 100.0));
				else if (towerfloor.Game.Game.SiteRatio >= 8)
					towerfloor.BcmPoints = (int)Math.Floor(towerfloor.BcmPoints * (.60 + (1 * results.FloorCount) / 100.0));

				totalPoints += towerfloor.BcmPoints;

				_context.MonthlyExclusions.Add(new MonthlyExclusion {
					Challenge = 5,
					GameId = towerfloor.Game!.GameId,
					PlayerId = player.Id
				});
			}

			_context.MayRecap.Add(new MayRecap {
				Gamertag = player.User!.Gamertag!,
				Games = results.Games.Select(x => x.Game!.Game!.Title!).ToList(),
				Floors = results.FloorCount,
				HigestRatio = results.Games.Max(x => x.Game?.Game?.SiteRatio) ?? 0,
				Participation = totalPoints > 0,
				TotalPoints = totalPoints,
				PlayerId = player.Id
			});
		}
		else {
			_context.MayRecap.Add(new MayRecap {
				TotalPoints = -1,
				Gamertag = player.User!.Gamertag!,
				PlayerId = player.Id,
			});
		}

		_context.SaveChanges();

		return;
	}

	public GenreResults DoTheThing(List<BcmPlayerGame> filteredGames, List<PlayerTopGenre> topGenres)
	{
		var results = new GenreResults();
		BcmPlayerGame previousGame = null;

		foreach (var game in filteredGames) {
			game.Game!.SiteRatio = game.Platform == Platform.Xbox360 ? game.Game!.SiteRatio + 0.5 : game.Game!.SiteRatio;
		}

		filteredGames = filteredGames.OrderBy(x => x.CompletionDate).ThenBy(x => x.Game!.SiteRatio).ToList();

		foreach (var game in filteredGames) {
			// genre that we picked previously
			var previousGenreTracker = new PlayerTopGenre();

			// genre selected
			var currentGenreTracker = new PlayerTopGenre();
			var availGenres = new List<PlayerTopGenre>();

			var length = game.Game!.FullCompletionEstimate;

			// how many genres we can go
			var steps = GetGenreSteps(length);

			var prevGameGenres = new List<PlayerTopGenre>();
			var genresFromGame = topGenres.Where(x => game.Game.GameGenres.Any(y => y.GenreId == x.GenreId));

			if (genresFromGame.Count() > 1)
				currentGenreTracker = genresFromGame.OrderBy(x => x.Rank).First();
			else
				currentGenreTracker = genresFromGame.First();

			if (previousGame != null) {
				prevGameGenres = topGenres.Where(x => previousGame.Game.GameGenres.Any(y => y.GenreId == x.GenreId)).ToList();

				if (prevGameGenres.Count() > 1)
					previousGenreTracker = prevGameGenres.OrderBy(x => x.Rank).First();
				else
					previousGenreTracker = prevGameGenres.First();
			}

			// get next allowable genres
			if (previousGame == null) {
				availGenres = topGenres;
			}
			else {
				if (steps == 5) {
					availGenres = topGenres;
				}
				else {
					int startRank = previousGenreTracker.Rank;
					int rank = startRank;

					do {
						rank++;
						if (rank > 5) rank = 1;

						availGenres.Add(topGenres.First(x => x.Rank == rank));
					} while (rank != startRank);
				}
			}

			if (availGenres.Any(x => x.GenreId == currentGenreTracker.GenreId)) {
				var ratio = game.Platform == Platform.Xbox360 ? game.Game.SiteRatio - .5 : game.Game.SiteRatio;
				var baseValue = _bcmService.CalcBcmValue(game.Platform, ratio, game.Game.FullCompletionEstimate) ?? 0;

				if (previousGame != null && previousGame.Game != null && game.Game.SiteRatio > previousGame.Game.SiteRatio) {
					results.FloorCount += 1;

					results.Games.Add(new TowerGame {
						Game = game,
						BcmPoints = baseValue
					});

					previousGame = game;
				}

				if (previousGame == null) {
					results.FloorCount += 1;

					results.Games.Add(new TowerGame {
						Game = game,
						BcmPoints = baseValue
					});

					previousGame = game;
				}
			}
		}

		var floorCount = results.Games.Select(x => (int)x.Game.Game.SiteRatio).Distinct().Count();
		results.FloorCount = floorCount;

		foreach (var game in filteredGames) {
			game.Game!.SiteRatio = game.Platform == Platform.Xbox360 ? game.Game!.SiteRatio - 0.5 : game.Game!.SiteRatio;
		}

		if (results.Games.Count() < 2) return new GenreResults();

		return results;
	}

	public class GenreResults {
		public int FloorCount { get; set; }
		public List<TowerGame> Games { get; set; } = new List<TowerGame>();
	}

	public class TowerGame {
		public int BcmPoints { get; set; }
		public BcmPlayerGame? Game { get; set; }
	}

	public bool CalcJunCommunityGoal()
	{
		var highlyRatedGamesCount = _context.BcmPlayerGames.Include(x => x.Game)
																			.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Month == 6 && x.CompletionDate.Value.Year == 2024
																								&& x.Game!.SiteRating >= 4.2 && x.Game!.GamersWithGame >= 2000)
																			.AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
																			.Count();


		return highlyRatedGamesCount >= 69;
	}

	public void CalcJunBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonus)
	{
		var communityQualified = completedGames.Any(x => x.Game!.SiteRating >= 4.2 && x.Game!.GamersWithGame >= 2000);
		var participated = completedGames.Any(x => x.Game!.SiteRating >= 3.75 && x.Game!.GamersWithGame >= 2000);
		var totalPoints = 0;

		foreach (var completion in completedGames.Where(x => x.Game!.GamersWithGame >= 2000)) {
			if (completion.Game!.SiteRating >= 3.75 && completion.Game!.SiteRating < 4) {
				var individualGameBonusPoints = (int)Math.Floor((_bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0) * (.4 + (completion.Game!.SiteRating / 100)) ?? 0);
				completion.BcmPoints = individualGameBonusPoints;
				totalPoints += individualGameBonusPoints;

				_context.MonthlyExclusions.Add(new MonthlyExclusion {
					Challenge = 6,
					GameId = completion.GameId,
					PlayerId = player.Id
				});
			}

			if (completion.Game!.SiteRating >= 4 && completion.Game!.SiteRating < 4.5) {
				var individualGameBonusPoints = (int)Math.Floor((_bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0) * (.6 + (completion.Game!.SiteRating / 100)) ?? 0);
				completion.BcmPoints = individualGameBonusPoints;
				totalPoints += individualGameBonusPoints;

				_context.MonthlyExclusions.Add(new MonthlyExclusion {
					Challenge = 6,
					GameId = completion.GameId,
					PlayerId = player.Id
				});
			}

			if (completion.Game!.SiteRating >= 4.5) {
				var individualGameBonusPoints = (int)Math.Floor((_bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0) * (.8 + (completion.Game!.SiteRating / 100)) ?? 0);
				completion.BcmPoints = individualGameBonusPoints;
				totalPoints += individualGameBonusPoints;

				_context.MonthlyExclusions.Add(new MonthlyExclusion {
					Challenge = 6,
					GameId = completion.GameId,
					PlayerId = player.Id
				});
			}
		}

		_context.JunRecap.Add(new JunRecap {
			PlayerId = player.Id,
			Gamertag = player.User!.Gamertag!,
			CommunityBonus = communityQualified && communityBonus,
			Participation = participated,
			TotalPoints = totalPoints,
			QualifiedGames = completedGames.Count()
		});

		_context.SaveChanges();
	}


	public bool CalcJulyCommunityGoal()
	{
		var highlyRatedGamesCount = _context.BcmPlayerGames.Include(x => x.Game)
																			.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Month == 7 && x.CompletionDate.Value.Year == 2024)
																			.AsEnumerable()
																			.Where(x => Queries.FilterGamesForYearlies(x.Game!, x))
																			.SelectMany(x => x.Game!.Title!.ToLower().ToCharArray())
																			.Count(c => c == 't');


		return highlyRatedGamesCount >= 69;
	}
	public void CalcJulyBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonus)
	{
		var qualifiedCompletions = completedGames.Where(x => x.Game != null
																										&& x.Game.FeatureList != null
																										&& x.Game.Title != null
																										&& x.Game.FeatureList.IdAtXbox
																										&& x.Game.GamersCompleted <= 1773
																										&& x.Game.Title.ToLower().Contains("t"));

		int totalTCount = qualifiedCompletions
												.SelectMany(x => x.Game!.Title!.ToLower().ToCharArray())
												.Count(c => c == 't');

		var communityQualified = totalTCount >= 5;
		var participated = qualifiedCompletions.Count() > 0;

		var totalPoints = 0;

		foreach (var completion in qualifiedCompletions) {
			var completionValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate) ?? 0;
			var tCount = completion.Game?.Title?.ToLower().Count(x => x == 't') ?? 0;

			if (completion.Game.GamersCompleted <= 1773 && completion.Game.GamersCompleted > 500) {
				var individualBonusPoints = (int)Math.Floor(completionValue * (.40 + (tCount / 100.0)));
				completion.BcmPoints = individualBonusPoints;
				totalPoints += individualBonusPoints;
			}

			if (completion.Game.GamersCompleted <= 500 && completion.Game.GamersCompleted > 100) {
				var individualBonusPoints = (int)Math.Floor(completionValue * (.60 + (tCount / 100.0)));
				completion.BcmPoints = individualBonusPoints;
				totalPoints += individualBonusPoints;
			}

			if (completion.Game.GamersCompleted <= 100) {
				var individualBonusPoints = (int)Math.Floor(completionValue * (.80 + (tCount / 100.0)));
				completion.BcmPoints = individualBonusPoints;
				totalPoints += individualBonusPoints;
			}

			_context.MonthlyExclusions.Add(new MonthlyExclusion {
				Challenge = 7,
				GameId = completion.GameId,
				PlayerId = player.Id
			});
		}

		_context.JulyRecap.Add(new JulyRecap {
			PlayerId = player.Id,
			Gamertag = player.User!.Gamertag!,
			TeaCount = totalTCount,
			CommunityBonus = communityQualified && communityBonus,
			Participation = participated,
			TotalPoints = totalPoints,
		});

		_context.SaveChanges();
	}

	private int GetGenreSteps(double? estimate)
	{
		if (estimate is null) return 0;

		if (estimate <= 10) return 1;
		if (estimate >= 10.5 && estimate <= 20) return 2;
		if (estimate >= 20.5 && estimate <= 30) return 3;
		if (estimate >= 30.5 && estimate <= 50) return 4;
		if (estimate >= 50.5) return 5;

		return 0;
	}

	private int GetNextGenre(PlayerTopGenre currentGenreTracker, int steps)
	{
		var estStep = currentGenreTracker.Rank + steps;

		if (estStep > 5)
			estStep = estStep - 5;

		return estStep;
	}

	public void CalcMarBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonusReached, IEnumerable<BcmPlayerGame> communityBounties)
	{
		var completedBounties = completedGames.Where(x => Bounties().Contains(x.Game!));
		double bonusPoints = 0;
		double individualGameBonusPoints = 0;
		var bestBounty = (new BcmPlayerGame(), 0.0);
		var currentBounty = (new BcmPlayerGame(), 0.0);

		foreach (var bounty in completedBounties) {
			var bountyProgress = communityBounties.Where(x => x.Game!.Id == bounty.GameId).Count();
			if (bountyProgress == 1) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 5;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 2) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 3;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 3) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 2;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 4) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 1;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 5) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .95;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 6) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .90;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 7) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .85;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 8) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .80;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 9) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .75;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress == 10) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .65;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}
			else if (bountyProgress > 10) {
				individualGameBonusPoints = (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .60;
				bonusPoints += individualGameBonusPoints;
				currentBounty.Item1 = bounty;
				currentBounty.Item2 = individualGameBonusPoints;
			}

			if (bestBounty.Item2 == 0 || bestBounty.Item2 < currentBounty.Item2)
				bestBounty = currentBounty;

			var game = _context.BcmPlayerGames.FirstOrDefault(x => x.PlayerId == player.Id && x.GameId == currentBounty.Item1.GameId);
			if (game != null)
				game.BcmPoints = individualGameBonusPoints;

			_context.MonthlyExclusions.Add(new MonthlyExclusion {
				Challenge = 3,
				GameId = currentBounty.Item1.GameId,
				PlayerId = player.Id
			});
		}

		var playerBountiesCompleted = completedBounties.Count();
		var playerMetCommunityBonus = communityBonusReached && playerBountiesCompleted > 0;
		var claimedBounties = completedBounties.Count() == 0 ? "" : completedBounties.Count() == 1 ? completedBounties.First().Game!.Title : string.Join(", ", completedBounties.Select(x => x.Game!.Title));

		var playerMarStats = _context.MarRecap.Add(new Tavis.Models.MarRecap {
			Gamertag = player.User!.Gamertag!,
			BestBounty = bestBounty.Item1.Game?.Title ?? "",
			BountyCount = playerBountiesCompleted,
			BountiesClaimed = claimedBounties ?? "",
			Participation = playerBountiesCompleted > 0,
			CommunityBonus = playerMetCommunityBonus,
			TotalPoints = bonusPoints + (playerMetCommunityBonus ? 500 : 0),
			PlayerId = player.Id
		});

		_context.SaveChanges();
	}

	public bool CalcMarCommunityGoal()
	{
		var allMarCompletions = _context.BcmPlayerGames
			.Include(x => x.Game)
			.Where(x => x.CompletionDate != null
					&& x.CompletionDate.Value.Year == 2024
					&& x.CompletionDate.Value.Month == 3
					&& Bounties().Any(y => y == x.Game))
			.ToList()
			.Where(x => !BcmRule.UpdateExclusions.Any(y => y.Id == x.GameId))
			.GroupBy(x => x.Game)
			.Select(g => Tuple.Create(g.Key, g.Count()))
			.ToList();

		if (allMarCompletions.Count() < 1) return false;

		foreach (var game in allMarCompletions) {
			if (game.Item2 < 2) return false;
		}

		return true;
	}

	public int CalcAprCommunityGoal()
	{
		//var pdugames = _context.BcmPlayerGames.Include(x => x.Game)
		//                                    .Where(x => x.CompletionDate != null && x.CompletionDate.Value.Month == 4 && x.CompletionDate.Value.Year == 2024 &&
		//                                            (x.Game != null && (x.Game.ServerClosure != null || x.Game.Unobtainables)));

		//var count = pdugames.Count();

		//return count * 5 >= 1000 ? 1000 : count * 5;

		return 1000;
	}

	public void CalcFebBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, List<Tuple<Game, int>> allFebCompletions, bool communityBonusReached)
	{
		var biComps = 0;
		var triComps = 0;
		var quadComps = 0;
		var quintComps = 0;
		var sexComps = 0;
		var sepComps = 0;
		var octComps = 0;
		var decComps = 0;
		var undeComps = 0;
		var duodeComps = 0;

		double biPoints = 0;
		double triPoints = 0;
		double quadPoints = 0;
		double quintPoints = 0;
		double sexPoints = 0;
		double sepPoints = 0;
		double octPoints = 0;
		double decPoints = 0;
		double undePoints = 0;
		double duodePoints = 0;

		foreach (var completion in completedGames) {
			var completionCollection = allFebCompletions.Where(x => Queries.FilterGamesForYearlies(x.Item1, completion)).FirstOrDefault(x => x.Item1.Id == completion.GameId);

			if (completionCollection == null) continue;
			if (completionCollection.Item2 < 2) continue;

			var baseGamePoints = _bcmService.CalcBcmValue(completion.Platform, completionCollection!.Item1.SiteRatio, completionCollection!.Item1.FullCompletionEstimate);

			if (completionCollection!.Item2 > 10) {
				duodeComps++;
				duodePoints += (baseGamePoints ?? 0) * 1;
			}

			if (completionCollection!.Item2 == 10) {
				undeComps++;
				undePoints += (baseGamePoints ?? 0) * .9;
			}

			if (completionCollection!.Item2 == 9) {
				decComps++;
				decPoints += (baseGamePoints ?? 0) * .8;
			}

			if (completionCollection!.Item2 == 8) {
				octComps++;
				octPoints += (baseGamePoints ?? 0) * .7;
			}

			if (completionCollection!.Item2 == 7) {
				sepComps++;
				sepPoints += (baseGamePoints ?? 0) * .6;
			}

			if (completionCollection!.Item2 == 6) {
				sexComps++;
				sexPoints += (baseGamePoints ?? 0) * .5;
			}

			if (completionCollection!.Item2 == 5) {
				quintComps++;
				quintPoints += (baseGamePoints ?? 0) * .4;
			}

			if (completionCollection!.Item2 == 4) {
				quadComps++;
				quadPoints += (baseGamePoints ?? 0) * .3;
			}

			if (completionCollection!.Item2 == 3) {
				triComps++;
				triPoints += (baseGamePoints ?? 0) * .2;
			}

			if (completionCollection!.Item2 == 2) {
				biComps++;
				biPoints += (baseGamePoints ?? 0) * .1;
			}

			//_context.MonthlyExclusions.Add(new MonthlyExclusion
			//{
			//  Challenge = 2,
			//  GameId = completion.GameId,
			//  PlayerId = player.Id
			//});
		}

		var playerMetCommunityBonus = communityBonusReached && (triComps > 0 || quadComps > 0 || quintComps > 0 || sexComps > 0 || sepComps > 0 || octComps > 0 || decComps > 0 || undeComps > 0 || duodeComps > 0);

		var playerFebStats = _context.FebRecap.Add(new Tavis.Models.FebRecap {
			Gamertag = player.User!.Gamertag!,
			BiCompletion = biComps,
			BiPoints = biPoints,
			TriCompletion = triComps,
			TriPoints = triPoints,
			QuadCompletion = quadComps,
			QuadPoints = quadPoints,
			QuintCompletion = quintComps,
			QuintPoints = quintPoints,
			SexCompletion = sexComps,
			SexPoints = sexPoints,
			SepCompletion = sepComps,
			SepPoints = sepPoints,
			OctCompletion = octComps,
			OctPoints = octPoints,
			DecCompletion = decComps,
			DecPoints = decPoints,
			UndeCompletion = undeComps,
			UndePoints = undePoints,
			DuodeCompletion = duodeComps,
			DuodePoints = duodePoints,
			Participation = biComps > 0 || triComps > 0 || quadComps > 0 || quintComps > 0 || sexComps > 0 || sepComps > 0 || octComps > 0 || decComps > 0 || undeComps > 0 || duodeComps > 0,
			CommunityBonus = playerMetCommunityBonus,
			TotalPoints = biPoints + triPoints + quadPoints + quintPoints + sexPoints + sepPoints + octPoints + decPoints + undePoints + duodePoints + (playerMetCommunityBonus ? 500 : 0),
			PlayerId = player.Id
		});

		_context.SaveChanges();
	}

	private bool FebCommunityGoalReached()
	{
		return false;
	}

	public List<Game> Bounties()
	{
		var bountyIds = new HashSet<int> { 3093, 229, 85, 246, 3173, 334, 3122, 1489, 2144, 3147, 4363, 1507, 344, 2211, 3255, 364, 238, 3113, 3241, 3228, 3059, 3128, 3174, 305, 298 };
		return _context.Games.Where(x => bountyIds.Contains(x.Id)).ToList();
	}
}
