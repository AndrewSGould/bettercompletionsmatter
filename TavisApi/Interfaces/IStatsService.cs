using TavisApi.V2.Bcm.Models;
using TavisApi.V2.Models;

namespace TavisApi.Services;

public interface IStatsService {
	int ScoreRgscCompletions(BcmPlayer player, List<BcmPlayerGame> completedGames);
	void CalcJanBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityGoalReached);
	void CalcFebBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, List<Tuple<Game, int>> allFebCompletions, bool communityBonusReached);
	void CalcMarBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonusReached, IEnumerable<BcmPlayerGame> communityBounties);
	void CalcAprBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, int communityBonus);
	void CalcMayBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonus);
	void CalcJunBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonus);
	void CalcJulyBonus(BcmPlayer player, List<BcmPlayerGame> completedGames, bool communityBonus);
	int CalcJulyCommunityProgress();
	int CalcAprCommunityGoal();
	List<Game> Bounties();
	bool CalcJanCommunityGoal();
	bool CalcMarCommunityGoal();
	bool CalcJunCommunityGoal();
	IEnumerable<BcmPlayerGame> CommunityBounties();
}
