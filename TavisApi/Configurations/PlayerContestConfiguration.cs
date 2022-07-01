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
        PlayerId = 6,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 7,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 8,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 9,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 10,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 11,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 12,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 13,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 14,  
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 15,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 16,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 17,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 18,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 19,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 20,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 1,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 2,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 3,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 4,
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
        PlayerId = 22,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 23,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 24,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 25,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 26,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 27,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 28,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 29,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 30,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 31,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 32,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 33,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 34,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 35,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 36,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 37,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 38,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 39,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 40,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 41,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 42,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 43,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 44,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 45,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 46,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 47,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 48,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 49,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 50,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 51,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 52,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 53,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 54,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 55,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 56,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 57,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 58,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 59,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 60,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 61,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 62,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 63,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 64,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 65,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 66,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 67,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 68,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 69,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 70,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 71,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 72,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 73,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 74,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 75,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 76,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 77,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 78,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 79,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 80,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 81,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 82,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 83,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 84,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 85,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 86,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 87,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 88,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 89,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 90,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 91,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 92,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 93,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 94,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 95,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 96,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 97,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 98,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 99,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 100,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 101,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 102,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 103,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 104,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 105,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 106,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 107,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 108,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 109,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 110,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 111,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 112,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 113,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 114,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 115,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 116,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 117,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 118,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 119,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 120,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 121,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 122,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 123,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 124,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 125,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 126,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 127,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 128,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 129,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 130,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 131,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 132,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 133,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 134,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 135,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 136,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 137,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 138,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 139,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 140,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 141,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 142,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 143,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 144,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 145,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 146,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 147,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 148,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 149,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 150,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 151,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 152,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 153,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 154,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 155,
        ContestId = 1
      },
      new PlayerContest {
        PlayerId = 156,
        ContestId = 1
      },
#endregion
#region Raid Boss Players
      new PlayerContest {
        PlayerId = 50,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 140,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 171,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 164,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 56,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 114,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 83,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 49,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 106,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 172,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 161,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 168,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 2,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 29,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 77,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 54,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 174,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 62,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 22,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 7,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 162,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 3,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 132,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 167,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 17,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 89,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 1,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 32,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 173,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 8,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 119,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 122,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 134,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 48,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 39,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 159,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 94,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 160,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 163,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 66,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 64,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 170,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 144,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 12,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 81,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 11,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 166,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 165,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 36,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 136,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 91,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 157,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 95,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 169,
        ContestId = 2
      },
      new PlayerContest {
        PlayerId = 59,
        ContestId = 2
      }
#endregion
    );
  }
}
