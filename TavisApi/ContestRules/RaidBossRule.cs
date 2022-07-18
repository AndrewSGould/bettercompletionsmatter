using System.Collections.ObjectModel;
using Tavis.Models;

namespace TavisApi.ContestRules;

public class RaidBossRule {

  public static double RaidBossHp = 3668;

  public RaidBossPlayer GenerateRaidPlayer(Player player) {
    var raidBossPlayer = new RaidBossPlayer();

    raidBossPlayer.Id = player.Id;
    raidBossPlayer.Name = player.Name;

    if (player.Name == "eohjay" || player.Name == "FreakyRO" || player.Name == "Mental Knight 5" || player.Name == "Legohead 1977"
      || player.Name == "A1exRD" || player.Name == "Ahayzo" || player.Name == "BenL72" || player.Name == "ChewieOnIce" || player.Name == "ChinDocta"
      || player.Name == "Erutaerc" || player.Name == "FlutteryChicken" || player.Name == "Icefiretn" || player.Name == "Infamous"
      || player.Name == "Jblacq" || player.Name == "JimbotUK" || player.Name == "mdp 73" || player.Name == "nightw0lf" || player.Name == "N龱T廾 T廾A G龱D"
      || player.Name == "Skeptical Mario" || player.Name == "Vulgar Latin" || player.Name == "Wakapeil")
      raidBossPlayer.Level = 2;
    else 
      raidBossPlayer.Level = 1;

    switch(player.Name)
    {
      case "eohjay":
        raidBossPlayer.AttackApproach = "uses the Staff of Aykayohohzedaybee for";
        break;
      case "FreakyRO":
        raidBossPlayer.AttackApproach = "flails weirdly for";
        break;
      case "lucas1987":
        raidBossPlayer.AttackApproach = "shoots his longbow for";
        break;
      case "Inigomontoya80":
        raidBossPlayer.AttackApproach = "uses Bonetti's Defense for";
        break;
      case "Mental Knight 5":
        raidBossPlayer.AttackApproach = "summons the Great Gif for";
        break;
      case "Legohead 1977":
        raidBossPlayer.AttackApproach = "throws down LEGO caltrops for";
        break;
      case "logicslayer":
        raidBossPlayer.AttackApproach = "defies logic for";
        break;
      case "IronFistofSnuff":
        raidBossPlayer.AttackApproach = "punches for";
        break;
      case "Sir Paulygon":
        raidBossPlayer.AttackApproach = "attacks with a Granstaff for";
        break;
      case "A1exRD":
        raidBossPlayer.AttackApproach = "writes a fifteen page framework  explaining his attack for";
        break;
      case "Ace":
        raidBossPlayer.AttackApproach = "slings Magma for";
        break;
      case "Ahayzo":
        raidBossPlayer.AttackApproach = "swings the fiery blade of Arizona for";
        break;
      case "Big Ell":
        raidBossPlayer.AttackApproach = "summons a sandwich for";
        break;
      case "ChewieOnIce":
        raidBossPlayer.AttackApproach = "rips off arms for";
        break;
      case "ChinDocta":
        raidBossPlayer.AttackApproach = "uppercuts for";
        break;
      case "CrunchyGoblin68":
        raidBossPlayer.AttackApproach = "crunches bones for";
        break;
      case "Darklord Davis":
        raidBossPlayer.AttackApproach = "enters for";
        break;
      case "emz fergi":
        raidBossPlayer.AttackApproach = "summons an Insect Swarm for";
        break;
      case "Enigma Gamer 77":
        raidBossPlayer.AttackApproach = "mysteriously strikes for";
        break;
      case "Erutaerc":
        raidBossPlayer.AttackApproach = "skcatta rof";
        break;
      case "FlutteryChicken":
        raidBossPlayer.AttackApproach = "pecks for";
        break;
      case "Hatton90":
        raidBossPlayer.AttackApproach = "bends it like beckham for";
        break;
      case "HawkeyeBarry20":
        raidBossPlayer.AttackApproach = "shoots for";
        break;
      case "Icefiretn":
        raidBossPlayer.AttackApproach = "icyhots for";
        break;
      case "IcyThrasher":
        raidBossPlayer.AttackApproach = "directs turret fire for";
        break;
      case "ITS ALivEx":
        raidBossPlayer.AttackApproach = "reanimates for";
        break;
      case "Jblacq":
        raidBossPlayer.AttackApproach = "jattacqs with their jbattleacqs for";
        break;
      case "JimbotUK":
        raidBossPlayer.AttackApproach = "casts Magic Mamamoo for";
        break;
      case "KooshMoose":
        raidBossPlayer.AttackApproach = "charges for";
        break;
      case "kT Echo":
        raidBossPlayer.AttackApproach = "orders T.A.V.I.S. to attack for";
        break;
      case "kungfuskills":
        raidBossPlayer.AttackApproach = "karate chops for";
        break;
      case "MadLefty2097":
        raidBossPlayer.AttackApproach = "uses Southpaw Stance for";
        break;
      case "Matrarch":
        raidBossPlayer.AttackApproach = "ducks for";
        break;
      case "mdp 73":
        raidBossPlayer.AttackApproach = "gem swaps for";
        break;
      case "NBA Kirkland":
        raidBossPlayer.AttackApproach = "slamdunks for";
        break;
      case "nightw0lf":
        raidBossPlayer.AttackApproach = "bites for";
        break;
      case "N龱T廾 T廾A G龱D":
        raidBossPlayer.AttackApproach = "breaks the game for";
        break;
      case "rawkerdude":
        raidBossPlayer.AttackApproach = "wields a lawnmower for";
        break;
      case "Shadow":
        raidBossPlayer.AttackApproach = "backstabs for";
        break;
      case "Skeptical Mario":
        raidBossPlayer.AttackApproach = "sets off a trap for";
        break;
      case "tatersoup19":
        raidBossPlayer.AttackApproach = "boils 'em, mashes 'em, sticks 'em in a stew for";
        break;
      case "Vulgar Latin":
        raidBossPlayer.AttackApproach = "insults the enemy by rapping 'Be a Man' for";
        break;
      case "Wakapeil":
        raidBossPlayer.AttackApproach = "throws a halo for";
        break;
      case "Whtthfgg":
        raidBossPlayer.AttackApproach = "ttcksfr";
        break;
      case "xI The Rock Ix":
        raidBossPlayer.AttackApproach = "peoples' elbows for";
        break;
      case "xLAx JesteR":
        raidBossPlayer.AttackApproach = "jokes for";
        break;
      case "Xpovos":
        raidBossPlayer.AttackApproach = "attacks with a sickle for";
        break;
      default:
        raidBossPlayer.AttackApproach = "attacks for";
        break;
    }

    return raidBossPlayer;
  }

  public double CalcDamageWithLevelModifier(double damage, RaidBossPlayer player) {
    switch(player.Level) 
    {
      case 1:
        return damage;
      case 2:
        return damage * 1.1;
      default: {
        Console.WriteLine($"ERROR: Could not determine player level for {player.Name}");
        return damage;
      }   
    }
  }
}

public class RaidBossPlayer : Player {
  public int Level {get; set;}
  public string? AttackApproach {get;set;}
}