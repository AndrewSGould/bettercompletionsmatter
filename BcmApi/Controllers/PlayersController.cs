using System.Diagnostics;
using System.Net;
using BcmApi.Context;
using BcmApi.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase 
{
  private BcmContext _context;

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
  [Route("getCompletedGames")]
  public IActionResult GetPlayerCompletedGames(int playerId)
  {
    if (playerId == 0) return BadRequest();

    var completedGames = _context.Players!
        .Join(_context.PlayerCompletedGames, player => player.Id, complGame => complGame.PlayerId, 
            (player, complGame) => new { Player = player, ComplGame = complGame})
        .Where(x => x.Player.Id == playerId)
        .Join(_context.Games!, complGame => complGame.ComplGame.GameId, game => game.Id,
            (complGame, game) => new {ComplGame = complGame, Game = game})
        .Select(x => new Game {
          Title = x.Game.Title,
          Ratio = x.Game.Ratio,
          Time = x.Game.Time,
          Value = x.Game.Value
        });

    return Ok(completedGames);
  }

  [HttpGet]
  [Route("retrieveCompletedGames")]
  public IActionResult RetrieveCompletedGames(int playerId) {
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    var playerTrueAchId = _context.Players!.Where(x => x.Id == Convert.ToInt32(playerId)).First().TrueAchievementId;
    playerTrueAchId = 461682;

    var entireGameList = new List<List<string>>();

    ParseCollectionPage(playerTrueAchId, entireGameList, 1);

    var typedGames = new List<Game>();

    foreach(var game in entireGameList) {
      double ratio = 0;
      if (Double.TryParse(game[23], out double result)) {
        ratio = result;
      }
      else
        Console.WriteLine($"Unable to parse {game[0].Trim()}");

      int time = 0;
      var gameTime = game[28];

      if (gameTime.StartsWith("1000"))
        time = 1000;
      else {
        gameTime = gameTime.Substring(gameTime.LastIndexOf('-') + 1).TrimEnd('h');

        if (Int32.TryParse(gameTime, out int timeresult)) {
          time = timeresult;
        }
        else
          Console.WriteLine($"Unable to parse {game[0].Trim()} with a time of {game[28]}");
      }

      typedGames.Add(new Game {
        Title = game[0].Trim(),
        Ratio = ratio,
        Time = time,
        Value = Convert.ToInt32(Math.Round(ratio * Math.Sqrt(ratio) * time))
      });
    }

    //TODO: reduce the 'completedgames' to anything that has been completed within the month
    var result2 = typedGames.Where(p => _context.Games!.All(p2 => p2.Title != p.Title));
    
    foreach(var game in result2) {
      _context.Games!.Add(new Game{
        Title = game.Title,
        Ratio = game.Ratio,
        Value = game.Value,
        Time = game.Time
      });
    }

    _context.SaveChanges();
    stopWatch.Stop();


    TimeSpan ts = stopWatch.Elapsed;

    // Format and display the TimeSpan value.
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    return Ok(result2);
  }

  private void ParseCollectionPage(int playerTrueAchId, List<List<string>> entireGameList, int page) {
    using var httpClient = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7CoGameCollection_TimeZone=Eastern%20Standard%20Time%26txtGamerID%3D104571%26ddlSortBy%3DTitlename%26ddlDLCInclusionSetting%3DAllDLC%26ddlCompletionStatus%3DAll%26ddlTitleType%3DGame%26ddlContestStatus%3DAll%26asdGamePropertyID%3D-1%26oGameCollection_Order%3DDatecompleted%26oGameCollection_Page%3D1%26oGameCollection_ItemsPerPage%3D10000%26oGameCollection_ShowAll%3DFalse%26txtGameRegionID%3D2%26GameView%3DoptListView%26chkColTitlename%3DTrue%26chkColCompletionestincDLC%3DTrue%26chkColUnobtainables%3DTrue%26chkColSiteratio%3DTrue%26chkColPlatform%3DTrue%26chkColServerclosure%3DTrue%26chkColNotforcontests%3DTrue%26chkColSitescore%3DTrue%26chkColOfficialScore%3DTrue%26chkColItems%3DTrue%26chkColDatestarted%3DTrue%26chkColDatecompleted%3DTrue%26chkColLastunlock%3DTrue%26chkColOwnershipstatus%3DTrue%26chkColPublisher%3DTrue%26chkColDeveloper%3DTrue%26chkColReleasedate%3DTrue%26chkColGamerswithgame%3DTrue%26chkColGamerscompleted%3DTrue%26chkColGamerscompletedperentage%3DTrue%26chkColCompletionestimate%3DTrue%26chkColSiterating%3DTrue%26chkColInstallsize%3DTrue");
    var response = httpClient.Send(request);
    using var reader = new StreamReader(response.Content.ReadAsStream());
    var responseBody = reader.ReadToEnd();

    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
    doc.LoadHtml(responseBody);
    
    List<List<string>> games = doc.DocumentNode.SelectSingleNode("//table")
      .Descendants("tr")
      .Skip(1)
      .SkipLast(1)
      .Where(tr=>tr.Elements("td").Count()>1)
      .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
      .ToList();

    var gamesFullHtml = doc.DocumentNode.SelectSingleNode("//table")
      .Descendants("tr")
      .Skip(1)
      .SkipLast(1)
      .Where(tr => tr.Elements("td").Count()>1);

    entireGameList.AddRange(games);

    if (games.Count() == 100) {
      page++;
      ParseCollectionPage(playerTrueAchId, entireGameList, page);
    }
  }
}
