using Bcm.Models;
using Microsoft.EntityFrameworkCore;  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
  
namespace BcmApi.Context  
{ 
  public class BcmContext : DbContext  
  {  
    protected readonly IConfiguration Configuration;

    public BcmContext(DbContextOptions<BcmContext> options, IConfiguration configuration) : base(options)  
    {   
      Configuration = configuration;
    }  

    public DbSet<Game> Games { get; set; }
    public DbSet<FeatureList> FeatureLists { get; set; }
    public DbSet<Player> Players {get;set;}
    public DbSet<PlayerGame> PlayerGames {get;set;}
    public DbSet<Genre> Genres {get;set;}
    public DbSet<GameGenre> GameGenres {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var connectionString = configuration.GetConnectionString("WebApiDatabase");
      optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      BindSmartEnums(modelBuilder);
      BindRelationships(modelBuilder);
      SeedData(modelBuilder);
    }

    protected void BindSmartEnums(ModelBuilder modelBuilder) {
      modelBuilder.Entity<PlayerGame>()
        .Property(p => p.Ownership)
        .HasConversion(
            p => p.Value,
            p => Ownership.FromValue(p));

      modelBuilder.Entity<PlayerGame>()
        .Property(p => p.Platform)
        .HasConversion(
            p => p.Value,
            p => Platform.FromValue(p));

      modelBuilder.Entity<GameGenre>()
        .Property(p => p.GenreId)
        .HasConversion(
            p => p.Value,
            p => GenreList.FromValue(p));

      modelBuilder.Entity<Genre>()
        .Property(p => p.Id)
        .HasConversion(
            p => p.Value,
            p => GenreList.FromValue(p));
    }

    protected void BindRelationships(ModelBuilder modelBuilder) {
      // keys
      modelBuilder.Entity<PlayerGame>().HasKey(c => new {c.GameId, c.PlayerId});


      // 1-to-1
      modelBuilder.Entity<Game>()
                    .HasOne<FeatureList>(x => x.FeatureList)
                    .WithOne(x => x.Game)
                    .HasForeignKey<FeatureList>(x => x.FeatureListOfGameId);

      // 1-to-many
      modelBuilder.Entity<PlayerGame>()
                    .HasOne<Player>(x => x.Player)
                    .WithMany(x => x.PlayerGames)
                    .HasForeignKey(x => x.PlayerId);

      modelBuilder.Entity<PlayerGame>()
                    .HasOne<Game>(x => x.Game)
                    .WithMany(x => x.PlayerGames)
                    .HasForeignKey(x => x.GameId);

      // many-to-many
      modelBuilder.Entity<GameGenre>().HasKey(x => new {x.GameId, x.GenreId});
    }

    protected void SeedData(ModelBuilder modelBuilder) {

      #region GenreData
      modelBuilder.Entity<Genre>().HasData(
        new Genre {
          Id = GenreList.Action,
          Name = GenreList.Action.Name
        },
        new Genre {
          Id = GenreList.Sports,
          Name = GenreList.Sports.Name
        },
        new Genre {
          Id = GenreList.Football,
          Name = GenreList.Football.Name
        },
        new Genre {
          Id = GenreList.TPS,
          Name = GenreList.TPS.Name
        },
        new Genre {
          Id = GenreList.ActionHorror,
          Name = GenreList.ActionHorror.Name
        },
        new Genre {
          Id = GenreList.ActionAdventure,
          Name = GenreList.ActionAdventure.Name
        },
        new Genre {
          Id = GenreList.ARPG,
          Name = GenreList.ARPG.Name
        },
        new Genre {
          Id = GenreList.RP,
          Name = GenreList.RP.Name
        },
        new Genre {
          Id = GenreList.HackAndSlash,
          Name = GenreList.HackAndSlash.Name
        },
        new Genre {
          Id = GenreList.Aerial,
          Name = GenreList.Aerial.Name
        },
        new Genre {
          Id = GenreList.VehicularCombat,
          Name = GenreList.VehicularCombat.Name
        },
        new Genre {
          Id = GenreList.AmericanFootball,
          Name = GenreList.AmericanFootball.Name
        },
        new Genre {
          Id = GenreList.ArcadeRacing,
          Name = GenreList.ArcadeRacing.Name
        },
        new Genre {
          Id = GenreList.Automobile,
          Name = GenreList.Automobile.Name
        },
        new Genre {
          Id = GenreList.AustralianFootball,
          Name = GenreList.AustralianFootball.Name
        },
        new Genre {
          Id = GenreList.Baseball,
          Name = GenreList.Baseball.Name
        },
        new Genre {
          Id = GenreList.Basketball,
          Name = GenreList.Basketball.Name
        },
        new Genre {
          Id = GenreList.FPS,
          Name = GenreList.FPS.Name
        },
        new Genre {
          Id = GenreList.BR,
          Name = GenreList.BR.Name
        },
        new Genre {
          Id = GenreList.BeatEmUp,
          Name = GenreList.BeatEmUp.Name
        },
        new Genre {
          Id = GenreList.Bowling,
          Name = GenreList.Bowling.Name
        },
        new Genre {
          Id = GenreList.Boxing,
          Name = GenreList.Boxing.Name
        },
        new Genre {
          Id = GenreList.Bull,
          Name = GenreList.Bull.Name
        },
        new Genre {
          Id = GenreList.CardAndBoard,
          Name = GenreList.CardAndBoard.Name
        },
        new Genre {
          Id = GenreList.Casino,
          Name = GenreList.Casino.Name
        },
        new Genre {
          Id = GenreList.CCG,
          Name = GenreList.CCG.Name
        },
        new Genre {
          Id = GenreList.Collection,
          Name = GenreList.Collection.Name
        },
        new Genre {
          Id = GenreList.Adventure,
          Name = GenreList.Adventure.Name
        },
        new Genre {
          Id = GenreList.PointAndClick,
          Name = GenreList.PointAndClick.Name
        },
        new Genre {
          Id = GenreList.Cricket,
          Name = GenreList.Cricket.Name
        },
        new Genre {
          Id = GenreList.Cue,
          Name = GenreList.Cue.Name
        },
        new Genre {
          Id = GenreList.Platformer,
          Name = GenreList.Platformer.Name
        },
        new Genre {
          Id = GenreList.Cycling,
          Name = GenreList.Cycling.Name
        },
        new Genre {
          Id = GenreList.Dance,
          Name = GenreList.Dance.Name
        },
        new Genre {
          Id = GenreList.Darts,
          Name = GenreList.Darts.Name
        },
        new Genre {
          Id = GenreList.Dodgeball,
          Name = GenreList.Dodgeball.Name
        },
        new Genre {
          Id = GenreList.OpenWorld,
          Name = GenreList.OpenWorld.Name
        },
        new Genre {
          Id = GenreList.DungeonCrawler,
          Name = GenreList.DungeonCrawler.Name
        },
        new Genre {
          Id = GenreList.EducationalTrivia,
          Name = GenreList.EducationalTrivia.Name
        },
        new Genre {
          Id = GenreList.Party,
          Name = GenreList.Party.Name
        },
        new Genre {
          Id = GenreList.Equestrian,
          Name = GenreList.Equestrian.Name
        },
        new Genre {
          Id = GenreList.Fighting,
          Name = GenreList.Fighting.Name
        },
        new Genre {
          Id = GenreList.Fishing,
          Name = GenreList.Fishing.Name
        },
        new Genre {
          Id = GenreList.Golf,
          Name = GenreList.Golf.Name
        },
        new Genre {
          Id = GenreList.Handball,
          Name = GenreList.Handball.Name
        },
        new Genre {
          Id = GenreList.Simulation,
          Name = GenreList.Simulation.Name
        },
        new Genre {
          Id = GenreList.Health,
          Name = GenreList.Health.Name
        },
        new Genre {
          Id = GenreList.Hockey,
          Name = GenreList.Hockey.Name
        },
        new Genre {
          Id = GenreList.Hunting,
          Name = GenreList.Hunting.Name
        },
        new Genre {
          Id = GenreList.Management,
          Name = GenreList.Management.Name
        },
        new Genre {
          Id = GenreList.Mech,
          Name = GenreList.Mech.Name
        },
        new Genre {
          Id = GenreList.Metroidvania,
          Name = GenreList.Metroidvania.Name
        },
        new Genre {
          Id = GenreList.MMA,
          Name = GenreList.MMA.Name
        },
        new Genre {
          Id = GenreList.MMO,
          Name = GenreList.MMO.Name
        },
        new Genre {
          Id = GenreList.MOBA,
          Name = GenreList.MOBA.Name
        },
        new Genre {
          Id = GenreList.Motocross,
          Name = GenreList.Motocross.Name
        },
        new Genre {
          Id = GenreList.OnRails,
          Name = GenreList.OnRails.Name
        },
        new Genre {
          Id = GenreList.Music,
          Name = GenreList.Music.Name
        },
        new Genre {
          Id = GenreList.Naval,
          Name = GenreList.Naval.Name
        },
        new Genre {
          Id = GenreList.Survival,
          Name = GenreList.Survival.Name
        },
        new Genre {
          Id = GenreList.Paintball,
          Name = GenreList.Paintball.Name
        },
        new Genre {
          Id = GenreList.Pinball,
          Name = GenreList.Pinball.Name
        },
        new Genre {
          Id = GenreList.Puzzle,
          Name = GenreList.Puzzle.Name
        },
        new Genre {
          Id = GenreList.Strategy,
          Name = GenreList.Strategy.Name
        },
        new Genre {
          Id = GenreList.RealTime,
          Name = GenreList.RealTime.Name
        },
        new Genre {
          Id = GenreList.Roguelite,
          Name = GenreList.Roguelite.Name
        },
        new Genre {
          Id = GenreList.Rugby,
          Name = GenreList.Rugby.Name
        },
        new Genre {
          Id = GenreList.RunAndGun,
          Name = GenreList.RunAndGun.Name
        },
        new Genre {
          Id = GenreList.Sandbox,
          Name = GenreList.Sandbox.Name
        },
        new Genre {
          Id = GenreList.Shmup,
          Name = GenreList.Shmup.Name
        },
        new Genre {
          Id = GenreList.SimRacing,
          Name = GenreList.SimRacing.Name
        },
        new Genre {
          Id = GenreList.Skateboarding,
          Name = GenreList.Skateboarding.Name
        },
        new Genre {
          Id = GenreList.Skiing,
          Name = GenreList.Skiing.Name
        },
        new Genre {
          Id = GenreList.Snowboarding,
          Name = GenreList.Snowboarding.Name
        },
        new Genre {
          Id = GenreList.Stealth,
          Name = GenreList.Stealth.Name
        },
        new Genre {
          Id = GenreList.SurvivalHorror,
          Name = GenreList.SurvivalHorror.Name
        },
        new Genre {
          Id = GenreList.Tennis,
          Name = GenreList.Tennis.Name
        },
        new Genre {
          Id = GenreList.TowerDefence,
          Name = GenreList.TowerDefence.Name
        },
        new Genre {
          Id = GenreList.VN,
          Name = GenreList.VN.Name
        },
        new Genre {
          Id = GenreList.Volleyball,
          Name = GenreList.Volleyball.Name
        },
        new Genre {
          Id = GenreList.Wrestling,
          Name = GenreList.Wrestling.Name
        },
        new Genre {
          Id = GenreList.TurnBased,
          Name = GenreList.TurnBased.Name
        }
      );
      #endregion GenreData
      
      #region PlayerData
      modelBuilder.Entity<Player>().HasData(
        new Player {
          Id = 1,
          TrueAchievementId = 104571,
          Name = "kT Echo",
          Region = "United States",
          Area = "Ohio"
        },
        new Player {
          Id = 2,
          TrueAchievementId = 266752,
          Name = "eohjay",
          Region = "United States",
          Area = "Ohio"
        },
        new Player {
          Id = 3,
          TrueAchievementId = 405202,
          Name = "IronFistofSnuff",
          Region = "United States",
          Area = "Ohio"
        },
        new Player {
          Id = 4,
          TrueAchievementId = 461682,
          Name = "zzScanMan1",
          Region = null,
          Area = null
        }
      );
      #endregion PlayerData
    }
  }
}
