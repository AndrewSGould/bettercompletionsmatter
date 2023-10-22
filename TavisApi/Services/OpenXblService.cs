using System.Text;
using Newtonsoft.Json;
using Tavis.Models;

namespace TavisApi.Services;

public class OpenXblService : IOpenXblService
{
  public async Task<XblSigninResponse> Connect(ConnectAuth openXblAuth)
  {
    XblSigninResponse oxblSignin = new();

    string url = "https://xbl.io/app/claim";

    using (HttpClient client = new())
    {
      var requestData = new
      {
        code = openXblAuth.Code,
        app_key = openXblAuth.App_Key
      };

      string jsonData = JsonConvert.SerializeObject(requestData);
      StringContent content = new(jsonData, Encoding.UTF8, "application/json");

      HttpResponseMessage response = await client.PostAsync(url, content);

      if (response.IsSuccessStatusCode)
      {
        string responseContent = await response.Content.ReadAsStringAsync();
        oxblSignin = JsonConvert.DeserializeObject<XblSigninResponse>(responseContent);
        Console.WriteLine("Response Content: " + responseContent);
      }
      else
      {
        Console.WriteLine("Request failed with status code: " + response.StatusCode);
      }
    }

    return oxblSignin;
  }
}
