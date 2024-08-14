using Microsoft.AspNetCore.SignalR;
using TavisApi.Models;
using TavisApi.TrueAchievements.Models;
using static TavisApi.TrueAchievements.DataSync;
using static TavisApi.TrueAchievements.TA_GameCollection;

namespace TavisApi.TrueAchievements.Interfaces;

public interface IDataSync {
	object DynamicSync(List<Player> players, SyncOptions syncOptions, SyncHistory syncLog, IHubContext<SyncSignal> hub);
	TaParseResult ParseTa(long playerId, SyncOptions gcOptions);
	void ParseGamePages(List<int?> gamesToUpdateIds);
	void ParseGamesWithGold(ref int page);
}
