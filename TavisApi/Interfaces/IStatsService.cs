using Tavis.Models;

namespace TavisApi.Services;

public interface IStatsService
{
  int ScoreRgscCompletions(BcmPlayer player, List<BcmPlayerGame> completedGames);
  void CalcJanBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityGoalReached);
  void CalcFebBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, List<Tuple<Game, int>> allFebCompletions, bool communityBonusReached);
  bool CalcJanCommunityGoal();
}
