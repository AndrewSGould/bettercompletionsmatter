using System.Diagnostics;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.Context;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using static TavisApi.Services.DataSync;
using static TavisApi.Services.TA_GameCollection;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace TavisApi.Services;

public class DataSync : IDataSync {
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly ITA_GameCollection _taGameCollection;

  public DataSync(TavisContext context, IParser parser, ITA_GameCollection taGameCollection) {
    _context = context;
    _parser = parser;
    _taGameCollection = taGameCollection;
  }

  public object DynamicSync(List<Player> players, SyncOptions syncOptions, SyncHistory syncLog, IHubContext<SyncSignal> hub) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    var results = new List<TaParseResult>();

    foreach(var player in players) {
      hub.Clients.All.SendAsync("SyncSignal", $"Parsing {player.Name}...");

      var parseStart = DateTime.UtcNow;
      
      var parsedPlayer = ParseTa(player.Id, syncOptions);
      results.Add(parsedPlayer);
      player.LastSync = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

      var parseEnd = DateTime.UtcNow;

      Console.WriteLine($"{player.Name} has been parsed at {DateTime.Now}");
    }

    _context.SaveChanges();

    var totalHits = 0;
    TimeSpan totalTimeHittingTa = new TimeSpan();
    foreach (var result in results) {
      totalHits = totalHits + result.TaHits;
      totalTimeHittingTa = totalTimeHittingTa.Add(result.TimeHittingTa);
    }

    stopWatch.Stop();

    TimeSpan ts = stopWatch.Elapsed;

    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    string totalTaHitTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        totalTimeHittingTa.Hours, totalTimeHittingTa.Minutes, totalTimeHittingTa.Seconds,
        totalTimeHittingTa.Milliseconds / 10);

    syncLog.TaHits = totalHits;

    return new {
      OverallTime = elapsedTime,
      TotalTaHits = totalHits,
      TotalTimeHittingTa = totalTaHitTime,
      PerPlayerTime = results
    };
  }

  public TaParseResult ParseTa(int playerId, SyncOptions gcOptions) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    var player = _context.Players!.Where(x => x.Id == playerId).First();

    List<List<CollectionSplit>> entireGameList = new List<List<CollectionSplit>>();
    var page = 1;
    var timeHittingTa = ParseCollectionPage(player.TrueAchievementId, entireGameList, gcOptions, ref page);
    
    var incomingData = new List<TA_CollectionEntry>();
    var taGameIdList = _context.Games!.Select(x => x.TrueAchievementId).ToList();

    StructureCollectionPage(entireGameList, incomingData, player);

    // Games can lose their completion if achievements are added
    // Let's remember when the players originally completed the game
    SaveRecompletionHistory(incomingData, taGameIdList, player);

    // a user can remove a game from their collection if they never
    // started it. let's make sure to have Tavis forget about it
    RemoveGamesFromCollection(incomingData, player, gcOptions);

    SaveNewlyDetectedGames(incomingData, taGameIdList);
    UpdateGameInformation(incomingData, taGameIdList);

    SaveNewlyDetectedCollectionEntries(incomingData, player);
    UpdateCollectionInformation(incomingData, taGameIdList, player);

    _context.SaveChanges();
    
    stopWatch.Stop();

    TimeSpan ts = stopWatch.Elapsed;

    // Format and display the TimeSpan value.
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    return new TaParseResult {
      Performance = elapsedTime,
      TaHits = page,
      TimeHittingTa = timeHittingTa
    };
  }

  private void RemoveGamesFromCollection(List<TA_CollectionEntry> incomingData, Player player, SyncOptions gcOptions) {
    // Only remove Games from collection if we are doing a full sync
    if (gcOptions.CompletionStatus.Value != SyncOption_CompletionStatus.All.Value || 
      gcOptions.ContestStatus.Value != SyncOption_ContestStatus.All.Value || 
      gcOptions.DateCutoff != null || gcOptions.LastUnlockCutoff != null)
        return;    

    // Get all the TA ID's Tavis has for the player
    var gamesInCollection = _context.PlayerGames.Where(x => x.PlayerId == player.Id)
                              .Join(_context.Games, pg => pg.GameId, g => g.Id, (pg, g) => new {pg, g})
                              .Select(x => (int)x.g.TrueAchievementId).ToList();

    // Get all the TA ID's for the newly scanned games
    var incomingGames = incomingData.Select(x => x.GameId).ToList();

    // Find the ID's that are in the collection but not in the incoming data
    // These games have been removed from their collection
    var removedGames = gamesInCollection.Except(incomingGames).ToList();

    // Translate the TA ID of the game to Tavis' Game ID
    var tavisGameIds = _context.Games.Where(x => removedGames.Contains(x.TrueAchievementId)).Select(x => x.Id).ToList();

    // Have the DB forget about the removed games
    foreach (var gameId in tavisGameIds) {
      var removedGame = _context.PlayerGames.Where(x => x.GameId == gameId && x.PlayerId == player.Id).First();
      _context.PlayerGames.Remove(removedGame);
    }

    _context.SaveChanges();
  }

  private void SaveRecompletionHistory(List<TA_CollectionEntry> incomingData, List<int> taGameIdList, Player player) {
    // Get all scanned games that are completed
    var incomingCompletedGames = incomingData.Where(x => x.CompletionDate != null);
    var completedIncomingGames = incomingCompletedGames
                                  .Join(_context.Games, cig => cig.GameId, g => g.TrueAchievementId, (cig, g) => new {cig, g});

    // Get the current completion status of the player's games
    var playersCurrentCompletedGames = _context.PlayerGames.Where(x => x.PlayerId == player.Id && x.CompletionDate != null)
                                                            .OrderByDescending(x => x.CompletionDate);

    // Compare the incoming completions with the completions on the PlayerGames table. If it's different it's a re-completion
    foreach(var incomingCompletion in completedIncomingGames) {
      var previouslyCompletedGame = playersCurrentCompletedGames.Where(x => x.Game.TrueAchievementId == incomingCompletion.g.TrueAchievementId).FirstOrDefault();
      if (previouslyCompletedGame != null && incomingCompletion.cig.CompletionDate != previouslyCompletedGame.CompletionDate) {
        _context.PlayerCompletionHistory.Add(new PlayerCompletionHistory {
          PlayerId = player.Id,
          GameId = incomingCompletion.g.Id,
          CompletionDate = previouslyCompletedGame.CompletionDate ?? DateTime.MinValue
        });
      }
    }

    _context.SaveChanges();
  }

  private TimeSpan ParseCollectionPage(int playerTrueAchId, List<List<CollectionSplit>> entireCollection, SyncOptions gcOptions, ref int page) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    using var httpClient = new HttpClient();
    var gameCollectionUrl = _taGameCollection.ParseManager(playerTrueAchId, page, gcOptions);
    var request = new HttpRequestMessage(HttpMethod.Get, gameCollectionUrl);
    request.Headers.TryAddWithoutValidation("User-Agent", "Other");
    var response = httpClient.Send(request);
    using var reader = new StreamReader(response.Content.ReadAsStream());
    var responseBody = reader.ReadToEnd();

    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
    doc.LoadHtml(responseBody);
    
    var collectionPage = doc.DocumentNode.SelectSingleNode("//table")
      ?.Descendants("tr")
      ?.Skip(1)
      ?.SkipLast(1)
      ?.Where(tr=>tr.Elements("td").Count()>1)
      ?.Select(tr => tr.Elements("td")
      ?.Select(td => new CollectionSplit { 
        CollectionValuesHtml = td.InnerText, 
        CollectionImagesHtml = td.InnerHtml, 
        GameIdHtml = td.OuterHtml })
      ?.ToList());

    if (collectionPage != null) {
      entireCollection.AddRange(collectionPage!);

      if (collectionPage.Count() == 100) {

        // if we have a cutoff, lets try to quit parsing early
        if (gcOptions.DateCutoff != null || gcOptions.LastUnlockCutoff != null) {
          var lastEntry = collectionPage.Last();
          var lastEntryCompletionDate = _parser.TaDate(lastEntry![6].CollectionValuesHtml!);
          var lastUnlockDate = _parser.TaDate(lastEntry![7].CollectionValuesHtml);

          if (lastUnlockDate < gcOptions.LastUnlockCutoff) {
            stopWatch.Stop();
            return stopWatch.Elapsed;
          }

          if (lastEntryCompletionDate < gcOptions.DateCutoff) {
            stopWatch.Stop();
            return stopWatch.Elapsed;
          }
        }

        page++;
        ParseCollectionPage(playerTrueAchId, entireCollection, gcOptions, ref page);
      }
    }

    stopWatch.Stop();

    return stopWatch.Elapsed;
  }

  private void StructureCollectionPage(List<List<CollectionSplit>> entireGameList, List<TA_CollectionEntry> incomingData, Player player) {
    // we're going to hardcode the array position here to avoid even more parsing
    //TODO: this deserves an integration test
    //TODO: consider breaking out what we parse if we already have the game on file?
      // maybe we shouldn't parse things that never change, like Dev, Publisher etc
      // if we do that, have a way to force an update
    foreach(var game in entireGameList) {
      try {
        incomingData.Add(new TA_CollectionEntry {
          GameId = _parser.GameId(game[1].GameIdHtml!),
          Title = Extensions.NullIfWhiteSpace(game[0].CollectionValuesHtml!),
          GameUrl = _parser.GameUrl(game[0].CollectionImagesHtml!),
          Platform = _parser.GamePlatform(game[1].CollectionImagesHtml!),
          PlayerTrueAchievement = _parser.PlayersGameSlashedValue(game[2].CollectionValuesHtml!),
          TotalTrueAchievement = _parser.GameTotalSlashedValue(game[2].CollectionValuesHtml!),
          PlayerGamerscore = _parser.PlayersGameSlashedValue(game[3].CollectionValuesHtml!),
          TotalGamerscore = _parser.GameTotalSlashedValue(game[3].CollectionValuesHtml!),
          PlayerAchievementCount = _parser.PlayersGameSlashedValue(game[4].CollectionValuesHtml!),
          TotalAchievementCount = _parser.GameTotalSlashedValue(game[4].CollectionValuesHtml!),
          StartedDate = _parser.TaDate(game[5].CollectionValuesHtml!),
          CompletionDate = _parser.TaDate(game[6].CollectionValuesHtml!),
          LastUnlock = _parser.TaDate(game[7].CollectionValuesHtml!),
          Ownership = _parser.GameOwnership(game[8].CollectionValuesHtml!),
          NotForContests = _parser.GameNotForContests(game[9].CollectionImagesHtml!),
          Publisher = Extensions.NullIfWhiteSpace(game[10].CollectionValuesHtml!),
          Developer = Extensions.NullIfWhiteSpace(game[11].CollectionValuesHtml!),
          ReleaseDate = _parser.TaDate(game[12].CollectionValuesHtml!),
          GamersWithGame = _parser.GamersCount(game[13].CollectionValuesHtml!),
          GamersCompleted = _parser.GamersCount(game[14].CollectionValuesHtml!),
          BaseCompletionEstimate = _parser.BaseGameCompletionEstimate(game[15].CollectionValuesHtml!),
          SiteRatio = _parser.DecimalString(game[16].CollectionValuesHtml!),
          SiteRating = _parser.DecimalString(game[17].CollectionValuesHtml!),
          Unobtainables = _parser.Unobtainables(game[18].CollectionImagesHtml!),
          ServerClosure = _parser.TaDate(game[19].CollectionValuesHtml!),
          InstallSize = _parser.GameSize(game[20].CollectionValuesHtml!),
          FullCompletionEstimate = _parser.FullCompletionEstimate(game[21].CollectionValuesHtml!)
        });
      }
      catch(Exception ex) {
        Console.Error.Write($"Error encountered trying to parse {game[1].GameIdHtml} for Player {player.Name} - ", ex);
      }
    }
  }

  private void SaveNewlyDetectedGames(List<TA_CollectionEntry> incomingData, List<int> taGameIdList) {
    var unknownGames = incomingData.Where(incData => !taGameIdList.Contains(incData.GameId)).ToList();

    foreach(var game in unknownGames) {
      var newGame = new Game {
        TrueAchievementId = game.GameId,
        Title = game.Title,
        Url = game.GameUrl,
        TrueAchievement = game.TotalTrueAchievement,
        Gamerscore = game.TotalGamerscore,
        AchievementCount = game.TotalAchievementCount,
        Publisher = game.Publisher,
        Developer = game.Developer,
        ReleaseDate = game.ReleaseDate,
        GamersWithGame = game.GamersWithGame,
        GamersCompleted = game.GamersCompleted,
        BaseCompletionEstimate = game.BaseCompletionEstimate,
        SiteRatio = game.SiteRatio,
        SiteRating = game.SiteRating,
        Unobtainables = game.Unobtainables,
        ServerClosure = game.ServerClosure,
        InstallSize = game.InstallSize,
        FullCompletionEstimate = game.FullCompletionEstimate
      };

      _context.Games!.Add(newGame);
    }

    // lets save early so we can get our game ID for later inserts
    _context.SaveChanges();
  }

  private void SaveNewlyDetectedCollectionEntries(List<TA_CollectionEntry> incomingData, Player player) {
    // lets figure out and update it if its the first time we see it in the players collection
    var newCollectionEntries = incomingData
                                    .Where(incData => !_context.PlayerGames!.Where(x => x.PlayerId == player.Id)
                                    .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new {pg, g})
                                    .Select(x => x.g.TrueAchievementId).Contains(incData.GameId));

    var gameIds = _context.Games!.Where(x => newCollectionEntries.Select(y => y.GameId).Contains(x.TrueAchievementId)).ToList();
    foreach(var entry in newCollectionEntries) {
      var newGame = new PlayerGame {
        GameId = gameIds.First(x => x.TrueAchievementId == entry.GameId).Id,
        PlayerId = player.Id,
        Platform = entry.Platform,
        TrueAchievement = entry.PlayerTrueAchievement,
        Gamerscore = entry.PlayerGamerscore,
        AchievementCount = entry.PlayerAchievementCount,
        StartedDate = entry.StartedDate,
        CompletionDate = entry.CompletionDate,
        LastUnlock = entry.LastUnlock,
        Ownership = entry.Ownership,
        NotForContests = entry.NotForContests
      };

      _context.PlayerGames!.Add(newGame);
    }
  }

  private void UpdateGameInformation(List<TA_CollectionEntry> incomingData, List<int> taGameIdList) {
    var knownGames = incomingData.Where(incData => taGameIdList.Contains(incData.GameId)).ToList();
    var gamesToUpdate = _context.Games!.Where(x => knownGames.Select(y => y.GameId).Contains(x.TrueAchievementId));

    foreach(var gameToUpdate in gamesToUpdate) {
      var knownGame = knownGames.First(x => x.GameId == gameToUpdate.TrueAchievementId);

      //we'll skip updating things that likely will never change
      //values skipped: TA ID, Title, Publisher, Developer, ReleaseDate, Url
      gameToUpdate.TrueAchievement = knownGame.TotalTrueAchievement;
      gameToUpdate.Gamerscore = knownGame.TotalGamerscore;
      gameToUpdate.AchievementCount = knownGame.TotalAchievementCount;
      gameToUpdate.GamersWithGame = knownGame.GamersWithGame;
      gameToUpdate.GamersCompleted = knownGame.GamersCompleted;
      gameToUpdate.BaseCompletionEstimate = knownGame.BaseCompletionEstimate;
      gameToUpdate.SiteRatio = knownGame.SiteRatio;
      gameToUpdate.SiteRating = knownGame.SiteRating;
      gameToUpdate.Unobtainables = knownGame.Unobtainables;
      gameToUpdate.ServerClosure = knownGame.ServerClosure;
      gameToUpdate.InstallSize = knownGame.InstallSize;
      gameToUpdate.FullCompletionEstimate = knownGame.FullCompletionEstimate;
      gameToUpdate.Url = knownGame.GameUrl;
    }
  }

  private void UpdateCollectionInformation(List<TA_CollectionEntry> incomingData, List<int> taGameIdList, Player player) {
    var knownEntries = incomingData.Where(incData => taGameIdList.Contains(incData.GameId));
    var entriesToUpdate = _context.PlayerGames!.Where(x => x.PlayerId == player.Id)
                              .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new {pg, g})
                              .Where(x => knownEntries.Select(y => y.GameId).Contains(x.g.TrueAchievementId));

    foreach(var entryToUpdate in entriesToUpdate) {
      var knownEntry = knownEntries.First(x => x.GameId == entryToUpdate.g.TrueAchievementId);

      entryToUpdate.pg.Platform = knownEntry.Platform;
      entryToUpdate.pg.TrueAchievement = knownEntry.PlayerTrueAchievement;
      entryToUpdate.pg.Gamerscore = knownEntry.PlayerGamerscore;
      entryToUpdate.pg.AchievementCount = knownEntry.PlayerAchievementCount;
      entryToUpdate.pg.StartedDate = knownEntry.StartedDate;
      entryToUpdate.pg.CompletionDate = knownEntry.CompletionDate;
      entryToUpdate.pg.LastUnlock = knownEntry.LastUnlock;
      entryToUpdate.pg.Ownership = knownEntry.Ownership;
      entryToUpdate.pg.NotForContests = knownEntry.NotForContests;
    }
  }

  public void ParseGamePages(List<int> gamesToUpdateIds) {
    var fls = _context.FeatureLists!.Select(x => x.FeatureListOfGameId);
    var parseList = _context.Games!.Where(x => !fls.Contains(x.Id)).Select(x => x.Id);

    var gamesToUpdate = _context.Games!.Where(x => parseList.Contains(x.Id)).ToList();
    Console.WriteLine($"Parsing {gamesToUpdate.Count()} games at {DateTime.Now}");

    var i = 0;
    foreach(var game in gamesToUpdate) {
      var genresToRemove = _context.GameGenres!.Where(x => x.GameId == game.Id).ToList();
      _context.GameGenres!.RemoveRange(genresToRemove);

      try {
        ParseGamePage(game);  
      }
      catch(Exception ex) {
        Console.WriteLine($"COULD NOT PARSE {game.Title} - {ex}");
      }
      
      Thread.Sleep(2000);
      Console.WriteLine($"Finished parsing {game.Title}, {i++} out of {gamesToUpdate.Count()}");
    }

    _context.SaveChanges();
  }

  private TimeSpan ParseGamePage(Game game) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    using var httpClient = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.trueachievements.com{game.Url}");
    var response = httpClient.Send(request);
    using var reader = new StreamReader(response.Content.ReadAsStream());
    var responseBody = reader.ReadToEnd();

    HtmlDocument doc = new HtmlDocument();
    doc.LoadHtml(responseBody);
    
    var gameInfoTable = doc.DocumentNode.SelectSingleNode("//dl[@class='game-info']");
    var labels = gameInfoTable?.Descendants("dt").Select(x => x.InnerHtml).ToList();
    var values = gameInfoTable?.Descendants("dd").Select(x => x.InnerText).ToList();

    var genres = GetDataFromDLTable("Genre", doc, labels!, values, game);
    var splitGenre = genres!.Split(new string[]{", "}, StringSplitOptions.None);

    foreach(var genre in splitGenre) {
      var typedGenre = GenreList.FromName(genre.Trim());
      
      _context.GameGenres!.Add(new GameGenre {
        GameId = game.Id,
        GenreId = typedGenre
      });
    }

    var features = GetDataFromDLTable("Feature", doc, labels!, values, game);
    var splitFeatures = features?.Split(new string[]{", "}, StringSplitOptions.None);

    
    var featureListToUpdate = _context.FeatureLists!.FirstOrDefault(x => x.Game.Id == game.Id);
    if (featureListToUpdate != null) {
      featureListToUpdate.BackwardsCompat = false;
      featureListToUpdate.CloudGaming = false;
      featureListToUpdate.Crossplay = false;
      featureListToUpdate.EaPlay = false;
      featureListToUpdate.GamePass = false;
      featureListToUpdate.GamePreview = false;
      featureListToUpdate.GamesWithGold = false;
      featureListToUpdate.Hdr = false;
      featureListToUpdate.IdAtXbox = false;
      featureListToUpdate.NotBackwardsCompat = false;
      featureListToUpdate.OneXEnhanced = false;
      featureListToUpdate.OnSteam = false;
      featureListToUpdate.OptimizedForSeries = false;
      featureListToUpdate.PcGamePass = false;
      featureListToUpdate.PlayAnywhere = false;
      featureListToUpdate.SmartDelivery = false;
      featureListToUpdate.xCloudTouch = false;
    } else {
      featureListToUpdate = new FeatureList {
        Game = game
      };
    }

    if (splitFeatures != null) {
      foreach(var feature in splitFeatures) {
        switch(feature) {
          case "Xbox One X Enhanced": 
            featureListToUpdate!.OneXEnhanced = true;
            break;
          case "Backwards Compatible":
            featureListToUpdate!.BackwardsCompat = true;
            break;
          case "Xbox Play Anywhere":
            featureListToUpdate!.PlayAnywhere = true;
            break;
          case "Smart Delivery":
            featureListToUpdate!.SmartDelivery = true;
            break;
          case "Optimized for Series X|S":
            featureListToUpdate!.OptimizedForSeries = true;
            break;
          case "Cross-Play":
            featureListToUpdate!.Crossplay = true;
            break;
          case "HDR":
            featureListToUpdate!.Hdr = true;
            break;
          case "xCloud Touch Controls":
            featureListToUpdate!.xCloudTouch = true;
            break;
          case "Transferable Progress":
            featureListToUpdate!.TransferableProgress = true;
            break;
          default:
            Console.WriteLine($"Feature {feature} was unparsable for game {game.Title}");
            break;
        }
      }
    }

    var notes = GetDataFromDLTable("Note", doc, labels!, values, game);
    var splitNotes = notes?.Split(new string[]{", "}, StringSplitOptions.None);

    if (splitNotes != null) {
      foreach(var note in splitNotes) {
        switch(note) {
          case "Xbox Game Pass": 
            featureListToUpdate!.GamePass = true;
            break;
          case "Xbox Cloud Gaming":
            featureListToUpdate!.CloudGaming = true;
            break;
          case "PC Game Pass":
            featureListToUpdate!.PcGamePass = true;
            break;
          case "EA Play":
            featureListToUpdate!.EaPlay = true;
            break;
          case "Game Preview":
            featureListToUpdate!.GamePreview = true;
            break;
          case "ID@Xbox":
            featureListToUpdate!.IdAtXbox = true;
            break;
          case "Xbox on Steam":
            featureListToUpdate!.OnSteam = true;
            break;
          case "Not Backwards Compatible":
            featureListToUpdate!.NotBackwardsCompat = true;
            break;
          default:
            Console.WriteLine($"Note {note} was unparsable for game {game.Title}");
            break;
        }
      }
    }

    _context.FeatureLists!.Update(featureListToUpdate);

    stopWatch.Stop();

    return stopWatch.Elapsed;
  }

  private string? GetDataFromDLTable(string column, HtmlDocument doc, List<string> columns, List<string>? values, Game game) {
    var columnIndex = columns!.FindIndex(x => x.StartsWith(column));

    if (columnIndex == -1) {
      Console.WriteLine($"{game.Title} did not have \"{column}\" to parse");
      return null;
    }

    return values?[columnIndex];
  }

  public void ParseGamesWithGold(ref int page) {
    using var httpClient = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.trueachievements.com/games-with-gold/games?page={page}");
    var response = httpClient.Send(request);
    using var reader = new StreamReader(response.Content.ReadAsStream());
    var responseBody = reader.ReadToEnd();

    HtmlDocument doc = new HtmlDocument();
    doc.LoadHtml(responseBody);

    var gamesWithGold = doc.DocumentNode.SelectSingleNode("//table[@class='maintable ']")
                            ?.Descendants("td")
                            ?.Where(e => e.HasClass("game"))
                            ?.Select(x => x.InnerText);

    foreach(var goldGame in gamesWithGold!) {
      var game = _context.Games!.FirstOrDefault(x => x.Title == goldGame);

      if (game != null) {
        var featurelist = _context.FeatureLists!.FirstOrDefault(x => x.Game == game);
        if (featurelist != null)
          featurelist.GamesWithGold = true;
        else {
          featurelist = new FeatureList { Game = game, GamesWithGold = true};
        }
      }
    }
    
    if (gamesWithGold?.Count() == 100) {
      page = page + 1;
      ParseGamesWithGold(ref page);
    }
  }
  
  public class CollectionSplit {
    public string? CollectionValuesHtml {get; set;}
    public string? CollectionImagesHtml {get;set;}
    public string? GameIdHtml {get;set;}
  }

  public class TaParseResult {
    public string? Performance {get;set;}
    public int TaHits {get;set;}
    public TimeSpan TimeHittingTa {get;set;}
  }
}