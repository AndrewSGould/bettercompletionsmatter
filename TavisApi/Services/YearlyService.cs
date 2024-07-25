using Microsoft.EntityFrameworkCore;
using TavisApi.Context;
using TavisApi.V2.Bcm.Models;
using TavisApi.V2.TrueAchievements.Models;

namespace TavisApi.Services;

public class YearlyService : IYearlyService {
	private TavisContext _context;
	private readonly IBcmService _bcmService;

	public YearlyService(TavisContext context, IBcmService bcmService)
	{
		_context = context;
		_bcmService = bcmService;
	}

	public class YearlyOption {
		public string? Title { get; set; }
		public bool Disabled { get; set; }
		public int? Value { get; set; }
	}

	public List<YearlyOption> EvalYearlyOptions(BcmPlayer player, int yearlyId, List<BcmPlayerGame> games, bool isCompletions)
	{
		var validYearlies = new List<BcmPlayerGame>();
		var yearlyOptions = new List<YearlyOption>();

		if (yearlyId == 23)
			validYearlies = ConnoisseurYearly(games);

		if (yearlyId == 24)
			validYearlies = HipsterYearly(games);

		if (yearlyId == 25)
			validYearlies = CriticYearly(games);

		if (yearlyId == 26)
			validYearlies = SocialiteYearly(player, games);

		if (yearlyId == 28)
			validYearlies = ProfessionalYearly(player, games);

		//if (yearlyId == 29)
		//  validYearlies = ContrarianYearly(games);

		if (yearlyId == 30)
			validYearlies = PioneerYearly(games);

		if (yearlyId == 31)
			validYearlies = ConformistYearly(games);

		if (yearlyId == 32)
			validYearlies = MinimalistYearly(games);

		if (yearlyId == 33)
			validYearlies = HedonistYearly(games);

		if (yearlyId == 34)
			validYearlies = TradYearly(games);

		if (yearlyId == 35)
			validYearlies = ArcadeTradYearly(games);

		if (yearlyId == 36)
			validYearlies = OrthographerYearly(player, games);

		if (yearlyId == 37)
			validYearlies = CalProjectYearly(player, games);

		if (yearlyId == 38)
			validYearlies = GenreProjYearly(games);

		if (yearlyId == 40)
			validYearlies = RaidBossYearly(games);

		foreach (var completion in validYearlies) {
			yearlyOptions.Add(new YearlyOption {
				Title = completion.Game!.Title,
				Disabled = !isCompletions,
				Value = completion.Game!.Id,
			});
		}


		return yearlyOptions;
	}

	private List<BcmPlayerGame> ConnoisseurYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.GamersWithGame <= 5000 && x.Game!.SiteRating >= 4).ToList();
	}

	private List<BcmPlayerGame> HipsterYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.GamersWithGame <= 1000).ToList();
	}

	private List<BcmPlayerGame> CriticYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.SiteRating >= 4.25).ToList();
	}

	private List<BcmPlayerGame> SocialiteYearly(BcmPlayer player, List<BcmPlayerGame> gamesToEval)
	{
		var threeOrMoreCompletions = _context.BcmPlayerGames
				.Where(x => x.Game!.TrueAchievementId != 0 && x.PlayerId != player.Id && x.CompletionDate != null && x.CompletionDate.Value.Year == 2024)
				.GroupBy(x => x.GameId)
				.Where(group => group.Count() >= 3)
				.Select(group => group.Key);

		return gamesToEval
				.Where(game => threeOrMoreCompletions.Contains(game.GameId))
				.ToList();
	}

	private List<BcmPlayerGame> ProfessionalYearly(BcmPlayer player, List<BcmPlayerGame> gamesToEval)
	{
		var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.BcmPlayer!.Id == player.Id && x.UserRegistrations.Any(x => x.RegistrationId == 1));
		var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

		var completedGames = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.PlayerId == player.Id && x.Game!.TrueAchievementId != 0 && x.CompletionDate != null);
		var toTake = (int)Math.Ceiling(completedGames.Count() * 0.1);
		var top10percent = completedGames.OrderByDescending(x => x.Game!.SiteRatio);
		var top10percentThreshold = top10percent.Take(toTake);
		var threshold = top10percentThreshold.Last().Game!.SiteRatio;

		return gamesToEval
							.Where(x => x.Game!.SiteRatio >= threshold)
							.ToList();
	}

	//private List<BcmPlayerGame> ContrarianYearly(List<BcmPlayerGame> gamesToEval)
	//{
	//  // TODO: We're not saving users ratings?
	//}

	private List<BcmPlayerGame> PioneerYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.GamersCompleted <= 100).ToList();
	}

	private List<BcmPlayerGame> ConformistYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.GamersWithGame >= 7500).ToList();
	}

	private List<BcmPlayerGame> MinimalistYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.InstallSize <= 200).ToList();
	}

	private List<BcmPlayerGame> HedonistYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.InstallSize >= 30000).ToList();
	}

	private List<BcmPlayerGame> CalProjectYearly(BcmPlayer player, List<BcmPlayerGame> gamesToEval)
	{
		var startYear = _context.BcmPlayerGames.OrderBy(x => x.StartedDate).First(x => x.BcmPlayer!.Id == player.Id && x.StartedDate != null).StartedDate!.Value.Year;
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.ReleaseDate != null && x.Game!.ReleaseDate.Value.Year == startYear).ToList();
	}

	private List<BcmPlayerGame> TradYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.AchievementCount == 50).ToList();
	}

	private List<BcmPlayerGame> ArcadeTradYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.AchievementCount == 12).ToList();
	}

	private List<BcmPlayerGame> RaidBossYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval.Where(x => x.Game!.TrueAchievementId != 0 && x.Platform == Platform.Xbox360.Value && x.Game!.FullCompletionEstimate >= 100).ToList();
	}

	private List<BcmPlayerGame> OrthographerYearly(BcmPlayer player, List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval
				.Where(x => x.Game!.TrueAchievementId != 0 && x.Game!.Title != null && x.Game.Title.Distinct().Count() >= 14)
				.ToList();
	}

	private List<BcmPlayerGame> GenreProjYearly(List<BcmPlayerGame> gamesToEval)
	{
		return gamesToEval
				.Join(_context.GameGenres,
							pg => pg.GameId,
							genre => genre.GameId,
							(pg, genre) => new { Game = pg, GenreId = genre.GenreId })
				.GroupBy(x => x.Game.GameId)
				.Where(group => group.Count() >= 4)
				.Select(group => group.First().Game)
				.ToList();
	}
}
