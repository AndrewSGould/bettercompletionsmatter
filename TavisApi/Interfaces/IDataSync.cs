using Microsoft.AspNetCore.SignalR;
using Tavis.Models;
using TavisApi.V2.Bcm.Models;
using static TavisApi.Services.DataSync;
using static TavisApi.Services.TA_GameCollection;

namespace TavisApi.Services;

public interface IDataSync {
	object DynamicSync(List<BcmPlayer> players, SyncOptions syncOptions, SyncHistory syncLog, IHubContext<SyncSignal> hub);
	TaParseResult ParseTa(long playerId, SyncOptions gcOptions);
	void ParseGamePages(List<int> gamesToUpdateIds);
	void ParseGamesWithGold(ref int page);
}
