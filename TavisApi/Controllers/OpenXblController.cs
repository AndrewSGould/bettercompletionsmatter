namespace WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using TavisApi.Context;
using System.Linq;
using TavisApi.Services;
using Discord.Rest;
using Discord;
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
  public async Task<IActionResult> GetXboxUser()
  {
    var currentUsername = _userService.GetCurrentUserName();
    var localuser = _context.Users.FirstOrDefault(x => x.Gamertag == currentUsername);
    var xboxLogin = _context.Logins.FirstOrDefault(x => x.UserId == localuser.Id);
    XblProfiles oxblProfiles = new();

    var response = await _openXblService.Get(xboxLogin.Password, "account", "2533274853753141");

    if (response.IsSuccessStatusCode)
    {
      string responseContent = await response.Content.ReadAsStringAsync();
      oxblProfiles = JsonConvert.DeserializeObject<XblProfiles>(responseContent);
      Console.WriteLine("Response Content: " + responseContent);
    }
    else
    {
      Console.WriteLine("Request failed with status code: " + response.StatusCode);
    }

    return Ok(oxblProfiles.ProfileUsers[0]);
  }
}
