namespace WebApi.Controllers;

using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;
using System.Data;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;

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

  [HttpGet, Authorize(Roles = "Guest")]
  [Route("getPlayerList")]
  public IActionResult GetPlayerList()
  {
    return Ok(_bcmService.GetPlayers().Select(x => x.User!.Gamertag).ToList());
  }

  [HttpGet]
  [Authorize(Roles = "Guest")]
  [Route("getBcmPlayer")]
  public IActionResult BcmPlayer(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("No gamertag found with provided player");

    var bcmPlayer = _context.BcmPlayers.First(x => x.UserId == localuser.Id);

    if (bcmPlayer == null) return BadRequest("Player not found");

    var playerBcmStats = _context.BcmStats?.FirstOrDefault(x => x.PlayerId == bcmPlayer.Id);

    return Ok(bcmPlayer);
  }

  [HttpGet]
  [Authorize(Roles = "Guest")]
  [Route("getBcmPlayerWithGames")]
  public IActionResult BcmPlayerWithGames(string player)
  {
    var localuser = _context.Users.Include(x => x.UserRegistrations).FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("No gamertag found with provided player");

    var bcmPlayer = _context.BcmPlayers.First(x => x.UserId == localuser.Id);

    if (bcmPlayer == null) return BadRequest("Player not found");

    var registrations = _context.Registrations
        .Include(x => x.UserRegistrations)
        .Where(x => x.UserRegistrations.Any(ur => ur.UserId == localuser.Id))
        .ToList();

    var bcmRegDate = registrations.First(x => x.Id == 1).StartDate;

    var userRegDate = localuser.UserRegistrations.First(x => x.RegistrationId == 1).RegistrationDate; // TODO: BCM

    var playerBcmGames = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.BcmPlayer == bcmPlayer
                                                                                && x.CompletionDate != null
                                                                                && x.CompletionDate > userRegDate
                                                                                && x.CompletionDate > bcmRegDate);

    var pointedGames = new List<object>();

    foreach (var game in playerBcmGames)
    {
      var newGame = new
      {
        Game = game,
        Points = _bcmService.CalcBcmValue(game.Platform, game.Game.SiteRatio, game.Game.FullCompletionEstimate),
      };

      pointedGames.Add(newGame);
    }

    var avgRatio = playerBcmGames
        .Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate && x.CompletionDate > bcmRegDate)
        .Select(x => x.Game.SiteRatio)
        .AsEnumerable()
        .DefaultIfEmpty(0)
        .Average();
    var avgTime = playerBcmGames
        .Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate && x.CompletionDate > bcmRegDate)
        .Select(x => x.Game.FullCompletionEstimate)
        .AsEnumerable()
        .DefaultIfEmpty(0)
        .Average();
    var highestTime = playerBcmGames
        .Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate && x.CompletionDate > bcmRegDate)
        .Select(x => x.Game.FullCompletionEstimate)
        .AsEnumerable()
        .DefaultIfEmpty(0)
        .Max();
    var highestRatio = playerBcmGames
        .Where(x => x.CompletionDate != null && x.CompletionDate > userRegDate && x.CompletionDate > bcmRegDate)
        .Select(x => x.Game.SiteRatio)
        .AsEnumerable()
        .DefaultIfEmpty(0)
        .Max();

    return Ok(new
    {
      Player = bcmPlayer,
      Games = pointedGames,
      AvgRatio = avgRatio,
      AvgTime = avgTime,
      HighestTime = highestTime,
      HighestRatio = highestRatio
    });
  }

  [HttpGet]
  [Authorize(Roles = "Guest")]
  [Route("yearlySummary")]
  public async Task<IActionResult> GetPlayerYearlySummary(string player)
  {
    var localuser = await _context.Users.FirstOrDefaultAsync(x => x.Gamertag == player);
    if (localuser is null) return BadRequest("Player not found with the provided gamertag");

    var bcmPlayer = await _context.BcmPlayers.FirstOrDefaultAsync(x => x.UserId == localuser.Id);
    if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

    return Ok(_bcmService.GetParticipationProgress(bcmPlayer));
  }


  [HttpGet]
  [Authorize(Roles = "Guest")]
  [Route("player/abcSummary")]
  public async Task<IActionResult> GetPlayerAbcSummary(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("Player not found with the provided gamertag");

    var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);

    if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

    return Ok(await _bcmService.GetAlphabetChallengeProgress(bcmPlayer.Id));
  }

  [HttpGet]
  [Authorize(Roles = "Guest")]
  [Route("player/oddjobSummary")]
  public async Task<IActionResult> GetPlayerOddjobSummary(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("Player not found with the provided gamertag");

    var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);

    if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

    return Ok(await _bcmService.GetOddJobChallengeProgress(bcmPlayer.Id));
  }

  [HttpGet]
  [Authorize(Roles = "Guest")]
  [Route("player/miscstats")]
  public async Task<IActionResult> GetPlayerMiscStats(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);

    if (localuser is null) return BadRequest("Player not found with the provided gamertag");

    var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);

    if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");

    return Ok();
  }

  [Authorize(Roles = "Guest")]
  [HttpGet, Route("monthly/jan")]
  public async Task<IActionResult> JanSummary(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
    if (localuser is null) return BadRequest("Player not found with the provided gamertag");

    var bcmPlayer = _context.BcmPlayers.FirstOrDefault(x => x.UserId == localuser.Id);
    if (bcmPlayer is null) return BadRequest("BCM Player not found for the provided user");    

    return Ok(await _context.BcmMonthlyStats.FirstOrDefaultAsync(x => x.BcmPlayerId == bcmPlayer.Id && x.Challenge == 1));
  }

  [Authorize(Roles = "Guest")]
  [HttpPost, Route("registerUser")]
  public async Task<IActionResult> RegisterUser()
  {
    try
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
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "Participant")]
  [HttpPost, Route("unregisterUser")]
  public async Task<IActionResult> UnregisterUser()
  {
    throw new NotImplementedException();
  }

  [Authorize(Roles = "Participant")]
  [HttpGet, Route("getPlayersGenres")]
  public async Task<IActionResult> GetPlayersGenres(string player)
  {
    var localuser = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == player);
    if (localuser is null) return BadRequest("No user found with supplied gamertag");

    var playerId = localuser.BcmPlayer?.Id;
    if (playerId is null) return BadRequest("Could not get Bcm Player");

    var pgs = _context.BcmPlayerGames.Include(x => x.Game).Where(x => x.BcmPlayer == localuser.BcmPlayer && x.CompletionDate != null);

    var genreStats = await _context.Genres
      .GroupJoin(
          _context.GameGenres,
          genre => genre.Id,
          gameGenre => gameGenre.GenreId,
          (genre, gameGenres) => new
          {
            GenreId = genre.Id,
            GenreName = genre.Name,
            GenreCount = gameGenres
            .Join(
                _context.BcmPlayerGames
                    .Where(bpg => bpg.PlayerId == playerId && bpg.CompletionDate != null),
                gg => gg.GameId,
                bpg => bpg.GameId,
                (gg, bpg) => gg // Use gg instead of 1
            )
            .Count()
          }
      )
      .OrderByDescending(result => result.GenreCount)
      .Select(x => new
      {
        Name = x.GenreName,
        Value = x.GenreCount
      })
      .ToListAsync();

    return Ok(genreStats);
  }
}
