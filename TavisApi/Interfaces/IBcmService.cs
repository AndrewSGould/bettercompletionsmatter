using Tavis.Models;

namespace TavisApi.Services;

public interface IBcmService
{
  List<BcmPlayer> GetPlayers();
  DateTime? GetContestStartDate();
  int? CalcBcmValue(int platformId, double? ratio, double? estimate);
  Task<List<string>> GetAlphabetChallengeProgress(long playerId);
  Task<List<Game>> GetOddJobChallengeProgress(long playerId);
  int GetParticipationProgress(BcmPlayer player);
  long? GetRegistrationId();
}
