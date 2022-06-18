using System.Diagnostics;
using Bcm.Extensions;
using Bcm.Models;
using BcmApi.Context;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using static BcmApi.Services.DataSync;

namespace BcmApi.Services;

public interface IDataSync {
  TaParseResult ParseTa(int playerId);
  void ParseGamePages(List<int> gamesToUpdateIds);
  void ParseGamesWithGold(ref int page);
}

public class DataSync : IDataSync {

  private BcmContext _context;
  private readonly IParser _parser;

  public DataSync(BcmContext context, IParser parser) {
    _context = context;
    _parser = parser;
  }

  public TaParseResult ParseTa(int playerId) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    var player = _context.Players!.Where(x => x.Id == Convert.ToInt32(playerId)).First();

    List<List<CollectionSplit>> entireGameList = new List<List<CollectionSplit>>();
    var page = 1;
    var timeHittingTa = ParseCollectionPage(player.TrueAchievementId, entireGameList, ref page);
    
    var incomingData = new List<TA_CollectionEntry>();
    var taGameIdList = _context.Games!.Select(x => x.TrueAchievementId).ToList();

    StructureCollectionPage(entireGameList, incomingData, player);

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

  private TimeSpan ParseCollectionPage(int playerTrueAchId, List<List<CollectionSplit>> entireCollection, ref int page) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    using var httpClient = new HttpClient();
    //https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7CoGameCollection_TimeZone=Eastern%20Standard%20Time%26txtGamerID%3D104571%26ddlSortBy%3DTitlename%26ddlDLCInclusionSetting%3DAllDLC%26ddlCompletionStatus%3DAll%26ddlTitleType%3DGame%26ddlContestStatus%3DAll%26asdGamePropertyID%3D-1%26oGameCollection_Order%3DDatecompleted%26oGameCollection_Page%3D1%26oGameCollection_ItemsPerPage%3D10000%26oGameCollection_ShowAll%3DFalse%26txtGameRegionID%3D2%26GameView%3DoptListView%26chkColTitlename%3DTrue%26chkColCompletionestincDLC%3DTrue%26chkColUnobtainables%3DTrue%26chkColSiteratio%3DTrue%26chkColPlatform%3DTrue%26chkColServerclosure%3DTrue%26chkColNotNotForContests%3DTrue%26chkColSitescore%3DTrue%26chkColOfficialScore%3DTrue%26chkColItems%3DTrue%26chkColDatestarted%3DTrue%26chkColDatecompleted%3DTrue%26chkColLastunlock%3DTrue%26chkColOwnershipstatus%3DTrue%26chkColPublisher%3DTrue%26chkColDeveloper%3DTrue%26chkColReleasedate%3DTrue%26chkColGamerswithgame%3DTrue%26chkColGamerscompleted%3DTrue%26chkColGamerscompletedperentage%3DTrue%26chkColCompletionestimate%3DTrue%26chkColSiterating%3DTrue%26chkColNotforcontests%3DTrue%26chkColInstallsize%3DTrue
    var request = new HttpRequestMessage(HttpMethod.Get, 
      "https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7Co" +
      "GameCollection_TimeZone=Eastern%20Standard%20Time" +
      $"%26txtGamerID%3D{playerTrueAchId}" +
      "%26ddlSortBy%3DTitlename" +
      "%26ddlDLCInclusionSetting%3DAllDLC" +
      "%26ddlCompletionStatus%3DAll" +
      "%26ddlTitleType%3DGame" +
      "%26ddlContestStatus%3DAll" +
      "%26asdGamePropertyID%3D-1" +
      "%26oGameCollection_Order%3DDatecompleted" +
      $"%26oGameCollection_Page%3D{page}" +
      "%26oGameCollection_ItemsPerPage%3D10000" +
      "%26oGameCollection_ShowAll%3DFalse" +
      "%26txtGameRegionID%3D2" +
      "%26GameView%3DoptListView" +
      "%26chkColTitlename%3DTrue" +
      "%26chkColCompletionestincDLC%3DTrue" +
      "%26chkColUnobtainables%3DTrue" +
      "%26chkColSiteratio%3DTrue" +
      "%26chkColPlatform%3DTrue" +
      "%26chkColServerclosure%3DTrue" +
      "%26chkColNotNotForContests%3DTrue" +
      "%26chkColSitescore%3DTrue" +
      "%26chkColOfficialScore%3DTrue" +
      "%26chkColItems%3DTrue" +
      "%26chkColDatestarted%3DTrue" +
      "%26chkColDatecompleted%3DTrue" +
      "%26chkColLastunlock%3DTrue" +
      "%26chkColOwnershipstatus%3DTrue" +
      "%26chkColPublisher%3DTrue" +
      "%26chkColDeveloper%3DTrue" +
      "%26chkColReleasedate%3DTrue" +
      "%26chkColGamerswithgame%3DTrue" +
      "%26chkColGamerscompleted%3DTrue" +
      "%26chkColGamerscompletedperentage%3DTrue" +
      "%26chkColCompletionestimate%3DTrue" +
      "%26chkColSiterating%3DTrue" +
      "%26chkColNotforcontests%3DTrue" +
      "%26chkColInstallsize%3DTrue");
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
        page++;
        ParseCollectionPage(playerTrueAchId, entireCollection, ref page);
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
    var unknownGames = incomingData.Where(incData => !taGameIdList.Contains(incData.GameId));

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
    //TODO: long term lets not hardcode ids mmkay
    var testingIds = new List<int>();
    testingIds.Add(1); testingIds.Add(2); testingIds.Add(3);
    testingIds.Add(4); testingIds.Add(5); testingIds.Add(6);
   
    var gamesToUpdate = _context.Games!.Where(x => testingIds.Contains(x.Id));

    foreach(var game in gamesToUpdate) {
      var genresToRemove = _context.GameGenres!.Where(x => x.GameId == game.Id);
      _context.GameGenres!.RemoveRange(genresToRemove);

      ParseGamePage(game);
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