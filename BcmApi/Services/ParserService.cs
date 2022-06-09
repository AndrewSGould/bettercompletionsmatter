using System.Globalization;
using Bcm.Models;

namespace BcmApi.Services 
{
  // Performance sensitive parsing for TrueAchievement specific inputs
  public interface IParser
  {
    int UsersGameSlashedValue(string unparsed);
    int GameTotalSlashedValue(string unparsed);
    Platform GamePlatform(string unparsed);
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

//TODO: improve performance here
  public class Parser : IParser 
  {
    public int UsersGameSlashedValue(string unparsed) {
      int index = unparsed.IndexOf('/');
      string sub = "";
      if (index >= 0)
        sub = unparsed.Substring(0, index).Trim();

      return int.Parse(sub, NumberStyles.AllowThousands);
    }

    public int GameTotalSlashedValue(string unparsed) {
      int index = unparsed.IndexOf('/');
      string sub = "";
      if (index >= 0)
        sub = unparsed.Substring(unparsed.LastIndexOf('/') + 1).Trim();

      return int.Parse(sub, NumberStyles.AllowThousands);
    }

    public Platform GamePlatform(string unparsed) {
      var leftOfTitle = unparsed.Split(new string[]{"title=\""}, StringSplitOptions.None).Last();
      var result = leftOfTitle.Substring(0,leftOfTitle.IndexOf('"'));
      return Platform.FromName(result);
    }

    public DateTime? TaDate(string unparsed) {
      return Convert.ToDateTime(unparsed);
    }

    public Ownership GameOwnership(string unparsed) {
      return Ownership.FromName(unparsed);
    }

    public bool GameForContests(string unparsed) {
      if (unparsed == "" || unparsed == null) return false;

      var leftOfTitle = unparsed.Split(new string[]{"title=\""}, StringSplitOptions.None).Last();
      var result = leftOfTitle.Substring(0,leftOfTitle.IndexOf('"')).Trim();
      if (result == "Not for contests")
        return true;
        
      return false;
    }

    public int GamersCount(string unparsed) {
      if (unparsed == "" || unparsed == null) return -1;

      return int.Parse(unparsed, NumberStyles.AllowThousands);
    }

    public float BaseGameCompletionEstimate(string unparsed) {
      if (unparsed == "" || unparsed == null) return -1;

      if (unparsed.StartsWith("1000")) return 1000;

      var removedHours = unparsed.Split(' ', StringSplitOptions.None).First();
      var result = removedHours.Split('-', StringSplitOptions.None).Last();

      return float.Parse(result);
    }

    // ratio
    // rating
    // u/d/pdu
    // server closure
    // install size
    // compl incl dlc
  }
}
