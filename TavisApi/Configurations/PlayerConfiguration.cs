using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
  public void Configure(EntityTypeBuilder<Player> builder)
  {
    builder.HasData
    (
      new Player {
        Id = 1,
        TrueAchievementId = 104571,
        Name = "kT Echo",
        Region = "United States",
        Area = "Ohio",
        IsActive = true
      },
      new Player {
        Id = 2,
        TrueAchievementId = 266752,
        Name = "eohjay",
        Region = "United States",
        Area = "Ohio",
        IsActive = true
      },
      new Player {
        Id = 3,
        TrueAchievementId = 405202,
        Name = "IronFistofSnuff",
        Region = "United States",
        Area = "Ohio",
        IsActive = true
      },
      new Player {
        Id = 4,
        TrueAchievementId = 461682,
        Name = "zzScanMan1",
        Region = null,
        Area = null,
        IsActive = false
      },
      new Player {
        Id = 5,
        TrueAchievementId = 691631,
        Name = "SwiftSupafly",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 6,
        TrueAchievementId = 644155,
        Name = "DubstepEdgelord",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 7,
        TrueAchievementId = 518665,
        Name = "Infamous",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 8,
        TrueAchievementId = 1013881,
        Name = "Luke17000",
        Region = "United States",
        Area = "California",
        IsActive = true
      },
      new Player {
        Id = 9,
        TrueAchievementId = 4838,
        Name = "lucas1987",
        Region = "United States",
        Area = "Kentucky",
        IsActive = true
      },
      new Player {
        Id = 10,
        TrueAchievementId = 695506,
        Name = "Fine Feat",
        Region = "Austrailia",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 11,
        TrueAchievementId = 1815,
        Name = "smrnov",
        Region = "Canada",
        Area = "Ontario",
        IsActive = true
      },
      new Player {
        Id = 12,
        TrueAchievementId = 746750,
        Name = "Sir Paulygon",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 13,
        TrueAchievementId = 385301,
        Name = "Rossco7530",
        Region = "Austrailia",
        Area = "Victoria",
        IsActive = true
      },
      new Player {
        Id = 14,
        TrueAchievementId = 992494,
        Name = "True Veteran",
        Region = "Austrailia",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 15,
        TrueAchievementId = 561724,
        Name = "CasualExile",
        Region = "New Zealand",
        Area = "New Zealand",
        IsActive = true
      },
      new Player {
        Id = 16,
        TrueAchievementId = 644384,
        Name = "darkwing1232",
        Region = "United States",
        Area = "Wisconsin",
        IsActive = true
      },
      new Player {
        Id = 17,
        TrueAchievementId = 567743,
        Name = "JimbotUK",
        Region = "England",
        Area = "Suffolk",
        IsActive = true
      },
      new Player {
        Id = 18,
        TrueAchievementId = 312377,
        Name = "xMagicMunKix",
        Region = "Germany",
        Area = "Hesse",
        IsActive = true
      },
      new Player {
        Id = 19,
        TrueAchievementId = 405939,
        Name = "Inferno118",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 20,
        TrueAchievementId = 503921,
        Name = "AC Rock3tman",
        Region = "Germany",
        Area = "Hesse",
        IsActive = true
      },
      new Player {
        Id = 21,
        TrueAchievementId = 585579,
        Name = "tackleglass54",
        Region = "United States",
        Area = "Michigan",
        IsActive = true
      },
      new Player {
        Id = 22,
        TrueAchievementId = 115320,
        Name = "IcyThrasher",
        Region = "United States",
        Area = "Illinois",
        IsActive = true
      },
      new Player {
        Id = 23,
        TrueAchievementId = 1872,
        Name = "TXMOOK",
        Region = "United States",
        Area = "Texas",
        IsActive = true
      },
      new Player {
        Id = 24,
        TrueAchievementId = 5540,
        Name = "BemusedBox",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 25,
        TrueAchievementId = 292068,
        Name = "nuttywray",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 26,
        TrueAchievementId = 441743,
        Name = "Christoph 5782",
        Region = "United States",
        Area = "Missouri",
        IsActive = true
      },
      new Player {
        Id = 27,
        TrueAchievementId = 8962,
        Name = "Mtld",
        Region = "Canada",
        Area = "Quebec",
        IsActive = true
      },
      new Player {
        Id = 28,
        TrueAchievementId = 978641,
        Name = "Lw N1GHTM4R3",
        Region = "United States",
        Area = "Virginia",
        IsActive = true
      },
      new Player {
        Id = 29,
        TrueAchievementId = 75572,
        Name = "Erutaerc",
        Region = "England",
        Area = "Nottinghamshire",
        IsActive = true
      },
      new Player {
        Id = 30,
        TrueAchievementId = 332637,
        Name = "WildWhiteNoise",
        Region = "Hungary",
        Area = "Hungary",
        IsActive = true
      },
      new Player {
        Id = 31,
        TrueAchievementId = 767838,
        Name = "GD GodSpeed",
        Region = "Germany",
        Area = "Rhineland-Palatinate",
        IsActive = true
      },
      new Player {
        Id = 32,
        TrueAchievementId = 273370,
        Name = "Legohead 1977",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 33,
        TrueAchievementId = 1001899,
        Name = "Saurvivalist",
        Region = "United States",
        Area = "California",
        IsActive = true
      },
      new Player {
        Id = 34,
        TrueAchievementId = 386783,
        Name = "UltimateDespair",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 35,
        TrueAchievementId = 483713,
        Name = "radnonnahs",
        Region = "United States",
        Area = "Michigan",
        IsActive = true
      },
      new Player {
        Id = 36,
        TrueAchievementId = 316981,
        Name = "Wakapeil",
        Region = "Sweden",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 37,
        TrueAchievementId = 661240,
        Name = "DaDuelingDonuts",
        Region = "United States",
        Area = "North Carolina",
        IsActive = true
      },
      new Player {
        Id = 38,
        TrueAchievementId = 251492,
        Name = "Oriole2682",
        Region = "United States",
        Area = "New Jersey",
        IsActive = true
      },
      new Player {
        Id = 39,
        TrueAchievementId = 18165,
        Name = "NBA Kirkland",
        Region = "United States",
        Area = "Washington",
        IsActive = true
      },
      new Player {
        Id = 40,
        TrueAchievementId = 540888,
        Name = "Xx Phatryda xX",
        Region = "United States",
        Area = "North Carolina",
        IsActive = true
      },
      new Player {
        Id = 41,
        TrueAchievementId = 514795,
        Name = "Fista Roboto",
        Region = "Austrailia",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 42,
        TrueAchievementId = 431830,
        Name = "HegemonicHater",
        Region = "United States",
        Area = "Florida",
        IsActive = true
      },
      new Player {
        Id = 43,
        TrueAchievementId = 109626,
        Name = "Team Brether",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 44,
        TrueAchievementId = 17362,
        Name = "Mattism",
        Region = "United States",
        Area = "Oklahoma",
        IsActive = true
      },
      new Player {
        Id = 45,
        TrueAchievementId = 667139,
        Name = "Alyssiya",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 46,
        TrueAchievementId = 389388,
        Name = "MajinFro",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 47,
        TrueAchievementId = 460875,
        Name = "Whisperin Clown",
        Region = "United States",
        Area = "Florida",
        IsActive = true
      },
      new Player {
        Id = 48,
        TrueAchievementId = 273989,
        Name = "Mental Knight 5",
        Region = "United States",
        Area = "New Hampshire",
        IsActive = true
      },
      new Player {
        Id = 49,
        TrueAchievementId = 473608,
        Name = "ChinDocta",
        Region = "Austrailia",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 50,
        TrueAchievementId = 108134,
        Name = "A1exRD",
        Region = "England",
        Area = "Devon",
        IsActive = true
      },
      new Player {
        Id = 51,
        TrueAchievementId = 662432,
        Name = "Ethigy",
        Region = "United States",
        Area = "Washington",
        IsActive = true
      },
      new Player {
        Id = 52,
        TrueAchievementId = 596837,
        Name = "Nichtl",
        Region = "Germany",
        Area = "Rhineland-Palatinate",
        IsActive = true
      },
      new Player {
        Id = 53,
        TrueAchievementId = 507793,
        Name = "PangoBara",
        Region = "United States",
        Area = "Pennsylvania",
        IsActive = true
      },
      new Player {
        Id = 54,
        TrueAchievementId = 548044,
        Name = "HawkeyeBarry20",
        Region = "United States",
        Area = "Iowa",
        IsActive = true
      },
      new Player {
        Id = 55,
        TrueAchievementId = 636969,
        Name = "KawiNinjaRider7",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 56,
        TrueAchievementId = 280034,
        Name = "BenL72",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 57,
        TrueAchievementId = 276088,
        Name = "Freamwhole",
        Region = "United States",
        Area = "Minnesota",
        IsActive = true
      },
      new Player {
        Id = 58,
        TrueAchievementId = 78779,
        Name = "Proulx",
        Region = "United States",
        Area = "Virginia",
        IsActive = true
      },
      new Player {
        Id = 59,
        TrueAchievementId = 60207,
        Name = "zzUrbanSpaceman",
        Region = "Austrailia",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 60,
        TrueAchievementId = 27797,
        Name = "A 0 E Monkey",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 61,
        TrueAchievementId = 38595,
        Name = "Majinbro",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 62,
        TrueAchievementId = 126013,
        Name = "Icefiretn",
        Region = "United States",
        Area = "Connecticut",
        IsActive = true
      },
      new Player {
        Id = 63,
        TrueAchievementId = 130600,
        Name = "LORDOFDOOKIE69",
        Region = "United States",
        Area = "Massachusetts",
        IsActive = true
      },
      new Player {
        Id = 64,
        TrueAchievementId = 685511,
        Name = "RetroChief1969",
        Region = "United States",
        Area = "North Carolina",
        IsActive = true
      },
      new Player {
        Id = 65,
        TrueAchievementId = 11497,
        Name = "EldritchSS",
        Region = "United States",
        Area = "California",
        IsActive = true
      },
      new Player {
        Id = 66,
        TrueAchievementId = 25889,
        Name = "rawkerdude",
        Region = "United States",
        Area = "Tennessee",
        IsActive = true
      },
      new Player {
        Id = 67,
        TrueAchievementId = 351310,
        Name = "iMaginaryy",
        Region = "United States",
        Area = "Illinois",
        IsActive = true
      },
      new Player {
        Id = 68,
        TrueAchievementId = 15074,
        Name = "Kez001",
        Region = "United States",
        Area = "South Carolina",
        IsActive = true
      },
      new Player {
        Id = 69,
        TrueAchievementId = 46893,
        Name = "WoodsMonk",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 70,
        TrueAchievementId = 391205,
        Name = "W1cked Girl",
        Region = "United States",
        Area = "Washington",
        IsActive = true
      },
      new Player {
        Id = 71,
        TrueAchievementId = 321249,
        Name = "Kitty Skies",
        Region = "Wales",
        Area = "Cardiff",
        IsActive = true
      },
      new Player {
        Id = 72,
        TrueAchievementId = 53128,
        Name = "MrGompers",
        Region = "United States",
        Area = "Connecticut",
        IsActive = true
      },
      new Player {
        Id = 73,
        TrueAchievementId = 991656,
        Name = "GTKrouwel83",
        Region = "Netherlands",
        Area = "Netherlands",
        IsActive = true
      },
      new Player {
        Id = 74,
        TrueAchievementId = 710980,
        Name = "CouchBurglar",
        Region = "United States",
        Area = "Kentucky",
        IsActive = true
      },
      new Player {
        Id = 75,
        TrueAchievementId = 391799,
        Name = "SprinkyDink",
        Region = "United States",
        Area = "Virginia",
        IsActive = true
      },
      new Player {
        Id = 76,
        TrueAchievementId = 57548,
        Name = "boldyno1",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 77,
        TrueAchievementId = 389092,
        Name = "FreakyRO",
        Region = "United States",
        Area = "Virginia",
        IsActive = true
      },
      new Player {
        Id = 78,
        TrueAchievementId = 259643,
        Name = "hotcurls3088",
        Region = "United States",
        Area = "Kansas",
        IsActive = true
      },
      new Player {
        Id = 79,
        TrueAchievementId = 714727,
        Name = "Kaiteh",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 80,
        TrueAchievementId = 107004,
        Name = "Yinga Garten",
        Region = "United Kingdom",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 81,
        TrueAchievementId = 47628,
        Name = "Skeptical Mario",
        Region = "United States",
        Area = "Minnesota",
        IsActive = true
      },
      new Player {
        Id = 82,
        TrueAchievementId = 286696,
        Name = "WildwoodMike",
        Region = "United States",
        Area = "New Jersey",
        IsActive = true
      },
      new Player {
        Id = 83,
        TrueAchievementId = 115479,
        Name = "ChewieOnIce",
        Region = "England",
        Area = "New Hampshire",
        IsActive = true
      },
      new Player {
        Id = 84,
        TrueAchievementId = 275395,
        Name = "Igneus DarkSide",
        Region = "Croatia",
        Area = "Croatia",
        IsActive = true
      },
      new Player {
        Id = 85,
        TrueAchievementId = 89301,
        Name = "DavidMcC1989",
        Region = "Ireland",
        Area = "Ireland",
        IsActive = true
      },
      new Player {
        Id = 86,
        TrueAchievementId = 378519,
        Name = "RadicalSniper99",
        Region = "United States",
        Area = "Pennsylvania",
        IsActive = true
      },
      new Player {
        Id = 87,
        TrueAchievementId = 4860,
        Name = "Hotdogmcgee",
        Region = "United States",
        Area = "Nevada",
        IsActive = true
      },
      new Player {
        Id = 88,
        TrueAchievementId = 332575,
        Name = "HenkeXD",
        Region = "Sweden",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 89,
        TrueAchievementId = 435315,
        Name = "KooshMoose",
        Region = "United States",
        Area = "Maryland",
        IsActive = true
      },
      new Player {
        Id = 90,
        TrueAchievementId = 58478,
        Name = "Xynvincible",
        Region = "United States",
        Area = "Virginia",
        IsActive = true
      },
      new Player {
        Id = 91,
        TrueAchievementId = 365315,
        Name = "Whtthfgg",
        Region = "United States",
        Area = "Wisconsin",
        IsActive = true
      },
      new Player {
        Id = 92,
        TrueAchievementId = 125044,
        Name = "PlayUltimate711",
        Region = "United States",
        Area = "Colorado",
        IsActive = true
      },
      new Player {
        Id = 93,
        TrueAchievementId = 119394,
        Name = "Seamus McLimey",
        Region = "Canada",
        Area = "British Columbia",
        IsActive = true
      },
      new Player {
        Id = 94,
        TrueAchievementId = 40704,
        Name = "Northern Lass",
        Region = "England",
        Area = "Lancashire",
        IsActive = true
      },
      new Player {
        Id = 95,
        TrueAchievementId = 37540,
        Name = "xNeo21x",
        Region = "United States",
        Area = "California",
        IsActive = true
      },
      new Player {
        Id = 96,
        TrueAchievementId = 401906,
        Name = "Redanian",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 97,
        TrueAchievementId = 64912,
        Name = "TBonePhone",
        Region = "United States",
        Area = "New Jersey",
        IsActive = true
      },
      new Player {
        Id = 98,
        TrueAchievementId = 734459,
        Name = "IxGermanBeastxI",
        Region = "Germany",
        Area = "Lower Saxony",
        IsActive = true
      },
      new Player {
        Id = 99,
        TrueAchievementId = 78294,
        Name = "PhillipWendell",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 100,
        TrueAchievementId = 338344,
        Name = "C64 Mat",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 101,
        TrueAchievementId = 519817,
        Name = "Benjii Redux",
        Region = "Austrailia",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 102,
        TrueAchievementId = 286468,
        Name = "KATAKL1ZM",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 103,
        TrueAchievementId = 408078,
        Name = "Mark B",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 104,
        TrueAchievementId = 82490,
        Name = "Slayer Reigning",
        Region = "United States",
        Area = "Iowa",
        IsActive = true
      },
      new Player {
        Id = 105,
        TrueAchievementId = 135630,
        Name = "xLAx JesteR",
        Region = "England",
        Area = "Derbyshire",
        IsActive = true
      },
      new Player {
        Id = 106,
        TrueAchievementId = 450068,
        Name = "CrunchyGoblin68",
        Region = "Canada",
        Area = "Ontario",
        IsActive = true
      },
      new Player {
        Id = 107,
        TrueAchievementId = 333080,
        Name = "SaucySlingo",
        Region = "United States",
        Area = "Massachusetts",
        IsActive = true
      },
      new Player {
        Id = 108,
        TrueAchievementId = 94956,
        Name = "III Torpedo III",
        Region = "Germany",
        Area = "Lower Saxony",
        IsActive = true
      },
      new Player {
        Id = 109,
        TrueAchievementId = 78762,
        Name = "Eliphelet77",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 110,
        TrueAchievementId = 434741,
        Name = "Lord Zell",
        Region = "Wales",
        Area = "Cardiff",
        IsActive = true
      },
      new Player {
        Id = 111,
        TrueAchievementId = 907614,
        Name = "Boda Yett",
        Region = "United States",
        Area = "Oregon",
        IsActive = true
      },
      new Player {
        Id = 112,
        TrueAchievementId = 800715,
        Name = "Not A Designer",
        Region = "United States",
        Area = "Tennessee",
        IsActive = true
      },
      new Player {
        Id = 113,
        TrueAchievementId = 64293,
        Name = "Muetschens",
        Region = "Switzerland",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 114,
        TrueAchievementId = 64295,
        Name = "Big Ell",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 115,
        TrueAchievementId = 2898,
        Name = "K4rn4ge",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 116,
        TrueAchievementId = 134205,
        Name = "bryan dot exe",
        Region = "United States",
        Area = "Texas",
        IsActive = true
      },
      new Player {
        Id = 117,
        TrueAchievementId = 410425,
        Name = "Kaneman",
        Region = "Slovenia",
        Area = "Slovenia",
        IsActive = true
      },
      new Player {
        Id = 118,
        TrueAchievementId = 24047,
        Name = "DudeWithTheFace",
        Region = "United States",
        Area = "Georgia",
        IsActive = true
      },
      new Player {
        Id = 119,
        TrueAchievementId = 684657,
        Name = "MadLefty2097",
        Region = "United States",
        Area = "Indiana",
        IsActive = true
      },
      new Player {
        Id = 120,
        TrueAchievementId = 447768,
        Name = "retstak",
        Region = "United States",
        Area = "California",
        IsActive = true
      },
      new Player {
        Id = 121,
        TrueAchievementId = 615774,
        Name = "Death Dealers",
        Region = "United States",
        Area = "Ohio",
        IsActive = true
      },
      new Player {
        Id = 122,
        TrueAchievementId = 5581,
        Name = "Matrarch",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 123,
        TrueAchievementId = 13608,
        Name = "omgeezus",
        Region = "United States",
        Area = "Pennsylvania",
        IsActive = true
      },
      new Player {
        Id = 124,
        TrueAchievementId = 7499,
        Name = "FlutteryChicken",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 125,
        TrueAchievementId = 651526,
        Name = "CarpeAdam79",
        Region = "United States",
        Area = "Florida",
        IsActive = true
      },
      new Player {
        Id = 126,
        TrueAchievementId = 647169,
        Name = "Hatton90",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 127,
        TrueAchievementId = 538641,
        Name = "Triple Triad777",
        Region = "United States",
        Area = "Indiana",
        IsActive = true
      },
      new Player {
        Id = 128,
        TrueAchievementId = 105056,
        Name = "Buffs",
        Region = "United States",
        Area = "Texas",
        IsActive = true
      },
      new Player {
        Id = 129,
        TrueAchievementId = 38717,
        Name = "BPBPBPBPBPBPBP",
        Region = "United States",
        Area = "Pennsylvania",
        IsActive = true
      },
      new Player {
        Id = 130,
        TrueAchievementId = 266948,
        Name = "CHERRY CHEERIOS",
        Region = "United States",
        Area = "West Virginia",
        IsActive = true
      },
      new Player {
        Id = 131,
        TrueAchievementId = 54140,
        Name = "DJB Hustlin",
        Region = "Jamaica",
        Area = "Jamaica",
        IsActive = true
      },
      new Player {
        Id = 132,
        TrueAchievementId = 328184,
        Name = "ITS ALivEx",
        Region = "Canada",
        Area = "Ontario",
        IsActive = true
      },
      new Player {
        Id = 133,
        TrueAchievementId = 893882,
        Name = "TheAlphaSeagull",
        Region = "United States",
        Area = "New York",
        IsActive = true
      },
      new Player {
        Id = 134,
        TrueAchievementId = 1995,
        Name = "mdp 73",
        Region = "Canada",
        Area = "Ontario",
        IsActive = true
      },
      new Player {
        Id = 135,
        TrueAchievementId = 533284,
        Name = "ListlessDragon",
        Region = "Switzerland",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 136,
        TrueAchievementId = 435101,
        Name = "WeezyFuzz",
        Region = "United States",
        Area = "Wisconsin",
        IsActive = true
      },
      new Player {
        Id = 137,
        TrueAchievementId = 628247,
        Name = "ILethalStang",
        Region = "United States",
        Area = "Louisiana",
        IsActive = true
      },
      new Player {
        Id = 138,
        TrueAchievementId = 392553,
        Name = "IrishWarrior022",
        Region = "Canada",
        Area = "Ontario",
        IsActive = true
      },
      new Player {
        Id = 139,
        TrueAchievementId = 6420,
        Name = "General Tynstar",
        Region = "United States",
        Area = "Arizona",
        IsActive = true
      },
      new Player {
        Id = 140,
        TrueAchievementId = 684086,
        Name = "Ace",
        Region = "Canada",
        Area = "British Columbia",
        IsActive = true
      },
      new Player {
        Id = 141,
        TrueAchievementId = 1022271,
        Name = "DirtyMcNasty126",
        Region = "United States",
        Area = "Ohio",
        IsActive = true
      },
      new Player {
        Id = 142,
        TrueAchievementId = 439921,
        Name = "Bsmittel",
        Region = "United States",
        Area = "Michigan",
        IsActive = true
      },
      new Player {
        Id = 143,
        TrueAchievementId = 387165,
        Name = "SKOOT2006",
        Region = "United States",
        Area = "Texas",
        IsActive = true
      },
      new Player {
        Id = 144,
        TrueAchievementId = 111184,
        Name = "Simpso",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 145,
        TrueAchievementId = 451792,
        Name = "BlazeFlareon",
        Region = "United States",
        Area = "Illinois",
        IsActive = true
      },
      new Player {
        Id = 146,
        TrueAchievementId = 375330,
        Name = "MrWolfw00d",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 147,
        TrueAchievementId = 50502,
        Name = "AgileDuke",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 148,
        TrueAchievementId = 349158,
        Name = "JeffMomm",
        Region = "United States",
        Area = "Wisconsin",
        IsActive = true
      },
      new Player {
        Id = 149,
        TrueAchievementId = 349605,
        Name = "MattiasAnderson",
        Region = "Sweden",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 150,
        TrueAchievementId = 711432,
        Name = "Kyleia",
        Region = "United States",
        Area = "Colorado",
        IsActive = true
      },
      new Player {
        Id = 151,
        TrueAchievementId = 896662,
        Name = "Miller N7",
        Region = "England",
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 152,
        TrueAchievementId = 380552,
        Name = "Fresh336669",
        Region = "United States",
        Area = "North Carolina",
        IsActive = true
      },
      new Player {
        Id = 153,
        TrueAchievementId = 721181,
        Name = "MOT Astro",
        Region = "England",
        Area = "Suffolk",
        IsActive = true
      },
      new Player {
        Id = 154,
        TrueAchievementId = 4936,
        Name = "Facial La Fleur",
        Region = "United States",
        Area = "California",
        IsActive = true
      },
      new Player {
        Id = 155,
        TrueAchievementId = 437038,
        Name = "AnonymousODB",
        Region = "United States",
        Area = "Ohio",
        IsActive = true
      },
      new Player {
        Id = 156,
        TrueAchievementId = 635863,
        Name = "FuFuCuddilyPoof",
        Region = "United States",
        Area = "Maryland",
        IsActive = true
      },
      new Player {
        Id = 157,
        TrueAchievementId = 408827,
        Name = "xI The Rock Ix",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 158,
        TrueAchievementId = 85256,
        Name = "tatersoup19",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 159,
        TrueAchievementId = 347191,
        Name = "nightw0lf",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 160,
        TrueAchievementId = 1725,
        Name = "N龱T廾 T廾A G龱D",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 161,
        TrueAchievementId = 702307,
        Name = "emz fergi",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 162,
        TrueAchievementId = 262143,
        Name = "Inigomontoya80",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 163,
        TrueAchievementId = 597081,
        Name = "PRTM CLUESCROL",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 164,
        TrueAchievementId = 48289,
        Name = "AZ Mongoose",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 165,
        TrueAchievementId = 76517,
        Name = "Vulgar Latin",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 166,
        TrueAchievementId = 643897,
        Name = "TobyLinn",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 167,
        TrueAchievementId = 52223,
        Name = "Jblacq",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 168,
        TrueAchievementId = 370170,
        Name = "Enigma Gamer 77",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 169,
        TrueAchievementId = 318602,
        Name = "Xpovos",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 170,
        TrueAchievementId = 409281,
        Name = "Shadow",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 171,
        TrueAchievementId = 276943,
        Name = "Ahayzo",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 172,
        TrueAchievementId = 364130,
        Name = "Darklord Davis",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 173,
        TrueAchievementId = 97393,
        Name = "logicslayer",
        Region = null,
        Area = null,
        IsActive = true
      },
      new Player {
        Id = 174,
        TrueAchievementId = 723406,
        Name = "HyRoad V2",
        Region = null,
        Area = null,
        IsActive = true
      }
    );
  }
}
