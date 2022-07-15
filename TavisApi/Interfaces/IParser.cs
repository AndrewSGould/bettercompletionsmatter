using Tavis.Models;

namespace TavisApi.Services;

public interface IParser
{
  int? PlayersGameSlashedValue(string unparsed);
  int? GameTotalSlashedValue(string unparsed);
  Platform GamePlatform(string unparsed);
  DateTime? TaDate(string unparsed);
  Ownership GameOwnership(string unparsed);
  bool GameNotForContests(string unparsed);
  int GamersCount(string unparsed);
  double BaseGameCompletionEstimate(string unparsed);
  double DecimalString(string unparsed);
  bool Unobtainables(string unparsed);
  double? GameSize(string unparsed);
  double? FullCompletionEstimate(string unparsed);
  int GameId(string unparsed);
  string? GameUrl(string unparsed);
}