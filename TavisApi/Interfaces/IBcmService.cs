using Tavis.Models;

namespace TavisApi.Services;

public interface IBcmService
{
  List<Player> GetPlayers();
  DateTime? GetContestStartDate();
  int? CalcBcmValue(double? ratio, double? estimate);
}