using BcmApi.Services;
using Moq;

namespace BcmApi.Tests;

public class ParserTests
{    
  [Theory]
  [InlineData("1,137 / 1,137", 1137)]
  [InlineData("0 / 5,871", 0)]
  public void Properly_Parses_Users_Game_TAScore(string unparsedTaScore, int expectedTaScore)
  {
    var _parse = new Parser();
    int parsedResult = _parse.UsersGameTaScore(unparsedTaScore);
    Assert.Equal(expectedTaScore, parsedResult);
  }

  [Theory]
  [InlineData("1,137 / 1,137", 1137)]
  [InlineData("0 / 5,871", 5871)]
  public void Properly_Parses_Game_Total_TAScore(string unparsedTaScore, int expectedTaScore)
  {
    var _parse = new Parser();
    int parsedResult = _parse.GameTotalTaScore(unparsedTaScore);
    Assert.Equal(expectedTaScore, parsedResult);
  }
}
