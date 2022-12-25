using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
  public void Configure(EntityTypeBuilder<Genre> builder)
  {
    builder
        .Property(p => p.Id)
        .HasConversion(
            p => p!.Value,
            p => GenreList.FromValue(p));

    builder.HasData
    (
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
      },
      new Genre {
        Id = GenreList.Swimming,
        Name = GenreList.Swimming.Name
      },
      new Genre {
        Id = GenreList.Surfing,
        Name = GenreList.Surfing.Name
      },
      new Genre {
        Id = GenreList.Badminton,
        Name = GenreList.Badminton.Name
      },
      new Genre {
        Id = GenreList.TableTennis,
        Name = GenreList.TableTennis.Name
      },
      new Genre {
        Id = GenreList.Skating,
        Name = GenreList.Skating.Name
      },
      new Genre {
        Id = GenreList.Lacrosse,
        Name = GenreList.Lacrosse.Name
      },
      new Genre {
        Id = GenreList.Skydiving,
        Name = GenreList.Skydiving.Name
      }
    );
  }
}
