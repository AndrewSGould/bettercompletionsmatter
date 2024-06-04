using Tavis.Models;

namespace TavisApi.V2.OXbl;

public interface IOpenXblServiceV2 {
	public Task<XblSigninResponse> Connect(OXblLogin openXblAuth);
	public Task<HttpResponseMessage> Get(string? oxblPassword, string endpoint, string values);
}
