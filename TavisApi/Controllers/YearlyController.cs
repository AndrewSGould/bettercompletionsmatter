using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Models;
using TavisApi.V2.TrueAchievements.Models;
using static TavisApi.Services.YearlyService;

namespace TavisApi.Controllers;

[Route("[controller]")]
[ApiController]
public class YearlyController : ControllerBase {
	private TavisContext _context;
	private readonly IUserService _userService;
	private readonly IYearlyService _yearlyService;
	private readonly IBcmService _bcmService;

	public YearlyController(TavisContext context, IUserService userService, IYearlyService yearlyService, IBcmService bcmService)
	{
		_context = context;
		_userService = userService;
		_bcmService = bcmService;
		_yearlyService = yearlyService;
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
	[HttpGet, Route("options")]
	public async Task<IActionResult> GetYearlyOptions(string player, int yearlyId)
	{
		if (yearlyId != 23 && yearlyId != 24 && yearlyId != 25 && yearlyId != 26 && yearlyId != 28 && yearlyId != 30 && yearlyId != 31 && yearlyId != 32 && yearlyId != 33 && yearlyId != 34 && yearlyId != 35 && yearlyId != 36 && yearlyId != 37 && yearlyId != 38 && yearlyId != 40) return Ok(new List<YearlyOption>());

		var localuser = await _context.Users.Include(x => x.BcmPlayer).FirstOrDefaultAsync(x => x.Gamertag == player);
		if (localuser is null) return BadRequest("No user found with supplied gamertag");

		var playerId = localuser.BcmPlayer?.Id;
		if (playerId is null) return BadRequest("Could not get Bcm Player");

		var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.BcmPlayer!.Id == playerId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
		var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;


		var playerCompletions = await _context.BcmPlayerGames.Include(x => x.Game)
											.Where(x => x.PlayerId == playerId &&
												x.CompletionDate != null &&
												x.CompletionDate >= _bcmService.GetContestStartDate() &&
												x.CompletionDate >= userRegDate!.Value.AddDays(-1) &&
												x.Game!.FullCompletionEstimate != null
												&& ((x.Game!.SiteRatio > (x.Platform == Platform.Xbox360.Value ? 1.0 : 1.5) && x.Game!.FullCompletionEstimate >= 6)
														|| x.Game!.FullCompletionEstimate > 20))
											.ToListAsync();

		var playerIncompletions = await _context.BcmPlayerGames.Include(x => x.Game)
											.Where(x => x.PlayerId == playerId &&
												x.CompletionDate == null &&
												x.Game!.FullCompletionEstimate != null &&
													((x.Game!.SiteRatio > (x.Platform == Platform.Xbox360.Value ? 1.0 : 1.5) && x.Game!.FullCompletionEstimate >= 6)
														|| x.Game!.FullCompletionEstimate > 20))
											.ToListAsync();

		var allOptions = new List<YearlyOption>();

		allOptions.AddRange(_yearlyService.EvalYearlyOptions(localuser.BcmPlayer!, yearlyId, playerCompletions, true));
		allOptions.AddRange(_yearlyService.EvalYearlyOptions(localuser.BcmPlayer!, yearlyId, playerIncompletions, false));

		return Ok(allOptions.OrderBy(x => x.Title).OrderByDescending(x => !x.Disabled));
	}

	[Authorize(Roles = "Participant")]
	[HttpPost, Route("save-automatedgame")]
	public async Task<IActionResult> SaveAutomatedGame([FromBody] Submission submission)
	{
		var currentUsername = _userService.GetCurrentUserName();
		var user = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == currentUsername);

		if (user is null) return BadRequest("No gamertag matches current user");

		if (submission.Option == "") {
			var clearedSubmission = new PlayerYearlyChallenge {
				YearlyChallengeId = submission.YearlyChallenge!.Id,
				PlayerId = user.BcmPlayer!.Id,
				GameId = null,
				Game = null,
			};

			var existPyc = _context.PlayerYearlyChallenges.FirstOrDefault(x => x.YearlyChallengeId == submission.YearlyChallenge!.Id && x.PlayerId == user.BcmPlayer!.Id);
			if (existPyc == null) return Ok(clearedSubmission);
			else {
				_context.PlayerYearlyChallenges.Remove(existPyc);
				await _context.SaveChangesAsync();
				return Ok(clearedSubmission);
			}
		}

		var gameId = Convert.ToInt32(submission.Option);

		var matchedGame = _context.Games.First(x => x.Id == gameId);

		var newSubmission = new PlayerYearlyChallenge {
			YearlyChallengeId = submission.YearlyChallenge!.Id,
			PlayerId = user.BcmPlayer!.Id,
			GameId = matchedGame.Id,
			Game = matchedGame,
			Approved = true,
		};

		var existingPyc = _context.PlayerYearlyChallenges.FirstOrDefault(x => x.YearlyChallengeId == submission.YearlyChallenge!.Id && x.PlayerId == user.BcmPlayer!.Id);

		if (existingPyc != null) {
			existingPyc.YearlyChallengeId = submission.YearlyChallenge!.Id;
			existingPyc.GameId = matchedGame.Id;
			existingPyc.Game = matchedGame;
			existingPyc.Approved = true;
		}
		else
			_context.PlayerYearlyChallenges.Add(newSubmission);

		await _context.SaveChangesAsync();

		return Ok(newSubmission);
	}

	[Authorize(Roles = "Participant")]
	[HttpPost, Route("save-writein")]
	public async Task<IActionResult> SaveWriteIn([FromBody] Submission submission)
	{
		var currentUsername = _userService.GetCurrentUserName();
		var user = _context.Users.Include(x => x.BcmPlayer).FirstOrDefault(x => x.Gamertag == currentUsername);

		if (user is null) return BadRequest("No gamertag matches current user");

		Game matchedGame = new Game();

		if (submission.PlayerYearlyChallenge?.WriteIn is not null) {
			string pattern = @"(/game/[^/]+/)";
			var regex = new Regex(pattern);
			Match match = regex.Match(submission.PlayerYearlyChallenge.WriteIn);
			var foundGame = false;

			if (!match.Success) {
				match = regex.Match(submission.PlayerYearlyChallenge.WriteIn + "/");
			}

			if (match.Success) {
				string capturedUrl = match.Groups[1].Value;
				var game = await _context.Games.FirstOrDefaultAsync(x => x.Url == capturedUrl);

				if (game != null) {
					foundGame = true;
					matchedGame = game;
				}
			}

			if (!match.Success || !foundGame)
				matchedGame = new Game { Id = 0, Title = "Unknown Game" };
		}

		var newWriteIn = new PlayerYearlyChallenge {
			YearlyChallengeId = submission.YearlyChallenge!.Id,
			PlayerId = user.BcmPlayer!.Id,
			WriteIn = submission.PlayerYearlyChallenge?.WriteIn,
			Reasoning = submission.PlayerYearlyChallenge?.Reasoning,
			GameId = matchedGame.Id,
			Game = matchedGame
		};

		var existingPyc = _context.PlayerYearlyChallenges.FirstOrDefault(x => x.YearlyChallengeId == submission.YearlyChallenge!.Id && x.PlayerId == user.BcmPlayer!.Id);

		if (existingPyc != null) {
			existingPyc.YearlyChallengeId = submission.YearlyChallenge!.Id;
			existingPyc.WriteIn = submission.PlayerYearlyChallenge?.WriteIn;
			existingPyc.Reasoning = submission.PlayerYearlyChallenge?.Reasoning;
			existingPyc.GameId = matchedGame.Id;
			existingPyc.Game = matchedGame;
		}
		else
			_context.PlayerYearlyChallenges.Add(newWriteIn);

		await _context.SaveChangesAsync();

		return Ok(newWriteIn);
	}
}
