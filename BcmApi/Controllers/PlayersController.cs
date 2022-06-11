using System.Diagnostics;
using System.Net;
using BcmApi.Context;
using Bcm.Models;
using BcmApi.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Bcm.Extensions;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase 
{
  private BcmContext _context;
  private Parser _parse = new Parser();

  public PlayersController(BcmContext context) {
    _context = context;
  }

  [HttpGet]
  [Route("getAll")]
  public IActionResult GetAll()
  {
    var players = _context.Players?.Where(x => x.Name != null);
    return Ok(players);
  }

  [HttpGet]
  [Route("getPlayersGames")]
  public IActionResult GetPlayersGames(int playerId)
  {
    if (playerId == 0) return BadRequest();

    var completedGames = _context.Players!
        .Join(_context.PlayersGames, player => player.Id, complGame => complGame.PlayerId, 
            (player, complGame) => new { Player = player, ComplGame = complGame})
        .Where(x => x.Player.Id == playerId)
        .Join(_context.Games!, complGame => complGame.ComplGame.GameId, game => game.Id,
            (complGame, game) => new {ComplGame = complGame, Game = game})
        .Select(x => new {
          Title = x.Game.Title,
          Ratio = x.Game.SiteRatio,
          Time = x.Game.FullCompletionEstimate
        });

    return Ok(completedGames);
  }

  [HttpGet]
  [Route("retrieveCompletedGames")]
  public IActionResult RetrieveCompletedGames(int playerId) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    // ScanMan is an alt account that was autoadding all games to collection
    // useful for benchmarking, has over 7,000 games on the profile 
    var zzScanMan = 157;
    var players = _context.Players!.Where(x => x.Id != zzScanMan).ToList();

    var results = new List<TaParseResult>();

    foreach(var player in players) {
      var parsedPlayer = ParseTa(player.Id);
      results.Add(parsedPlayer);
      Console.WriteLine($"Player {player.Name} has been parsed with a processing time of {parsedPlayer.Performance}");
    }

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

    return Ok(new {
      OverallTime = elapsedTime,
      TotalTaHits = totalHits,
      TotalTimeHittingTa = totalTaHitTime,
      PerPlayerTime = results
    });
  }

  private TaParseResult ParseTa(int playerId) {
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

  public class TaParseResult {
    public string? Performance {get;set;}
    public int TaHits {get;set;}
    public TimeSpan TimeHittingTa {get;set;}
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
          GameId = _parse.GameId(game[1].GameIdHtml!),
          Title = Extensions.NullIfWhiteSpace(game[0].CollectionValuesHtml!),
          Platform = _parse.GamePlatform(game[1].CollectionImagesHtml!),
          PlayerTrueAchievement = _parse.PlayersGameSlashedValue(game[2].CollectionValuesHtml!),
          TotalTrueAchievement = _parse.GameTotalSlashedValue(game[2].CollectionValuesHtml!),
          PlayerGamerscore = _parse.PlayersGameSlashedValue(game[3].CollectionValuesHtml!),
          TotalGamerscore = _parse.GameTotalSlashedValue(game[3].CollectionValuesHtml!),
          PlayerAchievementCount = _parse.PlayersGameSlashedValue(game[4].CollectionValuesHtml!),
          TotalAchievementCount = _parse.GameTotalSlashedValue(game[4].CollectionValuesHtml!),
          StartedDate = _parse.TaDate(game[5].CollectionValuesHtml!),
          CompletionDate = _parse.TaDate(game[6].CollectionValuesHtml!),
          LastUnlock = _parse.TaDate(game[7].CollectionValuesHtml!),
          Ownership = _parse.GameOwnership(game[8].CollectionValuesHtml!),
          NotForContests = _parse.GameNotForContests(game[9].CollectionImagesHtml!),
          Publisher = Extensions.NullIfWhiteSpace(game[10].CollectionValuesHtml!),
          Developer = Extensions.NullIfWhiteSpace(game[11].CollectionValuesHtml!),
          ReleaseDate = _parse.TaDate(game[12].CollectionValuesHtml!),
          GamersWithGame = _parse.GamersCount(game[13].CollectionValuesHtml!),
          GamersCompleted = _parse.GamersCount(game[14].CollectionValuesHtml!),
          BaseCompletionEstimate = _parse.BaseGameCompletionEstimate(game[15].CollectionValuesHtml!),
          SiteRatio = _parse.DecimalString(game[16].CollectionValuesHtml!),
          SiteRating = _parse.DecimalString(game[17].CollectionValuesHtml!),
          Unobtainables = _parse.Unobtainables(game[18].CollectionImagesHtml!),
          ServerClosure = _parse.TaDate(game[19].CollectionValuesHtml!),
          InstallSize = _parse.GameSize(game[20].CollectionValuesHtml!),
          FullCompletionEstimate = _parse.FullCompletionEstimate(game[21].CollectionValuesHtml!)
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
                                    .Where(incData => !_context.PlayersGames.Where(x => x.PlayerId == player.Id)
                                    .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new {pg, g})
                                    .Select(x => x.g.TrueAchievementId).Contains(incData.GameId));
    var gameIds = _context.Games!.Where(x => newCollectionEntries.Select(y => y.GameId).Contains(x.TrueAchievementId)).ToList();
    foreach(var entry in newCollectionEntries) {
      var newGame = new PlayersGame {
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

      _context.PlayersGames!.Add(newGame);
    }
  }

  private void UpdateGameInformation(List<TA_CollectionEntry> incomingData, List<int> taGameIdList) {
    var knownGames = incomingData.Where(incData => taGameIdList.Contains(incData.GameId)).ToList();
    var gamesToUpdate = _context.Games!.Where(x => knownGames.Select(y => y.GameId).Contains(x.TrueAchievementId));

    foreach(var gameToUpdate in gamesToUpdate) {
      var knownGame = knownGames.First(x => x.GameId == gameToUpdate.TrueAchievementId);

      //we'll skip updating things that likely will never change
      //values skipped: TA ID, Title, Publisher, Developer, ReleaseDate
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
    }
  }

  private void UpdateCollectionInformation(List<TA_CollectionEntry> incomingData, List<int> taGameIdList, Player player) {
    var knownEntries = incomingData.Where(incData => taGameIdList.Contains(incData.GameId));
    var entriesToUpdate = _context.PlayersGames!.Where(x => x.PlayerId == player.Id)
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

  public class CollectionSplit {
    public string? CollectionValuesHtml {get; set;}
    public string? CollectionImagesHtml {get;set;}
    public string? GameIdHtml {get;set;}
  }
}
