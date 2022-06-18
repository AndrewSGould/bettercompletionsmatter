using System.Globalization;
using Tavis.Models;

namespace TavisApi.Services 
{
  // Performance sensitive parsing for TrueAchievement specific inputs
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

//TODO: should this instead be a collection of static classes?
//TODO: improve performance here
  public class Parser : IParser 
  {
    public int? PlayersGameSlashedValue(string unparsed) {
      try {
        if (unparsed == "" || unparsed == "-" || unparsed == null) return null;

        int index = unparsed.IndexOf('/');
        string sub = "";
        if (index >= 0)
          sub = unparsed.Substring(0, index).Trim();

        return int.Parse(sub, NumberStyles.AllowThousands);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse a PlayersGameSlashedValue of {unparsed}");
        throw new Exception($"Error parsing PlayersGameSlashedValue with {unparsed}", ex);
      }
    }

    public int? GameTotalSlashedValue(string unparsed) {
      try {
        if (unparsed == "" || unparsed == "-" || unparsed == null) return null;

        int index = unparsed.IndexOf('/');
        string sub = "";
        if (index >= 0)
          sub = unparsed.Substring(unparsed.LastIndexOf('/') + 1).Trim();

        return int.Parse(sub, NumberStyles.AllowThousands);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse a GameTotalSlashedValue of {unparsed}");
        throw new Exception($"Error parsing GameTotalSlashedValue with {unparsed}", ex);
      }
    }

    public Platform GamePlatform(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return Platform.None;

        var leftOfTitle = unparsed.Split(new string[]{"alt=\""}, StringSplitOptions.None).Last();
        var result = leftOfTitle.Substring(0,leftOfTitle.IndexOf('"'));
        return Platform.FromName(result);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse a GamePlatform of {unparsed}");
        throw new Exception($"Error parsing GamePlatform with {unparsed}", ex);
      }
    }

    public int GameId(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) 
        throw new Exception("GameId was unable to be parsed! " + unparsed);

        var trimLeftOfId = unparsed.Split(new string[]{"tdPlatform_"}, StringSplitOptions.None).Last().Trim();
        var parsedGameId = trimLeftOfId.Substring(0,trimLeftOfId.IndexOf('"'));

        return int.Parse(parsedGameId);
      }
      catch (Exception ex) {
        Console.WriteLine($"Unable to parse a GameId of {unparsed}");
        throw new Exception($"Error parsing GameId with {unparsed}", ex);
      }
    }

    public DateTime? TaDate(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return null;

        if (unparsed == "Today")
          return Convert.ToDateTime(DateTime.Today);
        if (unparsed == "Yesterday")
          return Convert.ToDateTime(DateTime.Today.AddDays(-1));
        if (unparsed.Count() == 4)
          unparsed = "01/01/" + unparsed;

        return Convert.ToDateTime(unparsed);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse a TaDate of {unparsed}");
        throw new Exception($"Error parsing TaDate with {unparsed}", ex);
      }
    }

    public Ownership GameOwnership(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return Ownership.None;
        return Ownership.FromName(unparsed);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse Ownership of {unparsed}");
        throw new Exception($"Error parsing Ownership with {unparsed}", ex);
      }
    }

    public bool GameNotForContests(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return false;

        var leftOfTitle = unparsed.Split(new string[]{"alt=\""}, StringSplitOptions.None).Last();
        var result = leftOfTitle.Substring(0,leftOfTitle.IndexOf('"')).Trim();
        if (result == "Not for contests")
          return true;
          
        return false;
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse NotForContests of {unparsed}");
        throw new Exception($"Error parsing NotForContests with {unparsed}", ex);
      }
    }

    public int GamersCount(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return -1;
        return int.Parse(unparsed, NumberStyles.AllowThousands);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse GamersCount of {unparsed}");
        throw new Exception($"Error parsing GamersCount with {unparsed}", ex);
      }
    }

    public double BaseGameCompletionEstimate(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return -1;

        if (unparsed.Contains("1000+")) return 1000;
        if (unparsed.Contains("200+")) return 200;

        var removedHours = unparsed.Split(' ', StringSplitOptions.None).First();
        var result = removedHours.Split('-', StringSplitOptions.None).Last();

        return double.Parse(result);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse BaseGameCompletionEstimate of {unparsed}");
        throw new Exception($"Error parsing BaseGameCompletionEstimate with {unparsed}", ex);
      }
    }

    public double DecimalString(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return 0.0;
        return double.Parse(unparsed);
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse DecimalString of {unparsed}");
        throw new Exception($"Error parsing DecimalString with {unparsed}", ex);
      }
    }

    public bool Unobtainables(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return false;
      
        var leftOfTitle = unparsed.Split(new string[]{"alt=\""}, StringSplitOptions.None).Last().Trim();
        var result = leftOfTitle.Substring(0,leftOfTitle.IndexOf('"')).Trim();

        if (result == "Unobtainable" || result == "Discontinued" || result == "Partly Discontinued/Unobtainable")
          return true;

        return false;
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse BaseGameCompletionEstimate of {unparsed}");
        throw new Exception($"Error parsing BaseGameCompletionEstimate with {unparsed}", ex);
      }
    }

    public double? GameSize(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return null;

        var unparsedArray = unparsed.Select(x => new string(x, 1)).ToArray();

        var scale = "";
        var parsedDouble = "";

        foreach (var character in unparsedArray) {
          if (int.TryParse(character, out int number))
            parsedDouble += character;
          else if (character == ".")
            parsedDouble += ".";
          else if (character == ",")
            continue;
          else if (character == "M") {
            scale = "MB";
            break;
          }
          else if (character == "G") {
            scale = "GB";
            break;
          }
          else if (character == "K") {
            scale = "KB";
            break;
          }
          else 
            throw new Exception("Error parsing install size");
        }

        var size = double.Parse(parsedDouble);

        if (scale == "GB")
          size = size * 1000;
        else if (scale == "KB")
          size = size / 1000;

        return size;
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse GameSize of {unparsed}");
        throw new Exception($"Error parsing GameSize with {unparsed}", ex);
      }
    }

    public double? FullCompletionEstimate(string unparsed) {
      try{
        if (unparsed == "" || unparsed == null) return null;

        if (unparsed.Contains("1000+"))
          return 1000;
        else if (unparsed.Contains("200+"))
          return 200;
        else {
          var gameTime = unparsed.Substring(unparsed.LastIndexOf('-') + 1).TrimEnd('h');
          return double.Parse(gameTime);
        }
      }
      
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse FullCompletionEstimate of {unparsed}");
        throw new Exception($"Error parsing FullCompletionEstimate with {unparsed}", ex);
      }
    }

    public string? GameUrl(string unparsed) {
      try {
        if (unparsed == "" || unparsed == null) return null;

        var trimLeft = unparsed.Split(new string[]{"<a href=\""}, StringSplitOptions.None).Last().Trim();
        var trimRight = trimLeft.Split(new string[]{"achievements?"}, StringSplitOptions.None).First().Trim();

        return trimRight;
      }
      catch(Exception ex) {
        Console.WriteLine($"Unable to parse GameUrl of {unparsed}");
        throw new Exception($"Error parsing GameUrl with {unparsed}", ex);
      }
    }
  }
}
