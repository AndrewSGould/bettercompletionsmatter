using Tavis.Models;

namespace TavisApi.Services;

public interface IBcmService
{
  List<Player> GetPlayers();
  DateTime? GetContestStartDate();
  double? CalcBcmValue(double? ratio, double? estimate);
}