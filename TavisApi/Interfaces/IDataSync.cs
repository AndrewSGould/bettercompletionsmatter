using Tavis.Models;
using static TavisApi.Services.DataSync;
using static TavisApi.Services.TA_GameCollection;

namespace TavisApi.Services;

public interface IDataSync {
  object DynamicSync(List<Player> players, SyncOptions syncOptions);
  TaParseResult ParseTa(int playerId, SyncOptions gcOptions);
  void ParseGamePages(List<int> gamesToUpdateIds);
  void ParseGamesWithGold(ref int page);
}