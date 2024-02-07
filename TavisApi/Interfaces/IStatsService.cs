using Tavis.Models;

namespace TavisApi.Services;

public interface IStatsService
{
  int ScoreRgscCompletions(BcmPlayer player, List<BcmPlayerGame> completedGames);
  void CalcJanBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityGoalReached);
  bool CalcJanCommunityGoal();
}
