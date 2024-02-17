using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tavis.Models;
using TavisApi.Context;
using Microsoft.EntityFrameworkCore;
using TavisApi.Services;
using System.Text.RegularExpressions;
using System;

namespace TavisApi.Controllers;

[Route("[controller]")]
[ApiController]
public class MonthlyController : ControllerBase
{
  private TavisContext _context;
  private readonly IUserService _userService;

  public MonthlyController(TavisContext context, IUserService userService)
  {
    _context = context;
    _userService = userService;
  }

  [HttpGet, Route("jan-leaderboard")]
  public async Task<IActionResult> GetJanLeaderboard()
  {
    var recap = await _context.JanRecap.ToListAsync();
    return Ok(recap);
  }

  [HttpGet, Route("feb-leaderboard")]
  public async Task<IActionResult> GetFebLeaderboard()
  {
    var recap = await _context.FebRecap.ToListAsync();
    return Ok(recap);
  }

  [Authorize(Roles = "Participant")]
  [HttpGet, Route("challenges")]
  public async Task<IActionResult> GetChallenges(string player)
  {
    var localuser = await _context.Users.Include(x => x.BcmPlayer).FirstOrDefaultAsync(x => x.Gamertag == player);
    if (localuser is null) return BadRequest("No user found with supplied gamertag");

    var playerId = localuser.BcmPlayer?.Id;
    if (playerId is null) return BadRequest("Could not get Bcm Player");

    var yearlyList = await _context.YearlyChallenges
        .OrderBy(x => x.Id)
        .ToListAsync();

    var playerYearlyList = yearlyList.GroupJoin(
        _context.PlayerYearlyChallenges.Include(x => x.Game),
        yc => yc.Id,
        pyc => pyc.YearlyChallengeId,
        (yc, pycGroup) => new { YearlyChallenge = yc, PlayerYearlyChallenge = pycGroup.FirstOrDefault(x => x.PlayerId == playerId) }
    );

    return Ok(playerYearlyList);
  }

  [Authorize(Roles = "Participant")]
  [HttpPost, Route("save-writein")]
  public async Task<IActionResult> SaveWriteIn([FromBody] WriteInOption writeIn)
  {
    var currentUsername = _userService.GetCurrentUserName();
    var user = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == currentUsername);

    if (user is null) return BadRequest("No gamertag matches current user");

    Game matchedGame = new Game();

    if (writeIn.PlayerYearlyChallenge?.WriteIn is not null)
    {
      string pattern = @"(/game/[^/]+/)";
      var regex = new Regex(pattern);
      Match match = regex.Match(writeIn.PlayerYearlyChallenge.WriteIn);
      var foundGame = false;

      if (!match.Success)
      {
        match = regex.Match(writeIn.PlayerYearlyChallenge.WriteIn + "/");
      }

      if (match.Success)
      {
        string capturedUrl = match.Groups[1].Value;
        var game = await _context.Games.FirstOrDefaultAsync(x => x.Url == capturedUrl);

        if (game != null)
        {
          foundGame = true;
          matchedGame = game;
        }
      }

      if (!match.Success || !foundGame)
        matchedGame = new Game { Id = 0, Title = "Unknown Game" };
    }
    
    var newWriteIn = new PlayerYearlyChallenge
    {
      YearlyChallengeId = writeIn.YearlyChallenge!.Id,
      PlayerId = user.BcmPlayer!.Id,
      WriteIn = writeIn.PlayerYearlyChallenge?.WriteIn,
      Reasoning = writeIn.PlayerYearlyChallenge?.Reasoning,
      GameId = matchedGame.Id,
      Game = matchedGame
    };

    var existingPyc = _context.PlayerYearlyChallenges.FirstOrDefault(x => x.YearlyChallengeId == writeIn.YearlyChallenge!.Id && x.PlayerId == user.BcmPlayer!.Id);

    if (existingPyc != null)
    {
      existingPyc.YearlyChallengeId = writeIn.YearlyChallenge!.Id;
      existingPyc.WriteIn = writeIn.PlayerYearlyChallenge?.WriteIn;
      existingPyc.Reasoning = writeIn.PlayerYearlyChallenge?.Reasoning;
      existingPyc.GameId = matchedGame.Id;
      existingPyc.Game = matchedGame;
    }
    else
      _context.PlayerYearlyChallenges.Add(newWriteIn);

    await _context.SaveChangesAsync();

    return Ok(newWriteIn);
  }
} 
