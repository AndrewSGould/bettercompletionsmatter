namespace WebApi.Controllers;

using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;
using System.Data;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;
using System.Diagnostics;

[ApiController]
[Route("[controller]")]
public class BcmController : ControllerBase
{
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;
  private readonly IBcmService _bcmService;
  private readonly IUserService _userService;
  private readonly IDiscordService _discordService;
  private static readonly Random rand = new Random();

  public BcmController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService, IUserService userService, IDiscordService discordService)
  {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _bcmService = bcmService;
    _userService = userService;
    _discordService = discordService;
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

    return Ok(players.OrderBy(x => x.BcmStats?.Rank));
  }

  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("recalcBcmLeaderboard")]
  public IActionResult RecalcBcmLeaderboard()
  {
    //TODO: Changing data. This should be a POST
    var players = _bcmService.GetPlayers();

    var leaderboardList = new List<Ranking>();

    foreach (var player in players)
    {
      var playerBcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id);

      if (playerBcmStats == null)
      {
        playerBcmStats = new BcmStat();
        _context.BcmStats.Add(playerBcmStats);
      }

      playerBcmStats.PlayerId = player.Id;

      var playerCompletions = _context.BcmPlayerGames
                                      .Where(x => x.PlayerId == player.Id &&
                                        x.CompletionDate != null &&
                                        x.CompletionDate >= _bcmService.GetContestStartDate());

      var gamesCompletedThisYear = playerCompletions.Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { PlayersGames = pg, Games = g }).ToList();

      var completedGamesCount = gamesCompletedThisYear.Count();
      var ratioOfGames = gamesCompletedThisYear.Select(x => x.Games.SiteRatio);
      var estimateOfGames = gamesCompletedThisYear.Select(x => x.Games.FullCompletionEstimate);

      playerBcmStats.Completions = completedGamesCount;
      playerBcmStats.AverageRatio = ratioOfGames.DefaultIfEmpty(0).Average();
      playerBcmStats.HighestRatio = ratioOfGames.DefaultIfEmpty(0).Max();
      playerBcmStats.AverageTimeEstimate = estimateOfGames.DefaultIfEmpty(0).Average();
      playerBcmStats.HighestTimeEstimate = estimateOfGames.DefaultIfEmpty(0).Max();

      double? basePoints = 0.0;
      foreach (var game in gamesCompletedThisYear)
      {
        var pointValue = _bcmService.CalcBcmValue(game.PlayersGames.Platform, game.Games.SiteRatio, game.Games.FullCompletionEstimate);
        if (pointValue != null)
          basePoints += pointValue;
      }

      playerBcmStats.BasePoints = basePoints;
      playerBcmStats.AveragePoints = completedGamesCount != 0 ? basePoints / completedGamesCount : 0;

      leaderboardList.Add(new Ranking
      {
        PlayerId = player.Id,
        BcmPoints = playerBcmStats.BasePoints
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

  [HttpGet]
  [Route("getBcmPlayer")]
  public IActionResult BcmPlayer(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("No gamertag found with provided player");

    var bcmPlayer = _context.BcmPlayers.First(x => x.UserId == localuser.Id);

    if (bcmPlayer == null) return BadRequest("Player not found");

    var bcmPlayerSummary = new object();

    var playersGames = _context.BcmPlayerGames
                        .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { PlayersGames = pg, Games = g })
                        .Where(x => x.PlayersGames.PlayerId == bcmPlayer.Id
                          && x.PlayersGames.CompletionDate != null
                          && x.PlayersGames.CompletionDate >= _bcmService.GetContestStartDate())
                        .OrderByDescending(x => x.PlayersGames.CompletionDate)
                        .Select(x => new BcmPlayerSummary
                        {
                          Title = x.Games.Title ?? "",
                          Ratio = x.Games.SiteRatio,
                          Estimate = x.Games.FullCompletionEstimate,
                          CompletionDate = x.PlayersGames.CompletionDate,
                          Points = _bcmService.CalcBcmValue(x.PlayersGames.Platform, x.Games.SiteRatio, x.Games.FullCompletionEstimate)
                        }).ToList();

    bcmPlayerSummary = new
    {
      Player = bcmPlayer,
      Games = playersGames,
      Ranking = _context.BcmStats.First(x => x.PlayerId == bcmPlayer.Id),
      Score = playersGames.Sum(x => x.Points)
    };

    return Ok(bcmPlayerSummary);
  }

  [HttpGet]
  [Route("yearly-summary")]
  public async Task<IActionResult> GetYearlySummary(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("Player not found with provided gamertag");

    var bcmPlayer = _context.BcmPlayers.First(x => x.UserId == localuser.Id);

    return await Task.FromResult(Ok(new
    {
      completionLetters = _bcmService.GetAlphabetChallengeProgress(bcmPlayer.Id),
      OddJobCompletions = _bcmService.GetOddJobChallengeProgress(bcmPlayer.Id),
      YearlyCompletions = 0
    }));
  }

  [HttpPost, Authorize(Roles = "Admin")]
  [Route("rollRandom")]
  public IActionResult RollRandomGame(string? gamertag)
  {
    var players = _context.BcmPlayers.Include(u => u.User).Include(x => x.BcmRgscs).ToList();
    var currentBcmPlayer = players.FirstOrDefault(x => x.User!.Gamertag == gamertag);

    if (currentBcmPlayer is null)
    {
      players = players.Where(x => x.BcmRgscs!.Count() == 0 ||
        x.BcmRgscs!.Any(x => x.Issued <= DateTime.UtcNow.AddHours(-4))).ToList();

      if (players.Count() < 1) return BadRequest("no users left to random");

      var playerIndex = new Random().Next(0, players.Count);
      currentBcmPlayer = players[playerIndex];
    }

    _context.Attach(currentBcmPlayer);

    var randomGameOptions = _context.BcmPlayerGames?
            .Join(_context.Games!, pg => pg.GameId,
              g => g.Id, (pg, g) => new { BcmPlayersGames = pg, Games = g })
            .Where(x => x.BcmPlayersGames.PlayerId == currentBcmPlayer.Id
              && x.Games.GamersCompleted > 0
              && x.Games.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
              && !x.Games.Unobtainables
              && !x.BcmPlayersGames.NotForContests
              && x.BcmPlayersGames.CompletionDate == null
              && x.BcmPlayersGames.Ownership != Ownership.NoLongerHave
              && BcmRule.RandomValidPlatforms.Contains(x.BcmPlayersGames.Platform!))
            .AsEnumerable() // TODO: rewrite so this stays as a query?
            .Where(x => Queries.FilterGamesForYearlies(x.Games, x.BcmPlayersGames))
            .ToList();

    var currentRandoms = _context.BcmRgsc.Where(x => !x.Rerolled && x.BcmPlayerId == currentBcmPlayer.Id);

    randomGameOptions?.Select(x => !currentRandoms.Any(y => y.GameId == x.Games.Id));

    if (randomGameOptions is null || randomGameOptions?.Count() <= 50)
    {
      _context.BcmRgsc.Add(new BcmRgsc
      {
        Issued = DateTime.UtcNow,
        GameId = null,
        BcmPlayerId = currentBcmPlayer.Id
      });

      _context.SaveChanges();

      return Ok(new { PoolSize = randomGameOptions?.Count() ?? 0, currentBcmPlayer.User });
    }

    var randomIndex = new Random().Next(0, randomGameOptions!.Count);
    var currentRandom = randomGameOptions[randomIndex];

    // Try to update any rolls that didn't result in a valid pick first
    if (currentRandoms.Any(x => x.GameId == null))
    {
      var randomToUpdate = currentRandoms.First();
      randomToUpdate.Issued = DateTime.UtcNow;
      randomToUpdate.GameId = currentRandom.Games.Id;
    }
    else
    {
      _context.BcmRgsc.Add(new BcmRgsc
      {
        Issued = DateTime.UtcNow,
        GameId = currentRandom.Games.Id,
        BcmPlayerId = currentBcmPlayer.Id
      });
    }

    _context.SaveChanges();

    return Ok(new
    {
      PoolSize = randomGameOptions.Count(),
      currentBcmPlayer.User,
      Result = currentRandom,
      BcmValue = _bcmService.CalcBcmValue(currentRandom.BcmPlayersGames.Platform, currentRandom.Games.SiteRatio, currentRandom.Games.FullCompletionEstimate)
    });
  }

  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("produceStatReport")]
  public IActionResult StatReport()
  {
    var bcmPlayers = _bcmService.GetPlayers();
    var statSpread = new List<object>();

    foreach (var player in bcmPlayers)
    {
      var gamertag = _context.Users.FirstOrDefault(x => x.Id == player.UserId);

      var playerGames = _context.BcmPlayerGames.Where(x => x.PlayerId == player.Id);

      var gamerscoreTotal = playerGames.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Year == 2023 && x.CompletionDate.Value.Month == 2)
                                        .Sum(x => x.Gamerscore);
      var trueachievementTotal = playerGames.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Year == 2023 && x.CompletionDate.Value.Month == 2)
                                            .Sum(x => x.TrueAchievement);
      var completions = playerGames.Where(x => x.CompletionDate != null && x.CompletionDate.Value.Year == 2023 && x.CompletionDate.Value.Month == 2)
                                    .Count(x => x.CompletionDate != null);

      var stats = new
      {
        Player = gamertag,
        Gamerscore = gamerscoreTotal,
        TrueAchievement = trueachievementTotal,
        Ratio = trueachievementTotal == 0 ? 0 : Math.Round((decimal)((decimal)trueachievementTotal! / gamerscoreTotal!), 4),
        TAD = trueachievementTotal == 0 ? 0 : trueachievementTotal - gamerscoreTotal,
        Completions = completions
      };

      statSpread.Add(stats);
    }

    return Ok(statSpread);
  }

  [Authorize(Roles = "Guest")]
  [HttpPost, Route("registerUser")]
  public async Task<IActionResult> RegisterUser()
  {
    User? user = _userService.GetCurrentUser();
    if (user is null) return BadRequest("Could not determine user");

    var bcmReg = _context.Registrations.Find(_bcmService.GetRegistrationId()) ?? throw new Exception("Unable to get Registration ID for BCM");

    user.UserRegistrations.Add(new UserRegistration { Registration = bcmReg, RegistrationDate = DateTime.UtcNow });

    _context.BcmPlayers.Add(new BcmPlayer
    {
      UserId = user.Id,
    });

    _context.SaveChanges();

    try
    {
      await _discordService.AddBcmParticipantRole(user);
      var userInfo = _context.Users.Include(x => x.UserRegistrations)
                                .FirstOrDefault(x => x.UserRegistrations.Any(x => x.User == user && x.Registration.Name == "Better Completions Matter"));

      return Ok(new { RegDate = userInfo?.UserRegistrations.FirstOrDefault()?.RegistrationDate });
    }
    catch
    {
      return BadRequest("Something went wrong trying to register for BCM");
    }
  }

  [Authorize(Roles = "Participant")]
  [HttpPost, Route("unregisterUser")]
  public async Task<IActionResult> UnregisterUser()
  {
    throw new NotImplementedException();
  }
}
