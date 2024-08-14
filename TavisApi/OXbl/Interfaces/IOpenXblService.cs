using TavisApi.OXbl.Models;

namespace TavisApi.OXbl.Interfaces;

public interface IOpenXblServiceV2 {
	public Task<XblSigninResponse> Connect(OXblLogin openXblAuth);
	public Task<HttpResponseMessage> Get(string? oxblPassword, string endpoint, string values);
}
