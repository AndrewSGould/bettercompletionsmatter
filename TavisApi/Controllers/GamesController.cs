namespace WebApi.Controllers;

using System.Linq;
using TavisApi.Context;
using Tavis.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
  private TavisContext _context;

  public GamesController(TavisContext context) {
    _context = context;
  }

  // [HttpGet]
  // [Route("getAll")]
  // public IActionResult GetAll()
  // {
  //     var games = _context.Games?.Where(x => x.Title != null);
  //     return Ok(games);
  // }

  // [HttpPost]
  // [Route("addCompletedGame")]
  // public IActionResult AddCompletedGame([FromBody] Game newGame) {
  //   _context.Games?.Add(newGame);

  //   _context.SaveChanges();

  //   return Ok();
  // }
}
