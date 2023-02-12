namespace WebApi.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using TavisApi.Services;
using System.Linq;
using Tavis.Models;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly TavisContext _context;

  public UserController(TavisContext context, ITokenService tokenService)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  [HttpGet, Route("availableRegions")]
  public IActionResult AvailableRegions()
  {
    return Ok(_context.Players.Select(x => x.Region).Where(x => x != null).Distinct().OrderBy(x => x));
  } 

  [HttpGet, Route("availableAreas")]
  public IActionResult AvailableAreas()
  {
    //TODO pass in region to filter down the areas
    var test = "United States";
    return Ok(_context.Players.Where(x => x.Region.Contains(test)).Select(x => x.Area).Where(x => x != null).Distinct().OrderBy(x => x));
  }
}
