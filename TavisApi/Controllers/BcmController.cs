namespace WebApi.Controllers;

using TavisApi.ContestRules;
using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tavis.Models;
using System.Data;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tavis.Extensions;

[ApiController]
[Route("api/[controller]")]
public class BcmController : ControllerBase
{
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;
  private readonly IBcmService _bcmService;
  private readonly IUserService _userService;
  private static readonly Random rand = new Random();

  public BcmController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService, IUserService userService)
  {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
    _bcmService = bcmService;
    _userService = userService;
  }

  [HttpGet]
  [Route("getBcmLeaderboardList")]
  public IActionResult BcmLeaderboardList()
  {
    var players = _bcmService.GetPlayers();

    var leaderboard = new List<Leaderboard>();

    if (players.Count() == 0) return BadRequest("No players found!");

    foreach (var player in players)
    {
      leaderboard.Add(new Leaderboard
      {
        Player = player,
        BcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id)
      });
    }

    return Ok(leaderboard.OrderBy(x => x.BcmStats.Rank));
  }

  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("recalcBcmLeaderboard")]
  public IActionResult RecalcBcmLeaderboard()
  {
    //TODO: Changing data. This should be a POST
    var players = _bcmService.GetPlayers();

    var leaderboardList = new List<Ranking>();

    foreach (var player in players)
    {
      var playerBcmStats = _context.BcmStats.FirstOrDefault(x => x.PlayerId == player.Id);

      if (playerBcmStats == null)
      {
        playerBcmStats = new BcmStat();
        _context.BcmStats.Add(playerBcmStats);
      }

      playerBcmStats.PlayerId = player.Id;

      var playerCompletions = _context.PlayerGames
                                      .Where(x => x.PlayerId == player.Id &&
                                        x.CompletionDate != null &&
                                        x.CompletionDate >= _bcmService.GetContestStartDate());

      var gamesCompletedThisYear = playerCompletions.Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { PlayersGames = pg, Games = g }).ToList();

      var completedGamesCount = gamesCompletedThisYear.Count();
      var ratioOfGames = gamesCompletedThisYear.Select(x => x.Games.SiteRatio);
      var estimateOfGames = gamesCompletedThisYear.Select(x => x.Games.FullCompletionEstimate);

      playerBcmStats.Completions = completedGamesCount;
      playerBcmStats.AverageRatio = ratioOfGames.DefaultIfEmpty(0).Average();
      playerBcmStats.HighestRatio = ratioOfGames.DefaultIfEmpty(0).Max();
      playerBcmStats.AverageTimeEstimate = estimateOfGames.DefaultIfEmpty(0).Average();
      playerBcmStats.HighestTimeEstimate = estimateOfGames.DefaultIfEmpty(0).Max();

      double? basePoints = 0.0;
      foreach (var game in gamesCompletedThisYear)
      {
        var pointValue = _bcmService.CalcBcmValue(game.Games.SiteRatio, game.Games.FullCompletionEstimate);
        if (pointValue != null)
          basePoints += pointValue;
      }

      playerBcmStats.BasePoints = basePoints;
      playerBcmStats.AveragePoints = completedGamesCount != 0 ? basePoints / completedGamesCount : 0;

      leaderboardList.Add(new Ranking
      {
        PlayerId = player.Id,
        BcmPoints = playerBcmStats.BasePoints
      });
    }

    _context.SaveChanges();

    // after saving point calculations, lets order the leaderboard and save again for the rankings
    leaderboardList = leaderboardList.OrderByDescending(x => x.BcmPoints).ToList();

    foreach (var player in players)
    {
      var playerBcmStats = _context.BcmStats.First(x => x.PlayerId == player.Id);
      var previousRanking = playerBcmStats.Rank;
      var newRanking = leaderboardList.FindIndex(x => x.PlayerId == player.Id) + 1;

      playerBcmStats.Rank = newRanking;
      playerBcmStats.RankMovement = previousRanking - newRanking;
    }

    _context.SaveChanges();

    return Ok();
  }

  [HttpGet]
  [Route("getBcmPlayer")]
  public IActionResult BcmPlayer(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
    var playerId = localuser.Id;

    var bcmPlayer = _context.Players.First(x => x.Id == playerId);

    if (bcmPlayer == null) return BadRequest("Player not found");

    var bcmPlayerSummary = new Object();

    var playersGames = _context.PlayerGames
                        .Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { PlayersGames = pg, Games = g })
                        .Where(x => x.PlayersGames.PlayerId == bcmPlayer.Id
                          && x.PlayersGames.CompletionDate != null
                          && x.PlayersGames.CompletionDate >= _bcmService.GetContestStartDate())
                        .OrderByDescending(x => x.PlayersGames.CompletionDate)
                        .Select(x => new BcmPlayerSummary
                        {
                          Title = x.Games.Title,
                          Ratio = x.Games.SiteRatio,
                          Estimate = x.Games.FullCompletionEstimate,
                          CompletionDate = x.PlayersGames.CompletionDate,
                          Points = _bcmService.CalcBcmValue(x.Games.SiteRatio, x.Games.FullCompletionEstimate)
                        }).ToList();

    bcmPlayerSummary = new
    {
      Player = bcmPlayer,
      Games = playersGames,
      Ranking = _context.BcmStats.First(x => x.PlayerId == playerId),
      Score = playersGames.Sum(x => x.Points)
    };

    return Ok(bcmPlayerSummary);
  }

  [HttpGet]
  [Route("yearly-summary")]
  public async Task<IActionResult> GetYearlySummary(string player)
  {
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == player);
    var playerId = localuser.Id;

    return await Task.FromResult(Ok(new
    {
      completionLetters = _bcmService.GetAlphabetChallengeProgress(playerId),
      OddJobCompletions = _bcmService.GetOddJobChallengeProgress(playerId),
      YearlyCompletions = 0
    }));
  }

  // Get random games, sorted by eligibility, then alphabetically by player
  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("verifyRandomGameEligibility")]
  public IActionResult VerifyRandomGameEligibility()
  {
    var playersIneligible = new List<object>();
    var allPlayers = new List<RgscResult>();
    var players = _bcmService.GetPlayers().OrderBy(x => x.Name);

    foreach (var player in players)
    {
      var randomGameOptions = _context.PlayerGames?
            .Join(_context.Games!, pg => pg.GameId,
              g => g.Id, (pg, g) => new { PlayersGames = pg, Games = g })
            .Where(x => x.PlayersGames.PlayerId == player.Id
              && x.Games.GamersCompleted > 0
              && !x.Games.Unobtainables
              && !x.PlayersGames.NotForContests
              && x.PlayersGames.CompletionDate == null
              && x.PlayersGames.Ownership != Ownership.NoLongerHave
              && BcmRule.RandomValidPlatforms.Contains(x.PlayersGames.Platform!))
            .AsEnumerable() // TODO: rewrite so this stays as a query?
            .Where(x => Queries.FilterGamesForYearlies(x.Games))
            .ToList();

      if (randomGameOptions?.Count() < BcmRule.RandomMinimumEligibilityCount)
      {
        playersIneligible.Add(new
        {
          Player = player.Name,
          EligibleCount = randomGameOptions.Count(),
          GameList = randomGameOptions.Select(x => x.Games.Title).ToList()
        });
      }

      Game randomGame = null;

      if (randomGameOptions?.Count() != 0)
      {
        var random = rand.Next(0, randomGameOptions.Count());
        randomGame = randomGameOptions?[random].Games;

        allPlayers.Add(new RgscResult
        {
          Player = player.Name,
          RandomGame = randomGameOptions?.Count() < BcmRule.RandomMinimumEligibilityCount ? "" : randomGame.Title,
          EligibleCount = randomGameOptions.Count(),
          GameList = randomGameOptions.Select(x => x.Games.Title).OrderBy(x => x).ToList()
        });
      }

      _context.BcmRgsc.Add(new BcmRgsc
      {
        PlayerId = player.Id,
        GameId = randomGameOptions?.Count() != 0 ? randomGame.Id : 0,
        Issued = DateTime.Now
      });

      _context.SaveChanges();
    }

    var results = new
    {
      Invalids = playersIneligible,
      FullList = allPlayers
    };

    WriteRgscExcelFile(allPlayers);

    return Ok(results);
  }

  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("produceStatReport")]
  public IActionResult StatReport()
  {
    var bcmPlayers = _bcmService.GetPlayers();
    var statSpread = new List<object>();

    foreach (var player in bcmPlayers)
    {
      var playerGames = _context.PlayerGames.Where(x => x.PlayerId == player.Id);

      var gamerscoreTotal = playerGames.Where(x => x.CompletionDate.Value.Year == 2023 && x.CompletionDate.Value.Month == 2)
                                        .Sum(x => x.Gamerscore);
      var trueachievementTotal = playerGames.Where(x => x.CompletionDate.Value.Year == 2023 && x.CompletionDate.Value.Month == 2)
                                            .Sum(x => x.TrueAchievement);
      var completions = playerGames.Where(x => x.CompletionDate.Value.Year == 2023 && x.CompletionDate.Value.Month == 2)
                                    .Count(x => x.CompletionDate != null);

      var stats = new
      {
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

  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("produceBcmReport")]
  public IActionResult BcmReport()
  {
    WriteExcelFile();

    return Ok();
  }

  [HttpGet, Authorize(Roles = "Admin, Bcm Admin")]
  [Route("produceCompletedGamesReport")]
  public IActionResult CompletedGamesReport()
  {
    WriteCompletedGamesExcelFile();

    return Ok();
  }

  private void WriteRgscExcelFile(List<RgscResult> rgscResults)
  {
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

      foreach (var player in players)
      {
        sheetNumber++;

        var playersCompletedGames = _context.PlayerGames
          .Join(_context.Games, pg => pg.GameId, g => g.Id, (pg, g) => new { PlayerGames = pg, Games = g })
          .Where(x => x.PlayerGames.PlayerId == player.Id
            && x.PlayerGames.CompletionDate != null
            && x.PlayerGames.CompletionDate > bcmStart)
          .ToList()
          .OrderByDescending(x => x.PlayerGames.CompletionDate)
          .Select(x => new UserDetails
          {
            DateCompleted = x.PlayerGames.CompletionDate.Value.ToString("MMM dd"),
            GameTitle = x.Games.Title,
            Ratio = x.Games.SiteRatio
          })
          .ToList();

        // var playerCompletionHistory = _context.PlayerCompletionHistory.Where(x => x.PlayerId == player.Id).ToList();


        // foreach(var completedGame in playerCompletionHistory.Where(x => x.CompletionDate > bcmStart)) {
        //   var game = _context.Games
        //               .Join(_context.PlayerGames, g => g.Id, pg => pg.GameId, (g, pg) => new {Games = g, PlayerGames = pg})
        //               .Where(x => x.Games.Id == completedGame.GameId)
        //               .OrderBy(x => x.PlayerGames.CompletionDate)
        //               .FirstOrDefault();

        //   playersCompletedGames.Add(new UserDetails {
        //     DateCompleted = completedGame.CompletionDate.ToString("MMM dd"),
        //     GameTitle = game.Games.Title,
        //     Ratio = game.Games.SiteRatio
        //   });
        // }

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

  private void WriteCompletedGamesExcelFile()
  {
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
    foreach (var newCompletion in newlyCompletedGames)
    {
      var newGameCompletion = _context.Games.FirstOrDefault(x => x.Id == newCompletion);
      thisMonthsCompletions.Add(newGameCompletion);

      _context.BcmCompletionHistory.Add(new BcmCompletionHistory
      {
        GameId = newGameCompletion.Id,
        SiteRatio = newGameCompletion.SiteRatio
      });
    }

    _context.SaveChanges();
    var completedGamesReport = new List<object>();

    // get the first day of last month
    var today = DateTime.Today;
    var firstOfLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);

    foreach (var game in thisMonthsCompletions.OrderBy(x => x.Title))
    {
      if (game.SiteRatio >= 1.2)
      {
        completedGamesReport.Add(new
        {
          Title = game.Title,
          Ratio = game.ReleaseDate >= firstOfLastMonth ? "TBD" : game.SiteRatio.ToString(),
          CompletionTime = (game.Gamerscore == 200 || game.Gamerscore == 400 || game.Gamerscore == 1000)
                              && game.ReleaseDate < firstOfLastMonth
                              ? game.FullCompletionEstimate : null
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
