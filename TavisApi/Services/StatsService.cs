using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
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

    //_context.BcmMonthlyStats.Add(new BcmMonthlyStat
    //{
    //  Challenge = 1,
    //  BonusPoints = bonusPoints,
    //  Participation = filteredCompletions.Count(x => x.Game!.ReleaseDate!.Value.Year <= 2023) > 0,
    //  AllBuckets = hasAllBuckets,
    //  CommunityBonus = communityGoalReached && hasCompleted360Game,
    //  BcmPlayer = player,
    //  BcmPlayerId = player.Id
    //});

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

    foreach(var completion in completedGames)
    {
      var rawPoints = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game!.FullCompletionEstimate);

      if (completion.Game.ReleaseDate!.Value.Year >= 2005 && completion.Game.ReleaseDate!.Value.Year <= 2009 && rawPoints != null)
      {
        var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .5));
        gameBonusPoints += bonusPoints;
        bucket1Points += bonusPoints;
        bucket1Comps++;
      }
        
      if (completion.Game.ReleaseDate!.Value.Year >= 2010 && completion.Game.ReleaseDate!.Value.Year <= 2014 && rawPoints != null)
      {
        var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .25));
        gameBonusPoints += bonusPoints;
        bucket2Points += bonusPoints;
        bucket2Comps++;
      }
        
      if (completion.Game.ReleaseDate!.Value.Year >= 2015 && completion.Game.ReleaseDate!.Value.Year <= 2019 && rawPoints != null)
      {
        var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .15));
        gameBonusPoints += bonusPoints;
        bucket3Points += bonusPoints;
        bucket3Comps++;
      }
        
      if (completion.Game.ReleaseDate!.Value.Year >= 2020 && completion.Game.ReleaseDate!.Value.Year <= 2023 && rawPoints != null)
      {
        var bonusPoints = Convert.ToInt32(Math.Floor(rawPoints.Value * .10));
        gameBonusPoints += bonusPoints;
        bucket4Points += bonusPoints;
        bucket4Comps++;
      }
    }

    var hasCompleted360Game = completedGames.Count(x => x.Platform == Platform.Xbox360) > 0;

    var playerJanStats = _context.JanRecap.Add(new JanRecap
    {
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

  private IEnumerable<BcmPlayerGame> CommunityBounties()
  {
    var allMarCompletions = _context.BcmPlayerGames
        .Include(x => x.Game)
        .Where(x => x.CompletionDate != null
            && x.CompletionDate.Value.Year == 2024
            && x.CompletionDate.Value.Month == 3)
        .ToList();

    return allMarCompletions.Where(x => Bounties().Contains(x.Game!));
  }

  public void CalcMarBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonusReached)
  {
    var communityBounties = CommunityBounties();
    var completedBounties = completedGames.Where(x => Bounties().Contains(x.Game!));
    double bonusPoints = 0;
    var bestBounty = (new BcmPlayerGame(), 0.0);
    var currentBounty = (new BcmPlayerGame(), 0.0);

    foreach (var bounty in completedBounties)
    {
      var bountyProgress = communityBounties.Where(x => x.Game!.Id == bounty.GameId).Count();
      if (bountyProgress == 1)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 4;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 2)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 2.5;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 3)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * 1;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 4)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .9;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 5)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .8;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 6)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .75;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 7)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .7;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 8)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .65;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 9)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .6;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress == 10)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .55;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }
      else if (bountyProgress > 10)
      {
        bonusPoints += (_bcmService.CalcBcmValue(bounty.Platform, bounty.Game!.SiteRatio, bounty.Game!.FullCompletionEstimate) ?? 0) * .5;
        currentBounty.Item1 = bounty;
        currentBounty.Item2 = bonusPoints;
      }

      if (bestBounty.Item2 == 0 || bestBounty.Item2 < currentBounty.Item2)
        bestBounty = currentBounty;
    }

    var playerBountiesCompleted = completedBounties.Count();
    var playerMetCommunityBonus = communityBonusReached && playerBountiesCompleted > 0;
    var claimedBounties = completedBounties.Count() == 0 ? "" : completedBounties.Count() == 1 ? completedBounties.First().Game!.Title : string.Join(", ", completedBounties.Select(x => x.Game!.Title));

    var playerMarStats = _context.MarRecap.Add(new MarRecap
    {
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
      .GroupBy(x => x.Game)
      .Select(g => Tuple.Create(g.Key, g.Count()))
      .ToList();

    if (allMarCompletions.Count() < 1) return false;

    foreach(var game in allMarCompletions)
    {
      if (game.Item2 < 2) return false;
    }

    return true;
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

    foreach (var completion in completedGames)
    {
      var completionCollection = allFebCompletions.Where(x => Queries.FilterGamesForYearlies(x.Item1, completion)).FirstOrDefault(x => x.Item1.Id == completion.GameId);

      if (completionCollection == null) continue;
      if (completionCollection.Item2 < 2) continue;

      var baseGamePoints = _bcmService.CalcBcmValue(completion.Platform, completionCollection!.Item1.SiteRatio, completionCollection!.Item1.FullCompletionEstimate);

      if (completionCollection!.Item2 > 10)
      {
        duodeComps++;
        duodePoints += (baseGamePoints ?? 0) * 1;
      }
        
      if (completionCollection!.Item2 == 10)
      {
        undeComps++;
        undePoints += (baseGamePoints ?? 0) * .9;
      }
        
      if (completionCollection!.Item2 == 9)
      {
        decComps++;
        undePoints += (baseGamePoints ?? 0) * .8;
      }
        
      if (completionCollection!.Item2 == 8)
      {
        octComps++;
        octPoints += (baseGamePoints ?? 0) * .7;
      }
        
      if (completionCollection!.Item2 == 7)
      {
        sepComps++;
        sepPoints += (baseGamePoints ?? 0) * .6;
      }
        
      if (completionCollection!.Item2 == 6)
      {
        sexComps++;
        sexPoints += (baseGamePoints ?? 0) * .5;
      }
        
      if (completionCollection!.Item2 == 5)
      {
        quintComps++;
        quintPoints += (baseGamePoints ?? 0) * .4;
      }
        
      if (completionCollection!.Item2 == 4)
      {
        quadComps++;
        quadPoints += (baseGamePoints ?? 0) * .3;
      }
        
      if (completionCollection!.Item2 == 3)
      {
        triComps++;
        triPoints += (baseGamePoints ?? 0) * .2;
      }
        
      if (completionCollection!.Item2 == 2)
      {
        biComps++;
        biPoints += (baseGamePoints ?? 0) * .1;
      }
    }

    var playerMetCommunityBonus = communityBonusReached && (triComps > 0 || quadComps > 0 || quintComps > 0 || sexComps > 0 || sepComps > 0 || octComps > 0 || decComps > 0 || undeComps > 0 || duodeComps > 0);

    var playerFebStats = _context.FebRecap.Add(new FebRecap
    {
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
