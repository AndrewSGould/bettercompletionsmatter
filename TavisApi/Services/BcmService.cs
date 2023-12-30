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

  public List<string> GetAlphabetChallengeProgress(long playerId)
  {
    var playersCompletedGames = _context.BcmPlayerGames.Where(x => x.PlayerId == playerId
                                                      && x.CompletionDate != null
                                                      && x.CompletionDate.Value.Year == 2024);

    var completionCharacters = playersCompletedGames.Select(x => x.Game.Title.Substring(0, 1)).AsEnumerable();
    return completionCharacters.Where(x => char.IsLetter(x[0])).Distinct().OrderBy(x => x).ToList();
  }

  public List<Game> GetOddJobChallengeProgress(long playerId)
  {
    var playersCompletedGames = _context.BcmPlayerGames.Include(x => x.Game)
                            .Join(_context.GameGenres, pcg => pcg.GameId, genre => genre.GameId, (pcg, genre) => new { pcg, genre })
                            .AsEnumerable() // TODO: rewrite so this stays as a query?
                            .Where(x => x.pcg.PlayerId == playerId
                                      && Queries.FilterCompletedPlayerGames(x.pcg)
                                      && Queries.FilterGamesForYearlies(x.pcg.Game, x.pcg))
                            .ToList();

    var completedJobs = new List<Game>();
    foreach (var completedGame in playersCompletedGames)
    {
      foreach (var oddjob in BcmRule.OddJobs)
      {
        if (oddjob.All(job => completedGame.pcg.Game.GameGenres.Any(genre => genre.GenreId == job)))
        {
          completedJobs.Add(completedGame.pcg.Game);
        }
      }
    }

    return completedJobs.Distinct().ToList();
  }

  public long? GetRegistrationId()
  {
    return _context.Registrations.Where(x => x.Name != null && x.Name.Contains("Better Completions Matter")).First().Id;
  }
}
