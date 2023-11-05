using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.ContestRules;
using TavisApi.Context;

namespace TavisApi.Services;

public class BcmService : IBcmService
{
  private TavisContext _context;
  private int _bcmContestId;

  public BcmService(TavisContext context)
  {
    _context = context;
    _bcmContestId = GetContestId();
  }

  public List<Player> GetPlayers()
  {
    var bcmPlayers = _context.PlayerContests!.Where(x => x.ContestId == _bcmContestId).Select(x => x.PlayerId);
    return _context.Players!.Where(x => x.IsActive && bcmPlayers.Contains(x.Id)).OrderBy(x => x.Name).ToList();
  }

  public DateTime? GetContestStartDate()
  {
    return _context.Contests.Where(x => x.Id == _bcmContestId).Select(x => x.StartDate).FirstOrDefault();
  }

  public int? CalcBcmValue(double? ratio, double? estimate)
  {
    var rawPoints = Math.Pow((double)ratio, 1.5) * estimate;
    return rawPoints >= 1500 ? 1500 : Convert.ToInt32(rawPoints);
  }

  public List<string> GetAlphabetChallengeProgress(long playerId)
  {
    var playersCompletedGames = _context.PlayerGames.Where(x => x.PlayerId == playerId
                                                      && x.CompletionDate != null
                                                      && x.CompletionDate.Value.Year == DateTime.Now.Year);

    var completionCharacters = playersCompletedGames.Select(x => x.Game.Title.Substring(0, 1)).AsEnumerable();
    return completionCharacters.Where(x => char.IsLetter(x[0])).Distinct().OrderBy(x => x).ToList();
  }

  public List<Game> GetOddJobChallengeProgress(long playerId)
  {
    var playersCompletedGames = _context.PlayerGames.Include(x => x.Game)
                            .Join(_context.GameGenres, pcg => pcg.GameId, genre => genre.GameId, (pcg, genre) => new { pcg, genre })
                            .AsEnumerable() // TODO: rewrite so this stays as a query?
                            .Where(x => x.pcg.PlayerId == playerId
                                      && Queries.FilterCompletedPlayerGames(x.pcg)
                                      && Queries.FilterGamesForYearlies(x.pcg.Game))
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

  private int GetContestId()
  {
    return _context.Contests.Where(x => x.Name.Contains("Better Completions Matter")).First().Id;
  }
}
