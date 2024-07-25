using TavisApi.V2.Bcm.Models;

namespace TavisApi.Services;

public interface IRgscService {
	int GetUserRerollCount(long userId);
	List<BcmPlayerGame>? GetEligibleRandoms(BcmPlayer player);
}
