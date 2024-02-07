using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;

namespace TavisApi.Services;

public class StatsService : IStatsService
{
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

    var bonusPoints = 0;

    bonusPoints += ScoreJanBaseBonus(filteredCompletions);

    var hasAllBuckets = HasBucketBonus(filteredCompletions);
    bonusPoints += hasAllBuckets ? 250 : 0;

    var hasCompleted360Game = filteredCompletions.Count(x => x.Platform == Platform.Xbox360) > 0;
    bonusPoints += communityGoalReached && hasCompleted360Game ? 500 : 0;

    _context.BcmMonthlyStats.Add(new BcmMonthlyStat
    {
      Challenge = 1,
      BonusPoints = bonusPoints,
      Participation = filteredCompletions.Count(x => x.Game!.ReleaseDate!.Value.Year <= 2023) > 0,
      AllBuckets = hasAllBuckets,
      CommunityBonus = communityGoalReached && hasCompleted360Game,
      BcmPlayer = player,
      BcmPlayerId = player.Id
    });

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

  private int ScoreJanBaseBonus(List<BcmPlayerGame> completedGames)
  {
    var gameBonusPoints = 0;

    foreach(var completion in completedGames)
    {
      var rawPoints = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate);

      if (completion.Game.ReleaseDate!.Value.Year >= 2005 && completion.Game.ReleaseDate!.Value.Year <= 2009 && rawPoints != null)
        gameBonusPoints += Convert.ToInt32(Math.Floor(rawPoints.Value * .5));

      if (completion.Game.ReleaseDate!.Value.Year >= 2010 && completion.Game.ReleaseDate!.Value.Year <= 2014 && rawPoints != null)
        gameBonusPoints += Convert.ToInt32(Math.Floor(rawPoints.Value * .25));

      if (completion.Game.ReleaseDate!.Value.Year >= 2015 && completion.Game.ReleaseDate!.Value.Year <= 2019 && rawPoints != null)
        gameBonusPoints += Convert.ToInt32(Math.Floor(rawPoints.Value * .15));

      if (completion.Game.ReleaseDate!.Value.Year >= 2020 && completion.Game.ReleaseDate!.Value.Year <= 2023 && rawPoints != null)
        gameBonusPoints += Convert.ToInt32(Math.Floor(rawPoints.Value * .10));
    }

    return gameBonusPoints;
  }
}
