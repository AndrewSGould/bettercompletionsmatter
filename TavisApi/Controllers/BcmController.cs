namespace WebApi.Controllers;

using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using static TavisApi.Services.TA_GameCollection;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;
using System.Collections.ObjectModel;
using System.Data;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class BcmController : ControllerBase {
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;
  private readonly IBcmService _bcmService;
  private static readonly Random rand = new Random();

  public BcmController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService) {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _bcmService = bcmService;
  }

  // Get random games, sorted by eligibility, then alphabetically by player
  [HttpGet, Authorize(Roles = "Super Admin, Bcm Admin")]
  [Route("verifyRandomGameEligibility")]
  public IActionResult VerifyRandomGameEligibility() {
    var playersIneligible = new List<object>();
    var allPlayers = new List<RgscResult>();
    var players = _bcmService.GetPlayers().OrderBy(x => x.Name);

    foreach(var player in players) {
      var randomGameOptions = _context.PlayerGames?
            .Join(_context.Games!, pg => pg.GameId, 
              g => g.Id, (pg, g) => new {PlayersGames = pg, Games = g})
            .Where(x => x.PlayersGames.PlayerId == player.Id
              && x.Games.SiteRatio > BcmRule.MinimumRatio
              && (x.Games.FullCompletionEstimate <= BcmRule.RandomMaxEstimate
                || x.Games.FullCompletionEstimate == null)
              && x.Games.GamersCompleted > 0
              && !x.Games.Unobtainables
              && !x.PlayersGames.NotForContests
              && x.PlayersGames.CompletionDate == null
              && x.PlayersGames.Ownership != Tavis.Models.Ownership.NoLongerHave
              && BcmRule.RandomValidPlatforms.Contains(x.PlayersGames.Platform!)
              && !BcmRule.ExemptGames.Any(y => y == x.Games.Title))
            .ToList();

      if (randomGameOptions?.Count() < BcmRule.RandomMinimumEligibilityCount) {
        playersIneligible.Add(new {
          Player = player.Name,
          EligibleCount = randomGameOptions.Count(),
          GameList = randomGameOptions.Select(x => x.Games.Title).ToList()
        });
      }

      var random = rand.Next(0, randomGameOptions.Count());

      allPlayers.Add(new RgscResult {
        Player = player.Name,
        RandomGame = randomGameOptions?.Count() < BcmRule.RandomMinimumEligibilityCount ? "" : randomGameOptions?[random].Games.Title,
        EligibleCount = randomGameOptions.Count(),
        GameList = randomGameOptions.Select(x => x.Games.Title).OrderBy(x => x).ToList()
      });
    }

    var results = new {
      Invalids = playersIneligible,
      FullList = allPlayers
    };

    WriteRgscExcelFile(allPlayers);

    return Ok(results);
  }

  [HttpGet, Authorize(Roles = "Super Admin, Bcm Admin")]
  [Route("produceStatReport")]
  public IActionResult StatReport() {
    var bcmPlayers = _bcmService.GetPlayers();
    var statSpread = new List<object>();

    foreach(var player in bcmPlayers) {
      var playerGames = _context.PlayerGames.Where(x => x.PlayerId == player.Id);

      var gamerscoreTotal = playerGames.Sum(x => x.Gamerscore);
      var trueachievementTotal = playerGames.Sum(x => x.TrueAchievement);
      var completions = playerGames.Count(x => x.CompletionDate != null);

      var stats = new {
        Player = player.Name,
        Gamerscore = gamerscoreTotal,
        TrueAchievement = trueachievementTotal,
        Ratio = trueachievementTotal == 0 ? 0 : Math.Round((decimal)((decimal)trueachievementTotal / gamerscoreTotal), 4),
        TAD = trueachievementTotal == 0 ? 0 : trueachievementTotal - gamerscoreTotal,
        Completions = completions
      };

      statSpread.Add(stats);
    }
    

    return Ok(statSpread);
  }

  [HttpGet, Authorize(Roles = "Super Admin, Bcm Admin")]
  [Route("produceBcmReport")]
  public IActionResult BcmReport() {
    WriteExcelFile();

    return Ok();
  }

  [HttpGet, Authorize(Roles = "Super Admin, Bcm Admin")]
  [Route("produceCompletedGamesReport")]
  public IActionResult CompletedGamesReport() {
    WriteCompletedGamesExcelFile();

    return Ok();
  }

  public class UserDetails {
    public string DateCompleted {get; set;}
    public string GameTitle {get; set;}
    public double? Ratio {get; set;}
  }

  public class RgscResult {
    public string Player {get; set;}
    public string RandomGame {get; set;}
    public int EligibleCount {get; set;}
    public List<string> GameList {get; set;}
  }

  private void WriteRgscExcelFile(List<RgscResult> rgscResults) {
    using (SpreadsheetDocument document = SpreadsheetDocument.Create("rgsc.xlsx", SpreadsheetDocumentType.Workbook))
    {
      WorkbookPart workbookPart = document.AddWorkbookPart();
      workbookPart.Workbook = new Workbook();

      Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
      DocumentFormat.OpenXml.UInt32Value sheetNumber = 1;

      // Lets converts our object data to Datatable for a simplified logic.
      // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
      DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rgscResults), (typeof(DataTable)));

      WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
      var sheetData = new SheetData();
      worksheetPart.Worksheet = new Worksheet(sheetData);

      Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "RGSC" };

      sheets.Append(sheet);

      Row headerRow = new Row();

      List<String> columns = new List<string>();
      foreach (System.Data.DataColumn column in table.Columns)
      {
        columns.Add(column.ColumnName);

        Cell cell = new Cell();
        cell.DataType = CellValues.String;
        cell.CellValue = new CellValue(column.ColumnName);
        headerRow.AppendChild(cell);
      }

      foreach (DataRow dsrow in table.Rows)
      {
        Row newRow = new Row();
        foreach (String col in columns)
        {
          Cell cell = new Cell();
          cell.DataType = CellValues.String;
          cell.CellValue = new CellValue(dsrow[col].ToString());
          newRow.AppendChild(cell);
        }

        sheetData.AppendChild(newRow);
      }

      workbookPart.Workbook.Save();
    }
  }

  private void WriteExcelFile()
  {
    var players = _bcmService.GetPlayers().OrderBy(x => x.Name);
    var bcmStart = _bcmService.GetContestStartDate();

    using (SpreadsheetDocument document = SpreadsheetDocument.Create("bcmreport.xlsx", SpreadsheetDocumentType.Workbook))
    {
      WorkbookPart workbookPart = document.AddWorkbookPart();
      workbookPart.Workbook = new Workbook();

      Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
      DocumentFormat.OpenXml.UInt32Value sheetNumber = 1;

      foreach(var player in players) {
        sheetNumber++;

        var playersCompletedGames = _context.PlayerGames
          .Join(_context.Games, pg => pg.GameId, g => g.Id, (pg, g) => new {PlayerGames = pg, Games = g })
          .Where(x => x.PlayerGames.PlayerId == player.Id
            && x.PlayerGames.CompletionDate != null
            && x.PlayerGames.CompletionDate > bcmStart)
          .ToList()
          .Select(x => new UserDetails {
            DateCompleted = x.PlayerGames.CompletionDate.Value.ToString("MMM dd"),
            GameTitle = x.Games.Title,
            Ratio = x.Games.SiteRatio
          })
          .ToList();

        var playerCompletionHistory = _context.PlayerCompletionHistory.Where(x => x.PlayerId == player.Id).ToList();
        
        foreach(var completedGame in playerCompletionHistory) {
          var game = _context.Games
                      .Join(_context.PlayerGames, g => g.Id, pg => pg.GameId, (g, pg) => new {Games = g, PlayerGames = pg})
                      .Where(x => x.Games.Id == completedGame.GameId)
                      .FirstOrDefault();

          playersCompletedGames.Add(new UserDetails {
            DateCompleted = completedGame.CompletionDate.ToString("MMM dd"),
            GameTitle = game.Games.Title,
            Ratio = game.Games.SiteRatio
          });
        }

        playersCompletedGames = playersCompletedGames.OrderBy(x => DateTime.Parse(x.DateCompleted)).ToList();

        // Lets converts our object data to Datatable for a simplified logic.
        // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
        DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(playersCompletedGames), (typeof(DataTable)));

        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var sheetData = new SheetData();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = sheetNumber, Name = player.Name };

        sheets.Append(sheet);

        Row headerRow = new Row();

        List<String> columns = new List<string>();
        foreach (System.Data.DataColumn column in table.Columns)
        {
          columns.Add(column.ColumnName);

          Cell cell = new Cell();
          cell.DataType = CellValues.String;
          cell.CellValue = new CellValue(column.ColumnName);
          headerRow.AppendChild(cell);
        }

        foreach (DataRow dsrow in table.Rows)
        {
          Row newRow = new Row();
          foreach (String col in columns)
          {
            Cell cell = new Cell();
            cell.DataType = CellValues.String;
            cell.CellValue = new CellValue(dsrow[col].ToString());
            newRow.AppendChild(cell);
          }

          sheetData.AppendChild(newRow);
        }
      }

      workbookPart.Workbook.Save();
    }
  }

  private void WriteCompletedGamesExcelFile() {
    // get the GameId of every completion already recorded by BCM
    var historicallyCompletedGames = _context.BcmCompletionHistory.Select(x => x.GameId);

    // get the GameId of every completion Tavis knows of
    var completedGames = _context.PlayerGames
                  .Where(x => x.CompletionDate != null && x.CompletionDate >= _bcmService.GetContestStartDate())
                  .GroupBy(x => x.GameId)
                  .Select(x => x.First())
                  .ToList()
                  .Select(x => x.GameId)
                  .ToList();

    // the difference between all completions minus historical completions are new to Tavis
    var newlyCompletedGames = completedGames.Except(historicallyCompletedGames);
    var thisMonthsCompletions = new List<Game>();

    // record these new additions to the historical data
    foreach(var newCompletion in newlyCompletedGames) {
      var newGameCompletion = _context.Games.FirstOrDefault(x => x.Id == newCompletion);
      thisMonthsCompletions.Add(newGameCompletion);

      _context.BcmCompletionHistory.Add(new BcmCompletionHistory {
        GameId = newGameCompletion.Id,
        Title = newGameCompletion.Title,
        SiteRatio = newGameCompletion.SiteRatio,
        ReleaseDate = newGameCompletion.ReleaseDate
      });
    }

    _context.SaveChanges();
    var completedGamesReport = new List<object>();

    // get the first day of last month
    var today = DateTime.Today;
    var firstOfLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);       

    foreach(var game in thisMonthsCompletions.OrderBy(x => x.Title)) {
      if (game.SiteRatio >= 1.2) {
        completedGamesReport.Add(new {
          Title = game.Title,
          Ratio = game.ReleaseDate >= firstOfLastMonth ? "TBD" : game.SiteRatio.ToString()
        });
      }
    }

    // now that we have all completions recorded, let's generate the new spreadsheet
    using (SpreadsheetDocument document = SpreadsheetDocument.Create("completedgames.xlsx", SpreadsheetDocumentType.Workbook))
    {
      WorkbookPart workbookPart = document.AddWorkbookPart();
      workbookPart.Workbook = new Workbook();

      Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
      DocumentFormat.OpenXml.UInt32Value sheetNumber = 1;

      // Lets converts our object data to Datatable for a simplified logic.
      // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
      DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(completedGamesReport), (typeof(DataTable)));

      WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
      var sheetData = new SheetData();
      worksheetPart.Worksheet = new Worksheet(sheetData);

      Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Completed Games" };

      sheets.Append(sheet);

      Row headerRow = new Row();

      List<String> columns = new List<string>();
      foreach (System.Data.DataColumn column in table.Columns)
      {
        columns.Add(column.ColumnName);

        Cell cell = new Cell();
        cell.DataType = CellValues.String;
        cell.CellValue = new CellValue(column.ColumnName);
        headerRow.AppendChild(cell);
      }

      foreach (DataRow dsrow in table.Rows)
      {
        Row newRow = new Row();
        foreach (String col in columns)
        {
          Cell cell = new Cell();
          cell.DataType = CellValues.String;
          cell.CellValue = new CellValue(dsrow[col].ToString());
          newRow.AppendChild(cell);
        }

        sheetData.AppendChild(newRow);
      }

      workbookPart.Workbook.Save();
    }
  }
}
