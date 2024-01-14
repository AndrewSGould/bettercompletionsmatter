using Tavis.Models;

namespace TavisApi.Services;

public interface IStatsService
{
  void CalcJanBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityGoalReached);
  bool CalcJanCommunityGoal();
}
