using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;

namespace TavisApi.Services;

public class BcmService : IBcmService
{
  private TavisContext _context;
  private long? _bcmContestId;

  public BcmService(TavisContext context)
  {
    _context = context;
    _bcmContestId = GetRegistrationId();
  }

  public List<BcmPlayer> GetPlayers()
  {
    return _context.BcmPlayers!.Include(u => u.User).ToList();
  }

  public DateTime? GetContestStartDate()
  {
    return _context.Registrations.Where(x => x.Id == _bcmContestId).Select(x => x.StartDate).FirstOrDefault();
  }

  public int? CalcBcmValue(int platformId, double? ratio, double? estimate)
  {
    var is360 = platformId == Platform.Xbox360.Value;

    ratio = is360 ? ratio + 0.5 : ratio;
    ratio ??= 0;

    var rawPoints = Math.Pow((double)ratio, 1.5) * estimate;

    rawPoints = is360 ? rawPoints * 1.5 : rawPoints;

    return rawPoints >= BcmRule.MaximumGameScore ? BcmRule.MaximumGameScore : Convert.ToInt32(rawPoints);
  }

  public async Task<List<string>> GetAlphabetChallengeProgress(long playerId)
  {
    var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.BcmPlayer!.Id == playerId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
    var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;

    var playerCompletions = await _context.BcmPlayerGames
                                    .Include(x => x.Game)
                                    .Where(x => x.PlayerId == playerId &&
                                      x.CompletionDate != null &&
                                      x.CompletionDate >= GetContestStartDate() &&
                                      x.CompletionDate >= userRegDate!.Value.AddDays(-1))
                                    .ToListAsync();

    // now that we have the list of 2024 completions, lets apply our unqiue logic
    var filteredPlayerCompletions = playerCompletions.Where(x => Queries.FilterGamesForYearlies(x.Game!, x));

    var completionCharacters = filteredPlayerCompletions
        .Select(x => x.Game?.Title?.Substring(0, 1))
        .AsEnumerable();

    return completionCharacters
        .Where(x => char.IsLetter(x[0]))
        .Distinct()
        .OrderBy(x => x)
        .ToList();
  }

  public async Task<List<Game>> GetOddJobChallengeProgress(long playerId)
  {
    var userWithReg = _context.Users.Include(x => x.UserRegistrations).Where(x => x.BcmPlayer!.Id == playerId && x.UserRegistrations.Any(x => x.RegistrationId == 1));
    var userRegDate = userWithReg.First().UserRegistrations.First().RegistrationDate;


    var playerCompletions = await _context.BcmPlayerGames
                      .Join(_context.Games.Include(x => x.GameGenres), pcg => pcg.GameId, game => game.Id, (pcg, game) => new { pcg, game })
                      .Where(x => x.pcg.PlayerId == playerId &&
                        x.pcg.CompletionDate != null &&
                        x.pcg.CompletionDate >= GetContestStartDate() &&
                        x.pcg.CompletionDate >= userRegDate!.Value.AddDays(-1))
                      .OrderBy(x => x.pcg.CompletionDate)
                      .ToListAsync();

    // now that we have the list of 2024 completions, let's apply our unique logic
    var filteredCompletedGames = playerCompletions.Where(x => Queries.FilterGamesForYearlies(x.game, x.pcg));

    var completedJobs = BcmRule.OddJobs
      .Where(oddjob =>
          filteredCompletedGames.Any(completedGame =>
              oddjob.All(job =>
                  completedGame.game?.GameGenres?.Any(genre =>
                      genre.GenreId == job) ?? false)))
      .Select(oddjob =>
          filteredCompletedGames
              .First(completedGame =>
                  oddjob.All(job =>
                      completedGame.game?.GameGenres?.Any(genre =>
                          genre.GenreId == job) ?? false))
              .game)
      .Distinct()
      .ToList();

    return completedJobs;
  }

  public object GetParticipationProgress(BcmPlayer player)
  {
    var challengeSummary = _context.PlayerYearlyChallenges.Include(x => x.YearlyChallenge).Where(x => x.PlayerId == player.Id);
    var commStar = challengeSummary.Where(x => x.YearlyChallenge!.Category == Data.YearlyCategory.CommunityStar);
    var tavis = challengeSummary.Where(x => x.YearlyChallenge!.Category == Data.YearlyCategory.TheTAVIS);
    var retirement = challengeSummary.Where(x => x.YearlyChallenge!.Category == Data.YearlyCategory.RetirementParty);
    var partipCount = _context.BcmMonthlyStats.Where(x => x.BcmPlayerId == player.Id && x.BonusPoints > 0).GroupBy(x => x.Challenge).Count();
    var febPartipCount = _context.FebRecap.FirstOrDefault(x => x.PlayerId == player.Id)!.Participation ? 1 : 0;

    return new
    {
      Participation = partipCount + febPartipCount,
      CommStarApproved = commStar.Where(x => x.Approved).Count(),
      CommStarUnapproved = commStar.Where(x => !x.Approved).Count(),
      TavisApproved = tavis.Where(x => x.Approved).Count(),
      TavisUnapproved = tavis.Where(x => !x.Approved).Count(),
      RetirementApproved = retirement.Where(x => x.Approved).Count(),
      RetirementUnapproved = retirement.Where(x => !x.Approved).Count()
    };
  }


  public long? GetRegistrationId()
  {
    return _context.Registrations.Where(x => x.Name != null && x.Name.Contains("Better Completions Matter")).First().Id;
  }
}
