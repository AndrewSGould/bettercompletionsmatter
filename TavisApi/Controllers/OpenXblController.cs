namespace WebApi.Controllers;

using TavisApi.Context;
using TavisApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class OpenXblController : ControllerBase
{
  public OpenXblController()
  {
  }

  [HttpPost]
  [Route("connect")]
  public async Task<IActionResult> ConnectAsync(ConnectAuth thing)
  {
    var test = thing;
    XblSigninResponse responseJson = new XblSigninResponse();

    // Define the URL
    string url = "https://xbl.io/app/claim";

    // Create an instance of HttpClient
    using (HttpClient client = new HttpClient())
    {
      // Define the request data
      var requestData = new
      {
        code = thing.Code,
        app_key = thing.App_Key
      };

      // Serialize the request data to JSON
      string jsonData = JsonConvert.SerializeObject(requestData);
      var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

      // Send the POST request
      HttpResponseMessage response = await client.PostAsync(url, content);


      if (response.IsSuccessStatusCode)
      {
        // Request was successful
        string responseContent = await response.Content.ReadAsStringAsync();
        responseJson = JsonConvert.DeserializeObject<XblSigninResponse>(responseContent);
        Console.WriteLine("Response Content: " + responseContent);
      }
      else
      {
        // Request failed
        Console.WriteLine("Request failed with status code: " + response.StatusCode);
      }
    }

    // TODO: only return the pic and gamertag
    // next step would be to take the email that we have and check the users table to see
    //  if they have 'finished' registering
    return Ok(new { responseJson.Avatar, responseJson.Gamertag });
  }

  public class ConnectAuth
  {
    public string? Code { get; set; }
    public string? App_Key { get; set; }
  }

  public class XblSigninResponse
  {
    public string App_Key { get; set; }
    public string Xuid { get; set; }
    public string Gamertag { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
  }
}
