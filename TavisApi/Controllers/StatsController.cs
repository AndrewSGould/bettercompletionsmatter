namespace TavisApi.Controllers;

using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;
using System.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TavisApi.Models;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
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
    var players = _bcmService.GetPlayers();

    foreach (var player in players)
    {
      var bcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id);
    }

    return Ok(players.OrderBy(x => x.BcmStats?.Rank ?? 999));
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
  public IActionResult RecalcBcmLeaderboard()
  {
    var players = _bcmService.GetPlayers();

    var leaderboardList = new List<Ranking>();

    _context.BcmMonthlyStats.RemoveRange(_context.BcmMonthlyStats.ToList());

    var janCommunityGoalReached = _statsService.CalcJanCommunityGoal();

    foreach (var player in players)
    {
      var playerBcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id);

      if (playerBcmStats == null)
      {
        playerBcmStats = new BcmStat();
        _context.BcmStats.Add(playerBcmStats);
      }

      playerBcmStats.PlayerId = player.Id;

      var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.Id == player.UserId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
      var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

      var playerCompletions = _context.BcmPlayerGames
                                      .Include(x => x.Game)
                                      .Where(x => x.PlayerId == player.Id &&
                                        x.CompletionDate != null &&
                                        x.CompletionDate >= _bcmService.GetContestStartDate() &&
                                        x.CompletionDate >= userRegDate!.Value.AddDays(-1));

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
      foreach (var completion in gamesCompletedThisYear)
      {
        var pointValue = _bcmService.CalcBcmValue(completion.Platform, completion.Game!.SiteRatio, completion.Game.FullCompletionEstimate);
        if (pointValue != null)
          basePoints += pointValue;
      }

      playerBcmStats.BasePoints = basePoints;
      playerBcmStats.AveragePoints = completedGamesCount != 0 ? basePoints / completedGamesCount : 0;

      var rgscBonus = _statsService.ScoreRgscCompletions(player, gamesCompletedThisYear);
      _statsService.CalcJanBonus(player, gamesCompletedThisYear, janCommunityGoalReached);

      var monthlyBonus = _context.BcmMonthlyStats.FirstOrDefault(x => x.BcmPlayerId == player.Id)?.BonusPoints ?? 0;

      playerBcmStats.BonusPoints = monthlyBonus + rgscBonus;
      playerBcmStats.TotalPoints = basePoints + monthlyBonus + rgscBonus;

      leaderboardList.Add(new Ranking
      {
        PlayerId = player.Id,
        BcmPoints = playerBcmStats.TotalPoints
      });
    }

    _context.SaveChanges();

    // after saving point calculations, lets order the leaderboard and save again for the rankings
    leaderboardList = leaderboardList.OrderByDescending(x => x.BcmPoints).ToList();

    foreach (var player in players)
    {
      var playerBcmStats = _context.BcmStats.First(x => x.PlayerId == player.Id);
      var previousRanking = playerBcmStats.Rank;
      var newRanking = leaderboardList.FindIndex(x => x.PlayerId == player.Id) + 1;

      playerBcmStats.Rank = newRanking;
      playerBcmStats.RankMovement = previousRanking - newRanking;
    }

    _context.SaveChanges();

    return Ok();
  }
}
