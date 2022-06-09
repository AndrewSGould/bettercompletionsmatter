using Bcm.Models;
using BcmApi.Services;
using Moq;
using static BcmApi.Services.Parser;

namespace BcmApi.Tests;

public class ParserTests
{    
  [Theory]
  [InlineData("1,137 / 1,137", 1137)]
  [InlineData("534 / 20,021", 534)]
  [InlineData("0 / 86", 0)]
  public void Properly_Parses_Users_Game_TAScore(string unparsedTaScore, int expectedTaScore)
  {
    var _parse = new Parser();
    int parsedResult = _parse.UsersGameSlashedValue(unparsedTaScore);
    Assert.Equal(expectedTaScore, parsedResult);
  }

  [Theory]
  [InlineData("1,137 / 1,137", 1137)]
  [InlineData("534 / 20,021", 20021)]
  [InlineData("0 / 0", 0)]
  [InlineData("0 / 86", 86)]
  public void Properly_Parses_Game_Total_TAScore(string unparsedTaScore, int expectedTaScore)
  {
    var _parse = new Parser();
    int parsedResult = _parse.GameTotalSlashedValue(unparsedTaScore);
    Assert.Equal(expectedTaScore, parsedResult);
  }

  [Theory]
  [InlineData("<img src=\"/images/platforms/xbox-one.png\" alt=\"xbox-one\" title=\"xbox-one\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "xbox-one")]
  [InlineData("<img src=\"/images/platforms/xbox-360.png\" alt=\"xbox-360\" title=\"xbox-360\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "xbox-360")]
  [InlineData("<img src=\"/images/platforms/windows.png\" alt=\"windows\" title=\"windows\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "windows")]
  [InlineData("<img src=\"/images/platforms/xbox-series-x-s.png\" alt=\"xbox-series-x-s\" title=\"xbox-series-x-s\" width=\"43\" height=\"24\" loading=\"lazy\">"
    , "xbox-series-x-s")]
  [InlineData("<img src=\"/images/platforms/windows-phone.png\" alt=\"windows-phone\" title=\"windows-phone\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "windows-phone")]
  [InlineData("<img src=\"/images/platforms/games-for-windows-live.png\" alt=\"games-for-windows-live\" title=\"games-for-windows-live\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "games-for-windows-live")]
  [InlineData("<img src=\"/images/platforms/web.png\" alt=\"web\" title=\"web\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "web")]
  [InlineData("<img src=\"/images/platforms/ios.png\" alt=\"ios\" title=\"ios\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "ios")]
  [InlineData("<img src=\"/images/platforms/android.png\" alt=\"android\" title=\"android\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "android")]
  [InlineData("<img src=\"/images/platforms/apple-tv.png\" alt=\"apple-tv\" title=\"apple-tv\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "apple-tv")]
  [InlineData("<img src=\"/images/platforms/nintendo-switch.png\" alt=\"nintendo-switch\" title=\"nintendo-switch\" width=\"32\" height=\"32\" loading=\"lazy\">"
    , "nintendo-switch")]
  public void Properly_Parses_Game_Platform(string unparsedPlatform, string expectedPlatform) 
  {
    var _parse = new Parser();
    var parsedResult = _parse.GamePlatform(unparsedPlatform);
    Assert.IsType(typeof(Platform), parsedResult);
    Assert.Equal(parsedResult.Name, expectedPlatform);
  }

  [Theory]
  [InlineData("<td id=\"tdNotForContests_9781\"><img src=\"/images/icons/noentry.png\" alt=\"Not for contests\" title=\"Not for contests\" width=\"16\" height=\"16\"></td>"
    , true)]
  [InlineData("", false)]
  public void Properly_Parses_Game_ForContests(string unparsedContests, bool expectedContestStatus) 
  {
    var _parse = new Parser();
    var result = _parse.GameForContests(unparsedContests);
    Assert.Equal(result, expectedContestStatus);
  }

  [Theory]
  [InlineData("0 / 2,000", 0)]
  [InlineData("260 / 3,800", 260)]
  public void Properly_Parses_Users_Game_Gamerscore(string unparsedGamerscore, int expectedGamerscore)
  {
    var _parse = new Parser();
    int parsedResult = _parse.UsersGameSlashedValue(unparsedGamerscore);
    Assert.Equal(expectedGamerscore, parsedResult);
  }

  [Theory]
  [InlineData("0 / 2,000", 2000)]
  [InlineData("260 / 3,800", 3800)]
  [InlineData("0 / 0", 0)]
  public void Properly_Parses_Game_Total_Gamerscore(string unparsedGamerscore, int expectedGamerscore)
  {
    var _parse = new Parser();
    int parsedResult = _parse.GameTotalSlashedValue(unparsedGamerscore);
    Assert.Equal(expectedGamerscore, parsedResult);
  }

  [Theory]
  [InlineData("12 / 400", 12)]
  [InlineData("0 / 10", 0)]
  public void Properly_Parses_Users_Game_Achievements(string unparsedGamerscore, int expectedGamerscore) {
    var _parse = new Parser();
    int parsedResult = _parse.UsersGameSlashedValue(unparsedGamerscore);
    Assert.Equal(expectedGamerscore, parsedResult);
  }

  [Theory]
  [InlineData("12 / 400", 400)]
  [InlineData("0 / 10", 10)]
  [InlineData("0 / 0", 0)]
  public void Properly_Parses_Game_Total_Achievements(string unparsedGamerscore, int expectedGamerscore) {
    var _parse = new Parser();
    int parsedResult = _parse.GameTotalSlashedValue(unparsedGamerscore);
    Assert.Equal(expectedGamerscore, parsedResult);
  }

  [Fact]
  public void Properly_Parses_TaDate() {
    var _parse = new Parser();
    DateTime? parsedDate = _parse.TaDate("10 Oct 14");
    Assert.IsType(typeof(DateTime?), parsedDate);
  }

  [Theory]
  [InlineData("Owned", "Owned")]
  [InlineData("Loaned out", "Loaned out")]
  [InlineData("Looking to trade/sell", "Looking to trade/sell")]
  [InlineData("Borrowing", "Borrowing")]
  [InlineData("No longer have", "No longer have")]
  [InlineData("Renting", "Renting")]
  [InlineData("Games with Gold", "Games with Gold")]
  [InlineData("Ordered", "Ordered")]
  [InlineData("Trial", "Trial")]
  [InlineData("EA Play", "EA Play")]
  [InlineData("Unable to play", "Unable to play")]
  [InlineData("Xbox Game Pass", "Xbox Game Pass")]
  [InlineData("Xbox Game Pass PC", "Xbox Game Pass PC")]
  public void Properly_Parses_Game_Ownership(string unparsedOwnership, string expectedOwnership) 
  {
    var _parse = new Parser();
    var parsedResult = _parse.GameOwnership(unparsedOwnership);
    Assert.IsType(typeof(Ownership), parsedResult);
    Assert.Equal(parsedResult.Name, expectedOwnership);
  }

  [Theory]
  [InlineData("1,562,511", 1562511)]
  [InlineData("466,828", 466828)]
  [InlineData("13", 13)]
  [InlineData("0", 0)] 
  public void Properly_Parses_GamersCount(string unparsedGamersCount, int expectedCount) {
    var _parse = new Parser();
    var parsedCount = _parse.GamersCount(unparsedGamersCount);
    Assert.Equal(parsedCount, expectedCount);
  }

  [Theory]
  [InlineData("0-0.5 hours", .5)]
  [InlineData("200-300 hours", 300)]
  [InlineData("1000+ hours", 1000)]
  public void Properly_Parses_BaseGameCompletionEstimate(string unparsedBaseGameEstimate, float expectedEstimate) {
    var _parse = new Parser();
    var parsedEstimate = _parse.BaseGameCompletionEstimate(unparsedBaseGameEstimate);
    Assert.Equal(parsedEstimate, expectedEstimate);
  }
}
