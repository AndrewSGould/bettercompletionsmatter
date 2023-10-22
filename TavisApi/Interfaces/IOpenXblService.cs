using Tavis.Models;

namespace TavisApi.Services;

public interface IOpenXblService
{
  public Task<XblSigninResponse> Connect(ConnectAuth openXblAuth);
}
