using TavisApi.V2.Bcm.Models;
using TavisApi.V2.Models;

namespace TavisApi.Services;

public interface IBcmService {
	List<BcmPlayer> GetPlayers();
	DateTime? GetContestStartDate();
	int? CalcBcmValue(int platformId, double? ratio, double? estimate);
	Task<List<string>> GetAlphabetChallengeProgress(long playerId);
	Task<List<Game>> GetOddJobChallengeProgress(long playerId);
	object GetParticipationProgress(BcmPlayer player);
	long? GetRegistrationId();
}
