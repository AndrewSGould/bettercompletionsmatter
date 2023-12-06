namespace WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using System.Linq;
using TavisApi.Services;
using Newtonsoft.Json;
using Tavis.Models;

[Route("[controller]")]
[ApiController]
public class OpenXblController : ControllerBase
{
  private readonly TavisContext _context;
  private readonly IUserService _userService;
  private readonly IOpenXblService _openXblService;

  public OpenXblController(TavisContext context, IUserService userService, IOpenXblService openXblService)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    _openXblService = openXblService ?? throw new ArgumentNullException(nameof(openXblService));
  }

  [HttpGet, Route("user")]
  public async Task<IActionResult> GetXboxUser(string gamertag)
  {
    var currentUsername = _userService.GetCurrentUserName();
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == currentUsername);

    if (localuser is null) return BadRequest("No user found with supplied Gamertag");

    var xboxLogin = _context.Logins.FirstOrDefault(x => x.UserId == localuser.Id);
    XblProfiles oxblProfiles = new();

    if (xboxLogin is null) return BadRequest("No xbox profile associated with user");

    var targetXuid = _context.Users.FirstOrDefault(x => x.Gamertag == gamertag)?.Xuid;

    if (targetXuid is null) return BadRequest("No xbox profile associated with requested user");

    var response = await _openXblService.Get(xboxLogin.Password, "account", targetXuid);


    string responseContent = await response.Content.ReadAsStringAsync();
    oxblProfiles = JsonConvert.DeserializeObject<XblProfiles>(responseContent);

    return Ok(oxblProfiles?.ProfileUsers[0]);
  }
}
