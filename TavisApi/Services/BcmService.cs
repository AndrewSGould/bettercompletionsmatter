using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    var playersCompletedGames = await _context.BcmPlayerGames
                      .Join(_context.Games.Include(x => x.GameGenres), pcg => pcg.GameId, game => game.Id, (pcg, game) => new { pcg, game })
                      .Where(x => x.pcg.PlayerId == playerId &&
                        x.pcg.CompletionDate != null && x.pcg.CompletionDate.Value.Year == 2024)
                      .ToListAsync();

    // now that we have the list of 2024 completions, lets apply our unqiue logic
    playersCompletedGames = playersCompletedGames.Where(x => Queries.FilterGamesForYearlies(x.game, x.pcg)).ToList();

    var completionCharacters = playersCompletedGames
        .Select(x => x.game?.Title?.Substring(0, 1))
        .AsEnumerable();

    return completionCharacters
        .Where(x => char.IsLetter(x[0]))
        .Distinct()
        .OrderBy(x => x)
        .ToList();
  }

  public async Task<List<Game>> GetOddJobChallengeProgress(long playerId)
  {
    var playersCompletedGames = await _context.BcmPlayerGames
                      .Join(_context.Games.Include(x => x.GameGenres), pcg => pcg.GameId, game => game.Id, (pcg, game) => new { pcg, game })
                      .Where(x => x.pcg.PlayerId == playerId &&
                        x.pcg.CompletionDate != null && x.pcg.CompletionDate.Value.Year == 2024)
                      .OrderBy(x => x.pcg.CompletionDate)
                      .ToListAsync();

    // now that we have the list of 2024 completions, let's apply our unique logic
    playersCompletedGames = playersCompletedGames.Where(x => Queries.FilterGamesForYearlies(x.game, x.pcg)).ToList();

    var completedJobs = BcmRule.OddJobs
        .Where(oddjob => playersCompletedGames.Any(completedGame =>
            oddjob.All(job => completedGame.game.GameGenres.Any(genre => genre.GenreId == job))))
        .Select(oddjob => playersCompletedGames
            .First(completedGame =>
                oddjob.All(job => completedGame.game.GameGenres.Any(genre => genre.GenreId == job)))
            .game)
        .Distinct()
        .ToList();

    return completedJobs;
  }

  public int GetParticipationProgress(BcmPlayer player)
  {
    return _context.BcmMonthlyStats.Where(x => x.BcmPlayerId == player.Id && x.BonusPoints > 0).GroupBy(x => x.Challenge).Count();
  }


  public long? GetRegistrationId()
  {
    return _context.Registrations.Where(x => x.Name != null && x.Name.Contains("Better Completions Matter")).First().Id;
  }
}
