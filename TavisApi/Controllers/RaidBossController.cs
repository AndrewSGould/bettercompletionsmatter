using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Tavis.Models;
using TavisApi.Context;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RaidBossController : ControllerBase {

  private TavisContext _context;

  public RaidBossController(TavisContext context) {
    _context = context;
  }

  [HttpGet]
  [Route("hh")]
  public IActionResult Hh()
  {
    // get players
    var hhPlayers = RaidBossController.HhPlayers;
    var players = _context.Players!.Where(x => x.IsActive).ToList();
    var report = new List<HhUpdate>();

    foreach (var player in players) {
      // only get players in HH
      if (hhPlayers.Any(x => x.Player == player.Name)) {
        var update = new HhUpdate();

        // get that players games
        var playersGames = _context.PlayerGames!.Where(x => x.PlayerId == player.Id);
        var playersHH = hhPlayers.First(x => x.Player == player.Name);
        var pertinentGames = playersGames.Join(_context.Games!, pg => pg.GameId, g => g.Id, (pg, g) => new { PlayerGame = pg, Game = g})
                                            .Where(x => playersHH.Games.Any(y => y == x.Game!.Title));
        
        update.Player = player.Name!;
        update.Rival = playersHH.Rival;
        update.Games = new List<GameReport>();

        foreach (var game in pertinentGames) {
          update.Games.Add(new GameReport {
            Title = game.Game.Title,
            AchievementCount = game.PlayerGame.AchievementCount
          });
        }

        if (pertinentGames.Count() != playersHH.Games.Count()) {
          var missingGames = playersHH.Games.Where(x => !pertinentGames.Any(y => y.Game.Title.Contains(x)));
          
          foreach(var game in missingGames) {
            update.Games.Add(new GameReport {
              Title = game,
              AchievementCount = null
            });
          }
        }

        update.Games = update.Games.OrderBy(x => x.Title).ToList();
        report.Add(update);
      }
    }

    var firstSyncToday = players.Where(x => x.LastSync.GetValueOrDefault().Date != null && x.LastSync.GetValueOrDefault().Date == DateTime.Now.Date)
                                .OrderByDescending(x => x.LastSync)
                                .FirstOrDefault().LastSync;

    return Ok(new {
      Scanned = firstSyncToday,
      Report = report
    });
  }

  public class HhUpdate {
    public string Player {get; set;}
    public string Rival {get; set;}

    public List<GameReport> Games {get; set;}
  }

  public class GameReport {
    public string? Title {get; set;}
    public int? AchievementCount {get; set;}
  }

  public static readonly ReadOnlyCollection<HhPlayer> HhPlayers = 
    new ReadOnlyCollection<HhPlayer>(
      new List<HhPlayer> {
        new HhPlayer {
          Player = "lucas1987",
          Games = new string[] {"Cyberpunk 2077", "Cat Quest II", "Dungeon Punks", "The Outer Worlds", "Vikings – Wolves of Midgard"},
          Rival = "TXMOOK"
        },
        new HhPlayer {
          Player = "TXMOOK",
          Games = new string[] {"Cyberpunk 2077", "Cat Quest II", "Dungeon Punks", "The Outer Worlds", "Vikings – Wolves of Midgard"},
          Rival = "lucas1987"
        },
        new HhPlayer {
          Player = "smrnov",
          Games = new string[] {"Another Sight", "Revenant Dogma", "The Cave", "Tactics V: \"Obsidian Brigade\"", "Deus Ex: Human Revolution"},
          Rival = "eohjay"
        },
        new HhPlayer {
          Player = "eohjay",
          Games = new string[] {"Another Sight", "Revenant Dogma", "The Cave", "Tactics V: \"Obsidian Brigade\"", "Deus Ex: Human Revolution"},
          Rival = "smrnov"
        },
        new HhPlayer {
          Player = "Infamous",
          Games = new string[] {"Portal Knights", "Geometry Wars: Retro Evolved", "Extinction", "Dunk Lords", "Hexic HD"},
          Rival = "Fine Feat"
        },
        new HhPlayer {
          Player = "Fine Feat",
          Games = new string[] {"Portal Knights", "Geometry Wars: Retro Evolved", "Extinction", "Dunk Lords", "Hexic HD"},
          Rival = "Infamous"
        },
        new HhPlayer {
          Player = "Sir Paulygon",
          Games = new string[] {"Aerial_Knight's Never Yield", "Lake", "Olija", "Teenage Mutant Ninja Turtles: Shredder's Revenge", "Young Souls"},
          Rival = "Luke17000"
        },
        new HhPlayer {
          Player = "Luke17000",
          Games = new string[] {"Aerial_Knight's Never Yield", "Lake", "Olija", "Teenage Mutant Ninja Turtles: Shredder's Revenge", "Young Souls"},
          Rival = "Sir Paulygon"
        },
        new HhPlayer {
          Player = "SwiftSupafly",
          Games = new string[] {"Left 4 Dead 2", "Project Warlock", "BUTCHER", "Hexic HD", "ZOMBI"},
          Rival = "Wakapeil"
        },
        new HhPlayer {
          Player = "Wakapeil",
          Games = new string[] {"Left 4 Dead 2", "Project Warlock", "BUTCHER", "Hexic HD", "ZOMBI"},
          Rival = "SwiftSupafly"
        },
        new HhPlayer {
          Player = "True Veteran",
          Games = new string[] {"Anthem", "Bright Memory: Infinite", "Chernobylite", "Dead Rising", "Star Wars Jedi: Fallen Order"},
          Rival = "Inferno118"
        },
        new HhPlayer {
          Player = "Inferno118",
          Games = new string[] {"Anthem", "Bright Memory: Infinite", "Chernobylite", "Dead Rising", "Star Wars Jedi: Fallen Order"},
          Rival = "True Veteran"
        },
        new HhPlayer {
          Player = "darkwing1232",
          Games = new string[] {"Lollipop Chainsaw", "The Walking Dead: Survival Instinct", "DOOM Eternal", "The Council", "Castlevania: Lords of Shadow"},
          Rival = "WildWhiteNoise"
        },
        new HhPlayer {
          Player = "WildWhiteNoise",
          Games = new string[] {"Lollipop Chainsaw", "The Walking Dead: Survival Instinct", "DOOM Eternal", "The Council", "Castlevania: Lords of Shadow"},
          Rival = "darkwing1232"
        },
        new HhPlayer {
          Player = "Erutaerc",
          Games = new string[] {"Vandal Hearts: Flames of Judgment", "Rise Eterna", "Nexomon", "Neoverse", "The Witcher 3: Wild Hunt - Game of the Year Edition"},
          Rival = "IcyThrasher"
        },
        new HhPlayer {
          Player = "IcyThrasher",
          Games = new string[] {"Vandal Hearts: Flames of Judgment", "Rise Eterna", "Nexomon", "Neoverse", "The Witcher 3: Wild Hunt - Game of the Year Edition"},
          Rival = "Erutaerc"
        },
        new HhPlayer {
          Player = "Christoph 5782",
          Games = new string[] {"Peggle", "Super Meat Boy", "Super Mega Baseball: Extra Innings", "The Walking Dead (Xbox 360)", "Grand Theft Auto: San Andreas – The Definitive Edition"},
          Rival = "radnonnahs"
        },
        new HhPlayer {
          Player = "radnonnahs",
          Games = new string[] {"Peggle", "Super Meat Boy", "Super Mega Baseball: Extra Innings", "The Walking Dead (Xbox 360)", "Grand Theft Auto: San Andreas – The Definitive Edition"},
          Rival = "Christoph 5782"
        },
        new HhPlayer {
          Player = "Mtld",
          Games = new string[] {"Aliens: Colonial Marines", "Brink", "Flipping Death", "Mafia: Definitive Edition", "Vanquish (Xbox 360)"},
          Rival = "HegemonicHater"
        },
        new HhPlayer {
          Player = "HegemonicHater",
          Games = new string[] {"Aliens: Colonial Marines", "Brink", "Flipping Death", "Mafia: Definitive Edition", "Vanquish (Xbox 360)"},
          Rival = "Mtld"
        },
        new HhPlayer {
          Player = "Mental Knight 5",
          Games = new string[] {"Blizzard Arcade Collection", "Costume Quest 2", "Horse Racing 2016", "Panzer Dragoon: Remake", "Psychonauts 2"},
          Rival = "NBA Kirkland"
        },
        new HhPlayer {
          Player = "NBA Kirkland",
          Games = new string[] {"Blizzard Arcade Collection", "Costume Quest 2", "Horse Racing 2016", "Panzer Dragoon: Remake", "Psychonauts 2"},
          Rival = "Mental Knight 5"
        },
        new HhPlayer {
          Player = "Whisperin Clown",
          Games = new string[] {"Immortals Fenyx Rising", "WRC 8", "Castle Crashers Remastered", "Assassin's Creed IV: Black Flag", "Bulletstorm: Full Clip Edition"},
          Rival = "Lw N1GHTM4R3"
        },
        new HhPlayer {
          Player = "Lw N1GHTM4R3",
          Games = new string[] {"Immortals Fenyx Rising", "WRC 8", "Castle Crashers Remastered", "Assassin's Creed IV: Black Flag", "Bulletstorm: Full Clip Edition"},
          Rival = "Whisperin Clown"
        },
        new HhPlayer {
          Player = "Legohead 1977",
          Games = new string[] {"Fall Guys", "Assassin's Creed Chronicles: China", "Shred Nebula", "Kid Tripp", "Tomb Raider: Underworld"},
          Rival = "Oriole2682"
        },
        new HhPlayer {
          Player = "Oriole2682",
          Games = new string[] {"Fall Guys", "Assassin's Creed Chronicles: China", "Shred Nebula", "Kid Tripp", "Tomb Raider: Underworld"},
          Rival = "Legohead 1977"
        },
        new HhPlayer {
          Player = "IronFistOfSnuff",
          Games = new string[] {"Chasm", "Bugsnax", "Exception", "Neon City Riders", "The Gunk"},
          Rival = "A1exRD"
        },
        new HhPlayer {
          Player = "A1exRD",
          Games = new string[] {"Chasm", "Bugsnax", "Exception", "Neon City Riders", "The Gunk"},
          Rival = "IronFistOfSnuff"
        },
        new HhPlayer {
          Player = "Fista Roboto",
          Games = new string[] {"Death's Door", "Immortality", "Marvel's Guardians of the Galaxy", "Outer Wilds", "Trek to Yomi"},
          Rival = "Team Brether"
        },
        new HhPlayer {
          Player = "Team Brether",
          Games = new string[] {"Death's Door", "Immortality", "Marvel's Guardians of the Galaxy", "Outer Wilds", "Trek to Yomi"},
          Rival = "Fista Roboto"
        },
        new HhPlayer {
          Player = "MajinFro",
          Games = new string[] {"Dead Rising 2 (Xbox 360)", "Peggle", "Exile's End", "Song of the Deep", "Strider (Xbox 360)"},
          Rival = "BenL72"
        },
        new HhPlayer {
          Player = "BenL72",
          Games = new string[] {"Dead Rising 2 (Xbox 360)", "Peggle", "Exile's End", "Song of the Deep", "Strider (Xbox 360)"},
          Rival = "MajinFro"
        },
        new HhPlayer {
          Player = "HawkeyeBarry20",
          Games = new string[] {"Psychonauts 2", "Undertale", "Ori and the Will of the Wisps", "Fable II", "Castlevania: Harmony of Despair"},
          Rival = "Freamwhole"
        },
        new HhPlayer {
          Player = "Freamwhole",
          Games = new string[] {"Psychonauts 2", "Undertale", "Ori and the Will of the Wisps", "Fable II", "Castlevania: Harmony of Despair"},
          Rival = "HawkeyeBarry20"
        },
        new HhPlayer {
          Player = "Proulx",
          Games = new string[] {"BattleBlock Theater", "BioShock Infinite", "Lost Odyssey", "Spec Ops: The Line", "Völgarr the Viking"},
          Rival = "Yinga Garten"
        },
        new HhPlayer {
          Player = "Yinga Garten",
          Games = new string[] {"BattleBlock Theater", "BioShock Infinite", "Lost Odyssey", "Spec Ops: The Line", "Völgarr the Viking"},
          Rival = "Proulx"
        },
        new HhPlayer {
          Player = "Hotdogmcgee",
          Games = new string[] {"Bulletstorm: Full Clip Edition", "Max: The Curse of Brotherhood", "Four Sided Fantasy", "Conga Master", "Assassin's Creed Chronicles: Russia"},
          Rival = "Majinbro"
        },
        new HhPlayer {
          Player = "Majinbro",
          Games = new string[] {"Bulletstorm: Full Clip Edition", "Max: The Curse of Brotherhood", "Four Sided Fantasy", "Conga Master", "Assassin's Creed Chronicles: Russia"},
          Rival = "Hotdogmcgee"
        },
        new HhPlayer {
          Player = "Mattism",
          Games = new string[] {"Serious Sam 4 (Windows)", "Dying Light", "Iron Crypticle", "No Time To Explain", "Defense Grid: The Awakening"},
          Rival = "FreakyRO"
        },
        new HhPlayer {
          Player = "FreakyRO",
          Games = new string[] {"Serious Sam 4 (Windows)", "Dying Light", "Iron Crypticle", "No Time To Explain", "Defense Grid: The Awakening"},
          Rival = "Mattism"
        },
        new HhPlayer {
          Player = "RetroChief1969",
          Games = new string[] {"Haimrik", "DC League of Super-Pets: The Adventures of Krypto and Ace", "Journey of the Broken Circle", "Leisure Suit Larry - Wet Dreams Don't Dry", "The Council"},
          Rival = "boldyno1"
        },
        new HhPlayer {
          Player = "boldyno1",
          Games = new string[] {"Haimrik", "DC League of Super-Pets: The Adventures of Krypto and Ace", "Journey of the Broken Circle", "Leisure Suit Larry - Wet Dreams Don't Dry", "The Council"},
          Rival = "RetroChief1969"
        },
        new HhPlayer {
          Player = "ChewieOnIce",
          Games = new string[] {"Valley", "SOMA", "SteamWorld Dig", "The Last Campfire", "The Forgotten City"},
          Rival = "Icefiretn"
        },
        new HhPlayer {
          Player = "Icefiretn",
          Games = new string[] {"Valley", "SOMA", "SteamWorld Dig", "The Last Campfire", "The Forgotten City"},
          Rival = "ChewieOnIce"
        },
        new HhPlayer {
          Player = "CrunchyGoblin68",
          Games = new string[] {"Sunset Overdrive", "Assassin's Creed Chronicles: Russia", "Pikuniku", "Outlast", "Dead Space 2"},
          Rival = "xLAx Jester"
        },
        new HhPlayer {
          Player = "xLAx Jester",
          Games = new string[] {"Sunset Overdrive", "Assassin's Creed Chronicles: Russia", "Pikuniku", "Outlast", "Dead Space 2"},
          Rival = "CrunchyGoblin68"
        },
        new HhPlayer {
          Player = "hotcurls3088",
          Games = new string[] {"LEGO Marvel Super Heroes 2", "Outriders", "Maneater", "JUJU", "Fallout: New Vegas"},
          Rival = "MadLefty2097"
        },
        new HhPlayer {
          Player = "MadLefty2097",
          Games = new string[] {"LEGO Marvel Super Heroes 2", "Outriders", "Maneater", "JUJU", "Fallout: New Vegas"},
          Rival = "hotcurls3088"
        },
        new HhPlayer {
          Player = "FlutteryChicken",
          Games = new string[] {"Joy Ride Turbo", "BioShock", "Carmageddon: Max Damage", "SSX", "Marvel's Guardians of the Galaxy"},
          Rival = "Hatton90"
        },
        new HhPlayer {
          Player = "Hatton90",
          Games = new string[] {"Joy Ride Turbo", "BioShock", "Carmageddon: Max Damage", "SSX", "Marvel's Guardians of the Galaxy"},
          Rival = "FlutteryChicken"
        },
        new HhPlayer {
          Player = "WildwoodMike",
          Games = new string[] {"Alan Wake", "Perfect Dark (Xbox 360)", "Ryse: Son of Rome", "D4: Dark Dreams Don't Die", "No Time To Explain"},
          Rival = "Matrarch"
        },
        new HhPlayer {
          Player = "Matrarch",
          Games = new string[] {"Alan Wake", "Perfect Dark (Xbox 360)", "Ryse: Son of Rome", "D4: Dark Dreams Don't Die", "No Time To Explain"},
          Rival = "WildwoodMike"
        },
        new HhPlayer {
          Player = "Mark B",
          Games = new string[] {"Pupperazzi", "Sunset Overdrive", "Puyo Puyo Champions", "Burnout Paradise", "BattleBlock Theater"},
          Rival = "Big Ell"
        },
        new HhPlayer {
          Player = "Big Ell",
          Games = new string[] {"Pupperazzi", "Sunset Overdrive", "Puyo Puyo Champions", "Burnout Paradise", "BattleBlock Theater"},
          Rival = "Mark B"
        },
        new HhPlayer {
          Player = "Whtthfgg",
          Games = new string[] {"Guacamelee! Super Turbo Championship Edition", "Far Cry 5", "Crackdown 3: Campaign", "The Outer Worlds", "Assassin's Creed Chronicles: China"},
          Rival = "Simpso"
        },
        new HhPlayer {
          Player = "Simpso",
          Games = new string[] {"Guacamelee! Super Turbo Championship Edition", "Far Cry 5", "Crackdown 3: Campaign", "The Outer Worlds", "Assassin's Creed Chronicles: China"},
          Rival = "Whtthfgg"
        },
      });

  public class HhPlayer {
    public string Player {get; set;}
    public string[] Games {get; set;}
    public string Rival {get; set;}
  }
}
