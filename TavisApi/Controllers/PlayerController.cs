namespace WebApi.Controllers;

using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
  private TavisContext _context;
  private readonly IParser _parser;
  private readonly IDataSync _dataSync;

  public PlayerController(TavisContext context, IParser parser, IDataSync dataSync, IBcmService bcmService)
  {
    _context = context;
    _parser = parser;
    _dataSync = dataSync;
  }

  [HttpGet]
  [Route("getCompletedGames")]
  public IActionResult CompletedGames(int playerId)
  {
    var completedGames = _context.BcmPlayerGames.Where(x => x.PlayerId == playerId && x.CompletionDate != null);
    return Ok(completedGames);
  }
}
