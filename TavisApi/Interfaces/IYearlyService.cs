using TavisApi.V2.Bcm.Models;
using static TavisApi.Services.YearlyService;

namespace TavisApi.Services;

public interface IYearlyService {
	List<YearlyOption> EvalYearlyOptions(BcmPlayer player, int yearlyId, List<BcmPlayerGame> games, bool isCompletions);
}
