using System.Diagnostics;
using Tavis.Extensions;
using Tavis.Models;
using TavisApi.Context;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using static TavisApi.Services.TA_GameCollection;
using Microsoft.AspNetCore.SignalR;
using DeepEqual.Syntax;
using System.Net;
using dotenv.net;

namespace TavisApi.Services;

public class DataSync : IDataSync
{
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly ITA_GameCollection _taGameCollection;
  private readonly IRgscService _rgscService;

  public DataSync(TavisContext context, IParser parser, ITA_GameCollection taGameCollection, IRgscService rgscService)
  {
    _context = context;
    _parser = parser;
    _taGameCollection = taGameCollection;
    _rgscService = rgscService;
  }

  public object DynamicSync(List<BcmPlayer> players, SyncOptions syncOptions, SyncHistory syncLog, IHubContext<SyncSignal> hub)
  {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    var results = new List<TaParseResult>();

    foreach (var player in players)
    {
      var gamertag = _context.Users.FirstOrDefault(x => x.Id == player.UserId);

      hub.Clients.All.SendAsync("SyncSignal", $"Parsing {gamertag}...");

      var parseStart = DateTime.UtcNow;

      var parsedPlayer = ParseTa(player.Id, syncOptions);
      results.Add(parsedPlayer);
      player.LastSync = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

      var parseEnd = DateTime.UtcNow;

      Console.WriteLine($"{gamertag} has been parsed at {DateTime.UtcNow}");

      var eligRandoms = _rgscService.GetEligibleRandoms(player);
      var rgsc = _context.BcmRgsc.Where(x => x.BcmPlayerId == player.Id).OrderByDescending(x => x.Issued).FirstOrDefault();
      if (rgsc != null)
        rgsc.PoolSize = eligRandoms?.Count() ?? 0;

      _context.SaveChanges();
    }

    var totalHits = 0;
    TimeSpan totalTimeHittingTa = new TimeSpan();
    foreach (var result in results)
    {
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

    return new
    {
      OverallTime = elapsedTime,
      TotalTaHits = totalHits,
      TotalTimeHittingTa = totalTaHitTime,
      PerPlayerTime = results
    };
  }

  // we can maybe skip parsing game info until the last person in the queue to improve perf?
  public TaParseResult ParseTa(long playerId, SyncOptions gcOptions)
  {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    var player = _context.BcmPlayers!.Where(x => x.Id == playerId).First();

    List<List<CollectionSplit>> entireGameList = new List<List<CollectionSplit>>();
    var page = 1;
    var timeHittingTa = ParseCollectionPage(player.TrueAchievementId, entireGameList, gcOptions, ref page);

    var incomingData = new List<TA_CollectionEntry>();
    var taGameIdList = _context.Games!.Select(x => x.TrueAchievementId).ToList();

    StructureCollectionPage(entireGameList, incomingData, player);

    // Games can lose their completion if achievements are added
    // Let's remember when the players originally completed the game
    // commented out because it was no longer used in the no-lock-in bcm
    // and because it was resaving the same completions multiple times...
    // SaveRecompletionHistory(incomingData, taGameIdList, player);

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

    return new TaParseResult
    {
      Performance = elapsedTime,
      TaHits = page,
      TimeHittingTa = timeHittingTa
    };
  }

  private void RemoveGamesFromCollection(List<TA_CollectionEntry> incomingData, BcmPlayer player, SyncOptions gcOptions)
  {
    // Only remove Games from collection if we are doing a full sync
    if (gcOptions.CompletionStatus.Value != SyncOption_CompletionStatus.All.Value ||
      gcOptions.ContestStatus.Value != SyncOption_ContestStatus.All.Value ||
      gcOptions.DateCutoff != null || gcOptions.LastUnlockCutoff != null)
      return;

    // Get all the TA ID's Tavis has for the player
    var gamesInCollection = _context.BcmPlayerGames.Where(x => x.PlayerId == player.Id)
                              .Join(_context.Games, pg => pg.GameId, g => g.Id, (pg, g) => new { pg, g })
                              .Select(x => (int)x.g.TrueAchievementId).ToList();

    // Get all the TA ID's for the newly scanned games
    var incomingGames = incomingData.Select(x => x.GameId).ToList();

    // Find the ID's that are in the collection but not in the incoming data
    // These games have been removed from their collection
    var removedGames = gamesInCollection.Except(incomingGames).ToList();

    // Translate the TA ID of the game to Tavis' Game ID
    var tavisGameIds = _context.Games.Where(x => removedGames.Contains(x.TrueAchievementId)).Select(x => x.Id).ToList();

    // Have the DB forget about the removed games
    foreach (var gameId in tavisGameIds)
    {
      var removedGame = _context.BcmPlayerGames.Where(x => x.GameId == gameId && x.PlayerId == player.Id).First();
      _context.BcmPlayerGames.Remove(removedGame);
    }

    _context.SaveChanges();
  }

  private void SaveRecompletionHistory(List<TA_CollectionEntry> incomingData, List<int> taGameIdList, BcmPlayer player)
  {
    // Get all scanned games that are completed
    var incomingCompletedGames = incomingData.Where(x => x.CompletionDate != null);
    var completedIncomingGames = incomingCompletedGames
                                  .Join(_context.Games, cig => cig.GameId, g => g.TrueAchievementId, (cig, g) => new { cig, g });

    // Get the current completion status of the player's games
    var playersCurrentCompletedGames = _context.BcmPlayerGames.Where(x => x.PlayerId == player.Id && x.CompletionDate != null)
                                                            .OrderByDescending(x => x.CompletionDate);

    // Compare the incoming completions with the completions on the PlayerGames table. If it's different it's a re-completion
    foreach (var incomingCompletion in completedIncomingGames)
    {
      var previouslyCompletedGame = playersCurrentCompletedGames.Where(x => x.Game.TrueAchievementId == incomingCompletion.g.TrueAchievementId).FirstOrDefault();

      if (previouslyCompletedGame != null &&
          incomingCompletion.cig.CompletionDate != previouslyCompletedGame.CompletionDate)
      {
        _context.BcmPlayerCompletionHistory.Add(new BcmPlayerCompletionHistory
        {
          PlayerId = player.Id,
          GameId = incomingCompletion.g.Id,
          CompletionDate = previouslyCompletedGame.CompletionDate ?? DateTime.MinValue
        });
      }
    }

    _context.SaveChanges();
  }

  private TimeSpan ParseCollectionPage(int playerTrueAchId, List<List<CollectionSplit>> entireCollection, SyncOptions gcOptions, ref int page)
  {
    Stopwatch stopWatch = new();
    stopWatch.Start();

    using var httpClientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
    using var httpClient = new HttpClient(httpClientHandler);

    var gameCollectionUrl = _taGameCollection.ParseManager(playerTrueAchId, page, gcOptions);
    HttpRequestMessage request = new(HttpMethod.Get, gameCollectionUrl);

    var cookieContainer = httpClientHandler.CookieContainer;

    //var envVars = DotEnv.Read();
    //var sessionId = envVars.TryGetValue("TA_SESSIONID", out var key) ? key : null;
    //if (sessionId is null || sessionId == "") sessionId = Environment.GetEnvironmentVariable("TA_SESSIONID")!;

    //var authCookie = new Cookie("ASP.NET_SessionId", sessionId)
    //{
    //  Domain = "www.trueachievements.com",
    //  Path = "/"
    //};

    //cookieContainer.Add(authCookie);

    PrepAuthCookies(cookieContainer);

    request.Headers.TryAddWithoutValidation("User-Agent", "Other");

    var response = httpClient.Send(request);
    using StreamReader reader = new(response.Content.ReadAsStream());
    var responseBody = reader.ReadToEnd();

    HtmlDocument doc = new();
    doc.LoadHtml(responseBody);

    if (responseBody == "<div class=\"information\">Please log in or register to view this Gamer's Game Collection.</div>")
      throw new Exception("Bot Logged Out!");

    var collectionPage = doc.DocumentNode.SelectSingleNode("//table")
      ?.Descendants("tr")
      ?.Skip(1)
      ?.SkipLast(1)
      ?.Where(tr => tr.Elements("td").Count() > 1)
      ?.Select(tr => tr.Elements("td")
      ?.Select(td => new CollectionSplit
      {
        CollectionValuesHtml = td.InnerText,
        CollectionImagesHtml = td.InnerHtml,
        GameIdHtml = td.OuterHtml
      })
      ?.ToList());

    if (collectionPage != null)
    {
      entireCollection.AddRange(collectionPage!);

      if (collectionPage.Count() == 100)
      {

        // if we have a cutoff, lets try to quit parsing early
        if (gcOptions.DateCutoff != null || gcOptions.LastUnlockCutoff != null)
        {
          var lastEntry = collectionPage.Last();
          var lastEntryCompletionDate = _parser.TaDate(lastEntry![6].CollectionValuesHtml!);
          var lastUnlockDate = _parser.TaDate(lastEntry![7].CollectionValuesHtml);

          if (lastUnlockDate < gcOptions.LastUnlockCutoff)
          {
            stopWatch.Stop();
            return stopWatch.Elapsed;
          }

          if (lastEntryCompletionDate < gcOptions.DateCutoff)
          {
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

  private void StructureCollectionPage(List<List<CollectionSplit>> entireGameList, List<TA_CollectionEntry> incomingData, BcmPlayer player)
  {
    // we're going to hardcode the array position here to avoid even more parsing
    //TODO: this deserves an integration test
    //TODO: consider breaking out what we parse if we already have the game on file?
    // maybe we shouldn't parse things that never change, like Dev, Publisher etc
    // if we do that, have a way to force an update
    foreach (var game in entireGameList)
    {
      try
      {
        incomingData.Add(new TA_CollectionEntry
        {
          GameId = _parser.GameId(game[1].GameIdHtml),
          Title = Extensions.NullIfWhiteSpace(game[0].CollectionValuesHtml),
          GameUrl = _parser.GameUrl(game[0].CollectionImagesHtml),
          Platform = _parser.GamePlatform(game[1].CollectionImagesHtml),
          PlayerTrueAchievement = _parser.PlayersGameSlashedValue(game[2].CollectionValuesHtml),
          TotalTrueAchievement = _parser.GameTotalSlashedValue(game[2].CollectionValuesHtml),
          PlayerGamerscore = _parser.PlayersGameSlashedValue(game[3].CollectionValuesHtml),
          TotalGamerscore = _parser.GameTotalSlashedValue(game[3].CollectionValuesHtml),
          PlayerAchievementCount = _parser.PlayersGameSlashedValue(game[4].CollectionValuesHtml),
          TotalAchievementCount = _parser.GameTotalSlashedValue(game[4].CollectionValuesHtml),
          StartedDate = _parser.TaDate(game[5].CollectionValuesHtml),
          CompletionDate = _parser.TaDate(game[6].CollectionValuesHtml),
          LastUnlock = _parser.TaDate(game[7].CollectionValuesHtml),
          Ownership = _parser.GameOwnership(game[8].CollectionValuesHtml),
          NotForContests = _parser.GameNotForContests(game[9].CollectionImagesHtml),
          Publisher = Extensions.NullIfWhiteSpace(game[10].CollectionValuesHtml),
          Developer = Extensions.NullIfWhiteSpace(game[11].CollectionValuesHtml),
          ReleaseDate = _parser.TaDate(game[12].CollectionValuesHtml),
          GamersWithGame = _parser.GamersCount(game[13].CollectionValuesHtml),
          GamersCompleted = _parser.GamersCount(game[14].CollectionValuesHtml),
          BaseCompletionEstimate = _parser.BaseGameCompletionEstimate(game[15].CollectionValuesHtml),
          SiteRatio = _parser.DecimalString(game[16].CollectionValuesHtml),
          SiteRating = _parser.DecimalString(game[17].CollectionValuesHtml),
          Unobtainables = _parser.Unobtainables(game[18].CollectionImagesHtml),
          ServerClosure = _parser.TaDate(game[19].CollectionValuesHtml),
          InstallSize = _parser.GameSize(game[20].CollectionValuesHtml),
          FullCompletionEstimate = _parser.FullCompletionEstimate(game[21].CollectionValuesHtml)
        });
      }
      catch (Exception ex)
      {
        Console.Error.Write($"Error encountered trying to parse {game[1].GameIdHtml} for Player {player.Id} - ", ex);
      }
    }
  }

  private void SaveNewlyDetectedGames(List<TA_CollectionEntry> incomingData, List<int> taGameIdList)
  {
    var unknownGames = incomingData.Where(incData => !taGameIdList.Contains(incData.GameId)).ToList();

    foreach (var game in unknownGames)
    {
      var newGame = new Game
      {
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

      GetXboxApiInfo(newGame);
      _context.Games!.Add(newGame);
    }

    // lets save early so we can get our game ID for later inserts
    _context.SaveChanges();
  }

  private void GetXboxApiInfo(Game game)
  {
    // make the call to `{{baseUrl}}/api/v2/achievements/player/:xuid` to get achievement list
    // in that achievement list, there is an array of titles
    // use the game.Title to match on the xbl:title.name
    // if it doesn't match, just move on
    // save the titleId to the Game table
    // save the displayImage url
  }

  private void SaveNewlyDetectedCollectionEntries(List<TA_CollectionEntry> incomingData, BcmPlayer player)
  {
    // lets figure out and update it if its the first time we see it in the players collection
    var newCollectionEntries = incomingData
                                    .Where(incData => !_context.BcmPlayerGames!.Where(x => x.PlayerId == player.Id)
                                    .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { pg, g })
                                    .Select(x => x.g.TrueAchievementId).Contains(incData.GameId));

    var gameIds = _context.Games!.Where(x => newCollectionEntries.Select(y => y.GameId).Contains(x.TrueAchievementId)).ToList();
    foreach (var entry in newCollectionEntries)
    {
      var newGame = new BcmPlayerGame
      {
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

      _context.BcmPlayerGames!.Add(newGame);
    }
  }

  private void UpdateGameInformation(List<TA_CollectionEntry> incomingData, List<int> taGameIdList)
  {
    var incomingGames = incomingData.Where(incData => taGameIdList.Contains(incData.GameId)).ToList();
    var gamesToUpdate = _context.Games!.Where(x => incomingGames.Select(y => y.GameId).Contains(x.TrueAchievementId));

    foreach (var gameToUpdate in gamesToUpdate)
    {
      var incomingGame = incomingGames.First(x => x.GameId == gameToUpdate.TrueAchievementId);

      gameToUpdate.Publisher = incomingGame.Publisher;
      gameToUpdate.Developer = incomingGame.Developer;
      gameToUpdate.ReleaseDate = incomingGame.ReleaseDate;
      gameToUpdate.Title = incomingGame.Title;
      gameToUpdate.TrueAchievement = incomingGame.TotalTrueAchievement;
      gameToUpdate.Gamerscore = incomingGame.TotalGamerscore;
      gameToUpdate.AchievementCount = incomingGame.TotalAchievementCount;
      gameToUpdate.GamersWithGame = incomingGame.GamersWithGame;
      gameToUpdate.GamersCompleted = incomingGame.GamersCompleted;
      gameToUpdate.BaseCompletionEstimate = incomingGame.BaseCompletionEstimate;
      gameToUpdate.SiteRatio = incomingGame.SiteRatio;
      gameToUpdate.SiteRating = incomingGame.SiteRating;
      gameToUpdate.Unobtainables = incomingGame.Unobtainables;
      gameToUpdate.ServerClosure = incomingGame.ServerClosure;
      gameToUpdate.InstallSize = incomingGame.InstallSize;
      gameToUpdate.FullCompletionEstimate = DetermineCompletionEstimate(gameToUpdate, incomingGame);
      gameToUpdate.Url = incomingGame.GameUrl;
    }
  }

  public double? DetermineCompletionEstimate(Game tavisGame, TA_CollectionEntry scannedGame)
  {
    // always use the TA estimate if it's not empty
    if (scannedGame.FullCompletionEstimate != null)
    {
      tavisGame.ManuallyScored = false;
      return scannedGame.FullCompletionEstimate;
    }

    // if a game _seems_ like its a base game, use the base completion estimate
    if ((scannedGame.TotalGamerscore == 1000 || scannedGame.TotalGamerscore == 400 || scannedGame.TotalGamerscore == 200) && scannedGame.BaseCompletionEstimate != -1)
      return scannedGame.BaseCompletionEstimate;

    // otherwise return nothing to be manually scored, unless it's already been manually scored
    return tavisGame.ManuallyScored
        ? tavisGame.FullCompletionEstimate
        : null;
  }

  private void UpdateCollectionInformation(List<TA_CollectionEntry> incomingData, List<int> taGameIdList, BcmPlayer player)
  {
    var knownEntries = incomingData.Where(incData => taGameIdList.Contains(incData.GameId));
    var entriesToUpdate = _context.BcmPlayerGames!.Where(x => x.PlayerId == player.Id)
                              .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { pg, g })
                              .Where(x => knownEntries.Select(y => y.GameId).Contains(x.g.TrueAchievementId));

    foreach (var entryToUpdate in entriesToUpdate)
    {
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

  public void ParseGamePages(List<int> gamesToUpdateIds)
  {
    var gamesToUpdate = _context.Games!.Where(x => gamesToUpdateIds.Contains(x.Id)).ToList();
    Console.WriteLine($"Parsing {gamesToUpdate.Count()} games at {DateTime.UtcNow}");

    var i = 0;
    foreach (var game in gamesToUpdate)
    {
      var genresToRemove = _context.GameGenres!.Where(x => x.GameId == game.Id).ToList();
      _context.GameGenres!.RemoveRange(genresToRemove);

      try
      {
        ParseGamePage(game);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"COULD NOT PARSE {game.Title} - {ex}");
      }

      Thread.Sleep(2000);
      Console.WriteLine($"Finished parsing {game.Title}, {i++} out of {gamesToUpdate.Count()}");
    }

    try
    {
      _context.SaveChanges();
    }
    catch (Exception ex)
    {
      var derp = ex;
    }
  }

  private TimeSpan ParseGamePage(Game game)
  {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    // using var httpClient = new HttpClient();
    // var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.trueachievements.com{game.Url}");

    using var httpClientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
    using var httpClient = new HttpClient(httpClientHandler);

    HttpRequestMessage request = new(HttpMethod.Get, $"https://www.trueachievements.com{game.Url}");

    var cookieContainer = httpClientHandler.CookieContainer;

    //var envVars = DotEnv.Read();
    //var sessionId = envVars.TryGetValue("TA_SESSIONID", out var key) ? key : null;
    //if (sessionId is null || sessionId == "") sessionId = Environment.GetEnvironmentVariable("TA_SESSIONID")!;

    //var authCookie = new Cookie("ASP.NET_SessionId", sessionId)
    //{
    //  Domain = "www.trueachievements.com",
    //  Path = "/"
    //};

    //cookieContainer.Add(authCookie);

    PrepAuthCookies(cookieContainer);

    request.Headers.TryAddWithoutValidation("User-Agent", "Other");

    var response = httpClient.Send(request);

    using var reader = new StreamReader(response.Content.ReadAsStream());
    var responseBody = reader.ReadToEnd();

    HtmlDocument doc = new HtmlDocument();
    doc.LoadHtml(responseBody);

    var gameInfoTable = doc.DocumentNode.SelectSingleNode("//dl[@class='game-info']");
    var labels = gameInfoTable?.Descendants("dt").Select(x => x.InnerHtml).ToList();
    var values = gameInfoTable?.Descendants("dd").Select(x => x.InnerText).ToList();

    var genres = GetDataFromDLTable("Genre", doc, labels, values, game);

    if (genres == null)
    {
      _context.GameGenres!.Add(new GameGenre
      {
        GameId = game.Id,
        GenreId = GenreList.None,
        LastSync = DateTime.UtcNow
      });
    }
    else
    {
      var splitGenre = genres!.Split(new string[] { ", " }, StringSplitOptions.None);

      foreach (var genre in splitGenre)
      {
        var typedGenre = GenreList.FromName(genre.Trim());

        _context.GameGenres!.Add(new GameGenre
        {
          GameId = game.Id,
          GenreId = typedGenre,
          LastSync = DateTime.UtcNow
        });
      }
    }

    var features = GetDataFromDLTable("Feature", doc, labels!, values, game);
    var splitFeatures = features?.Split(new string[] { ", " }, StringSplitOptions.None);


    var featureListToUpdate = _context.FeatureLists!.FirstOrDefault(x => x.Game.Id == game.Id);
    if (featureListToUpdate != null)
    {
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
    }
    else
    {
      featureListToUpdate = new FeatureList
      {
        Game = game
      };
    }

    if (splitFeatures != null)
    {
      foreach (var feature in splitFeatures)
      {
        switch (feature)
        {
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
    var splitNotes = notes?.Split(new string[] { ", " }, StringSplitOptions.None);

    if (splitNotes != null)
    {
      foreach (var note in splitNotes)
      {
        switch (note)
        {
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

  private string? GetDataFromDLTable(string column, HtmlDocument doc, List<string>? columns, List<string>? values, Game game)
  {
    var columnIndex = columns?.FindIndex(x => x.StartsWith(column));

    if (columnIndex == -1 || columnIndex == null)
    {
      Console.WriteLine($"{game.Title} did not have \"{column}\" to parse");
      return null;
    }

    return values?[columnIndex.Value];
  }

  public void ParseGamesWithGold(ref int page)
  {
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

    foreach (var goldGame in gamesWithGold!)
    {
      var game = _context.Games!.FirstOrDefault(x => x.Title == goldGame);

      if (game != null)
      {
        var featurelist = _context.FeatureLists!.FirstOrDefault(x => x.Game == game);
        if (featurelist != null)
          featurelist.GamesWithGold = true;
        else
        {
          featurelist = new FeatureList { Game = game, GamesWithGold = true };
        }
      }
    }

    if (gamesWithGold?.Count() == 100)
    {
      page = page + 1;
      ParseGamesWithGold(ref page);
    }
  }

  private void PrepAuthCookies(CookieContainer cookieJar)
  {
    var externalCookie = new Cookie(".AspNet.ExternalCookie", "_y-I3fGmosR8k6ZAgyoF2vuN5krJGWuMcpGPSnH9l0XHFhqEgr95NB0W4pFcjoN7wG3nyOgUCpIUcN5N7i9qa_OLfZ-VHpNQH7gC7Zh3sDAefeJncIwOnBdMHm2-OzxYL5Iov7gGKHIPuoQ54WYhQG4y9FaD02MYh0ytcvLHry0u1XEOn4iGPsYCmnYdu9GPPDLTnD1YtO0c1CSSrHY2CwDCT11vvY2Oq4Mc29QcnoBaV4ANUT4JMhmZrZHjZxkGJ06yuaEvuV_aePAGY0YhuTymqLUJ2kaqqd4zENfoHD-8ZEoovuRTpjlgfGFn4eYNSO0o5_ud7bI7soD4yrOnwpx3Szn_OLhMyf5-g1WEJqwbYyFJzrqAqm9FuW5bTw9ITxU0rAnXKNfE-9wdo3AzHAIdZB6ed9n3FGbM1LMpPuFjF1dNXFxHy-GwaVctTnWzs8AKlXsLP3ul0Rm55rVobgPffehnQgnBMgiemnNSOomPtfCpXZM8nKmicyPMUBLZnHTel6z4HwWHx9JP7qigAp3M7LDQ1j8kql74lsVJeTKY3c0FvrAehqh4JN6nqjrxe0GDoai06ph8XmKpsPnfQ3blVzysM1bnCTcdheUQNUVMi5UQgEYhq9Utf7lptURyneakwXjGbhnbTbnbwDjwJ67vt3IGSRSs5l0TxD2EWzorPIg65xnhu7zUT1dm0BCYLHvSYdpSXXn6RIhOLcwiQ1ojB-KkIn0nKpSQb4E_-xfRnmavnEgOZrnusMojdgStqXvnGXCD6ZSIV-Ei1rd93M8F8HqjXpy6YeltX8vzxuTRk8QEIg1OPVM7u2jZKBJXRsbaoj0nTY_-5_diwt7cukhXfLzLKMEPrRhjonhJzquJRzXaa6tXbbUH_cZBBqYH14mdBqs_yTNBhTjZUIDtkBsSsQ-bQo29yaTC-6-kHoU34SPXTHIzcjor80znFfZ6LyDm6AlsEkGQLJhRfha6tnzA5iHgZjkCdbJ_jU1LXVAj0O2Qa6iE3tflNwzzyokCgZ7m9kVwjBZ_DB3avm0DvfLAY1RRahsM5PWuT-Ca64E_-0d1gm-zzpcpKE1uMpX3e8mJC5T71t6usxOmiPiXCRBszd1cQWh74iN0cXYObkcwNgIpgBI_BWFn13pxxM5rUXN2Vr1J2OpFwGCoNSdrPWHadFRV71PY5pXfeHJM-FM8J3bFFtSYk0Q4x3FTH4MvB-NlK4dcFfVm_fUOXY4OyIZSw5nwT-1GqWGJGwYon0-G2HACSElYo2Hi3K5zZpaXacGTZJefU8eYCzJaOOlFV1PVMOQMUfVt-q3dVprbVuV8B8dE3VOpS_1i2vr2ZEhviUXxsPa4f6yeQC2dM1GRWBOl9pjZgnY9Qe9XVDV6c8Q58EiW35mfazh2VsabXJ75tD0bykrdumwjPHOewPioxBneN8Y4OWv5DpZ9tr1opfrC5gSsAifelhL6kuO4_m1m4fd8crTSjNNjprNowek1D6Wvg79Jq-3dtoYU_J8sF2b0Gh21-f03pxS6qQAO2u0q3Z8fFGT0oiIm4U94N_jC7HPLBzHP2bFnzVb-tNJA63HHbU4v0P2APHWDkJOnXbJ1dEhIRE5AtfbccngBkv1g-OwJ7yJw4dJx6kYSzqy7jpiNb7BBBwgNS2HsydZydA7OPmKbstnH1XfzPmKwkuRuNsdwydrALh_XRUN-5sQKQ-1ApBCLHvKP2oGdmnTJAf5p6W1sEYJ8dCd51yUriAxpMNGAvtkdjiM5ibik_WhNJLtfE5TRKZ0rbFirRaDvSoR7t5gNwcPAs1WorUQICg4XMgYjpLJ0AxOUESauhwgTDXprsFVhqsawfrNv45wMC6V5pJLg3nUv-r_860p4ob0g-tAur0J604gAL-5utYHdxkL_L6SaFqeEgiAbh5Id1jK0wVEauXhOFSa63nGAkqosYTOYlHldTFocJM3dwLJOk66l5SgL49Qb2vvFqtLYo9OreUhFGtyCk2H_62GIOWGj6wZe0zk4RMsG_x40DBIzfNYFZQLsWWi2tFcCBjYY9o74rw7Md72lk9mZ5byFL3bbwsFslOanrf2rNkH0Epb9mqS6LhKpbbCf8P1bGt2pLaHRqLHULbYdsvn3Bdc_aYQMfSZmjPBg2UO2sgfBwGYvjvFz4yNTFs76wHSRnqH5iPo_2-IvlU0znm_gm6zRIhIdf28wMoVm21jhaPIPKMnQZ5tsGH0cq2mBnj01WcuY8-oJTLADYck8lUGNrXbboBYXH0ZTJ8cNZB4kDxG1kb2mr-wH2zk-cr55zEYDHGruwadq9tTk1aBCTqyjwZ8ccwr1a_lR_Vd3OIAqqlaQzqs-yWM05dhm4Z1uRvw_1K0IKLOPgLGUphi0M5lYK2YrNC1ExbenDlXt7exxyDaDfbRUY-cYPg70eCDXX6GlN4CBMu5GCuYGsi02hNeZUe2HN5FfBrN0RVC_yGD7tkTbVvOxfV2vi75OJmFVmm_6wwOsUOEQxiJ-VCivM3Vq_vRQWP6BY0_wgfyK0cABhCwjSmWKfMyDKeQFM1wB7xagw7TuDbjImWsEOo6KrTfgfI7cSvkE68_QPbI6dOcdaL7RImBjCloFbrPZbeRzhQrNGCOSNQCKpovTs1n6ZIw_rlC_Z1niPA95jLOnmI6LNVtNq0mMXx32Tq79D4LxkHTJEsciQSsNUHRFwPECgD0_7C_cB5tum5GcJlyrEEdsvhi93PiaS8qW2ZaUL44C7XCDEyjw91FhVi4p1ykkl49HEzutKrwI9gircSj4hNDc4mhAaP0vTIj09O3pBJ4tq8vS7Ppd7-qAZCVJIv99bN65IemhSIS-sWOAu4YRTa8uPeIjWfh0Lec8azmZwDTplmeRGexEuuy3xMUf5bK3SB9yfKwTr4SvQuczk10iLCw0DeLdq8Var4rWzi-CanNUZnJgcmMtjGOkrETUo4gPRdQcrsuKhldicft7jou7KMjR6nHPFzUFrUc0G0vAFE-D6lluHMRIPF9reH3YYZMvgemzwZ7YvFREV0AheBuacw-iAEEAsTB2U9YZKCsswjRjrXWG77qyH4ihoiGn0ivo_WOOvRKL2mZwYSPWocPUOPfM2H4TYKgqNsv5C7dYptn79P_D7_6BKd6OEbIWZoT_cXOUS_GtJBc5VlTHJGUGAbcGeeIGrJ3OZnLWXzKb")
    {
      Domain = "www.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(externalCookie);

    var sessionCookie = new Cookie("ASP.NET_SessionId", "su5zepyrisupiulijb2251v1")
    {
      Domain = "www.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(sessionCookie);

    var logglyCookie = new Cookie("logglytrackingsession", "636a67d2-686f-44b5-a707-100bc8cdb961")
    {
      Domain = "www.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(logglyCookie);

    var boncedCookie = new Cookie("bounceClientVisit6884v", "N4IgNgDiBcIBYBcEQM4FIDMBBNAmAYnvgO6kB0CATgK4CmAhgMZwCWtAbrQLa0B2CKMowD2XIiAA0ISjBCSQLFAH0A5sKUpaKFC2G8YAM3phNUxaogatOvYeOaAvkA")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(boncedCookie);

    var gidCookie = new Cookie("_gid", "GA1.2.78795632.1708152331")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gidCookie);

    var panIdCookie = new Cookie("panoramaId", "0289662f7fa1e525745e7c7c8863185ca02c189769550ce82eab4b835d0eefda")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(panIdCookie);

    var typeCookie = new Cookie("panoramaIdType", "panoDevice")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(typeCookie);

    var panoCookie = new Cookie("panoramaId_expiry", "1708757127269")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(panoCookie);

    var pbjsCookie = new Cookie("_pbjs_userid_consent_data", "6683316680106290")
    {
      Domain = "www.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(pbjsCookie);

    var eoiCookie = new Cookie("__eoi", "ID=d531edff929d82fd:T=1708152328:RT=1708152328:S=AA-AfjajEUZT8vFBlPoj44G05MT3")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(eoiCookie);

    var ccidCookie = new Cookie("_cc_id", "e14cdc95c8ef61ab5e1b27b208877482")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(ccidCookie);

    var sharedCookie = new Cookie("_sharedid", "6c113ef1-6b89-4c26-a32d-e643e6eeaba1")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(sharedCookie);

    var dnsCookie = new Cookie("dnsDisplayed", "undefined")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(dnsCookie);

    var ccpaCookie = new Cookie("ccpaApplies", "false")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(ccpaCookie);

    var signedCookie = new Cookie("signedLspa", "undefined")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(signedCookie);

    var spCookie = new Cookie("_sp_su", "false")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(spCookie);

    var uuidCookie = new Cookie("ccpaUUID", "ababa9db-29a1-45da-b93d-ff5de5074099")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(uuidCookie);

    var bounceCookie = new Cookie("bounceClientVisit6884", "N4IgJglmIFwgHARgOyIGxoKwCZ4AZsBmPZNRRQ7EAGhADcpYU8kdL41C15M9aAzOgBcmyFoja5aAQwD2sPiAA2AB1ggAFkKEqAzgFJCAQX3YAYqbMB3GwDohAJwCuAU2kBjDRBd0XAWxcAOyFdW3dZP0saEGldBVo6FTomWmg4aFoHdWj3YVFxSXhaXUYYRFoAc3cssoBOWj945TyysVYiXABfIA")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(bounceCookie);

    var auCookie = new Cookie("_au_1d", "AU1D-0100-001708152331-PBD6M4B7-TKKX")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(auCookie);

    var auLastCookie = new Cookie("_au_last_seen_pixels", "eyJhcG4iOjE3MDgxNTIzMzEsInR0ZCI6MTcwODE1MjMzMSwicHViIjoxNzA4MTUyMzMxLCJydWIiOjE3MDgxNTIzMzEsInRhcGFkIjoxNzA4MTUyMzMxLCJhZHgiOjE3MDgxNTIzMzEsImdvbyI6MTcwODE1MjMzMSwiaW5kZXgiOjE3MDgxNTIzMzEsIm9wZW54IjoxNzA4MTUyMzMxLCJzb24iOjE3MDgxNTIzMzF9")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(auLastCookie);

    var gamerCookie = new Cookie("GamerID", "1480739")
    {
      Domain = "wwww.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gamerCookie);

    var hashCookie = new Cookie("HashKey", "cb0c9eed21c343ffa7f3550f57110571")
    {
      Domain = "wwww.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(hashCookie);

    var tpGamerCookie = new Cookie("TPGamerID", "1480739")
    {
      Domain = "wwww.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(tpGamerCookie);

    var gamerTokenCookie = new Cookie("GamerToken", "cb0c9eed21c343ffa7f3550f57110571_FC3893F59C9C4B36AC02D71A0A2A63BB")
    {
      Domain = "wwww.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gamerTokenCookie);

    var tgIdentityCookie = new Cookie("TrueGamingIdentity", "2YZaaXfJmBeGA13JfaTG50Rly0vLPAK_4-6aUDTMs4eexwVjlTlG-tWrVU781eqJWItSPKEFyT4zfVNQlIXF0UTMYDOI2musiEEhcYBrQFQoQlVh2Tx_a4hYteBWl0JAvUAru1HBJdS7BdZPVU7Tenbezs3436_CibTZcC_IdGjlgw21KgYkV6JTkA0nliWLd6jBr9BPC6BzkQtskCWr41vWEWFj56WkS01UezCBxop3VIZ9Red_AAwLUibdwI_8XR42XRpdtDHPWYxj9MnwAYWtgo4Vh_Za4SHTkTPSv324Ji2V8NG-i2KfcCkot0h0ysT7QR0l7LQHD9cpCefb8rntdZQWAMsBTa1yCX47INk2uHhm")
    {
      Domain = "wwww.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(tgIdentityCookie);

    var langCookie = new Cookie("LanguageID", "0")
    {
      Domain = "wwww.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(langCookie);

    var gadsCookie = new Cookie("__gads", "ID=cbb6253d87801b1b:T=1708152328:RT=1708152328:S=ALNI_MaQ7RCNZlMCqk_Tj98-rD5JKZH5lw")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gadsCookie);

    var gpiCookie = new Cookie("__gpi", "UID=00000dc1d5e677c6:T=1708152328:RT=1708152328:S=ALNI_Mapkt9u07bzkaXqZK42mjchyEW4Rw")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gpiCookie);

    var regionCookie = new Cookie("ContentRecordRegionID", "1")
    {
      Domain = "www.trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(regionCookie);

    var gaCookie = new Cookie("_ga", "GA1.2.1288830505.1708152329")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gaCookie);

    var gaextCookie = new Cookie("_ga_0CPE0JFSCT", "GS1.1.1708152329.1.0.1708152331.0.0.0")
    {
      Domain = ".trueachievements.com",
      Path = "/"
    };

    cookieJar.Add(gaextCookie);
  }

  public class CollectionSplit
  {
    public string? CollectionValuesHtml { get; set; }
    public string? CollectionImagesHtml { get; set; }
    public string? GameIdHtml { get; set; }
  }

  public class TaParseResult
  {
    public string? Performance { get; set; }
    public int TaHits { get; set; }
    public TimeSpan TimeHittingTa { get; set; }
  }
}
