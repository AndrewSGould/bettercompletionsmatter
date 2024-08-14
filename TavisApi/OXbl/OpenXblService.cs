using Newtonsoft.Json;
using System.Text;
using TavisApi.OXbl.Interfaces;
using TavisApi.OXbl.Models;

namespace TavisApi.OXbl;

public class OpenXblServiceV2 : IOpenXblServiceV2
{
    public async Task<XblSigninResponse> Connect(OXblLogin openXblAuth)
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

    public async Task<HttpResponseMessage> Get(string? oxblPassword, string endpoint, string values)
    {
        if (oxblPassword is null) throw new Exception("No oxblPassword provided");

        string url = $"https://xbl.io/api/v2/{endpoint}/{values}";

        using (HttpClient client = new())
        {
            client.DefaultRequestHeaders.Add("X-Contract", "100");
            client.DefaultRequestHeaders.Add("X-Authorization", oxblPassword);

            return await client.GetAsync(url);
        }
    }
}
