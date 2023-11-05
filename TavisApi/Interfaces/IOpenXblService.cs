using Tavis.Models;

namespace TavisApi.Services;

public interface IOpenXblService
{
  public Task<XblSigninResponse> Connect(ConnectAuth openXblAuth);
  public Task<HttpResponseMessage> Get(string oxblPassword, string endpoint, string values);
}
