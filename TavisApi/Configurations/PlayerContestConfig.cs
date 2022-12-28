using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class PlayerContestConfiguration : IEntityTypeConfiguration<PlayerContest>
{
  public void Configure(EntityTypeBuilder<PlayerContest> builder)
  {
    builder
      .HasKey(c => new {c.ContestId, c.PlayerId});

    builder
      .HasOne<Player>(x => x.Player)
      .WithMany(x => x.PlayerContests)
      .HasForeignKey(x => x.PlayerId);

    builder
      .HasOne<Contest>(x => x.Contest)
      .WithMany(x => x.PlayerContests)
      .HasForeignKey(x => x.ContestId);


    builder.HasData(
#region BCM Players
      new PlayerContest {
        PlayerId = 60,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 50,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 140,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 177,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 164,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 24,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 101,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 56,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 114,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 145,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 76,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 129,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 178,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 15,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 83,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 26,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 74,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 106,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 179,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 16,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 180,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 85,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 121,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 181,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 65,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 109,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 161,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 2,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 29,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 154,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 182,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 10,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 41,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 124,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 77,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 57,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 73,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 126,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 54,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 42,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 88,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 78,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 62,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 22,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 7,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 19,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 183,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 3,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 98,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 184,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 17,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 79,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 55,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 68,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 89,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 185,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 1,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 186,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 63,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 9,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 8,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 119,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 187,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 122,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 149,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 44,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 188,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 189,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 190,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 191,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 72,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 27,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 39,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 94,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 192,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 58,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 193,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 86,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 120,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 194,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 33,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 195,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 93,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 144,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 12,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 104,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 196,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 197,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 5,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 21,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 198,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 127,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 23,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 70,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 36,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 136,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 199,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 47,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 91,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 30,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 82,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 18,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 95,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 80,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 99,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 75,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 25,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 71,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 31,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 115,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 113,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 108,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 69,
        ContestId = 1
      }
#endregion
    );
  }
}
