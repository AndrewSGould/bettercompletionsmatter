using System.Globalization;

namespace BcmApi.Services 
{
  // Performance sensitive parsing for TrueAchievement specific inputs
  public interface IParser
  {
    int UsersGameTaScore(string unparsed);
    int GameTotalTaScore(string unparsed);
    // uint UsersGameGamerscore();
    // uint GameTotalGamerscore();
    // float UsersGameCompletionPercentage();
    // float UsersGameRatio();
    // // UsersGameRating?
    // // Time Played?
    // DateTime UsersGameStartDate();
    // DateTime UsersGameCompletedDate();
    // DateTime UsersGameLastUnlock();
    // //Ownership, enum?
    // //Media, enum?
    // //Play Status, enum?
    // bool UsersGameForContest();
    // DateTime GameReleaseDate();
  }

  public class Parser : IParser 
  {
    public int UsersGameTaScore(string unparsed) {
      int index = unparsed.IndexOf('/');
      string sub = "";
      if (index >= 0)
        sub = unparsed.Substring(0, index).Trim();

      return int.Parse(sub, NumberStyles.AllowThousands);
    }

    public int GameTotalTaScore(string unparsed) {
      int index = unparsed.IndexOf('/');
      string sub = "";
      if (index >= 0)
        sub = unparsed.Substring(unparsed.LastIndexOf('/') + 1).Trim();

      return int.Parse(sub, NumberStyles.AllowThousands);
    }
  }
}
