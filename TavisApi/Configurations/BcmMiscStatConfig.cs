using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Tavis.Models;
using TavisApi.Models;

namespace TavisApi.Context;

public class BcmMiscStatConfiguration : IEntityTypeConfiguration<BcmMiscStat>
{
  public void Configure(EntityTypeBuilder<BcmMiscStat> builder)
  {
    builder
      .HasKey(c => c.Id);

    builder
      .Property(p => p.HistoricalStats)
      .HasColumnType("jsonb");

    builder
      .HasOne(x => x.BcmPlayer)
      .WithOne(x => x.BcmMiscStats)
      .HasForeignKey<BcmMiscStat>(x => x.PlayerId);

    #region
    var kTEchoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 8,
        FullCombo = false,
        Placement = 31
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 71
      }
    };

    var aeoMonkeyHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 12,
        FullCombo = true,
        Placement = 7
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 40
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 3,
        FullCombo = false,
        Placement = 93
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 73
      },
    };

    var acaeulsTHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 70
      }
    };

    var aceHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 119
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 82
      }
    };

    var ahayzoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 4,
        FullCombo = false,
        Placement = 31
      }
    };

    var albinoKidEliteHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 22
      }
    };

    var benL72HistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 12,
        FullCombo = false,
        Placement = 9
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 11,
        FullCombo = false,
        Placement = 41
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 34
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 26
      }
    };

    var bigEllHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 12,
        FullCombo = false,
        Placement = 73
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 69
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 2,
        FullCombo = false,
        Placement = 57
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 2,
        FullCombo = false,
        Placement = 74
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 1,
        FullCombo = false,
        Placement = 104
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 90
      },
    };

    var blazeFlareonHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 11,
        FullCombo = false,
        Placement = 28
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 3,
        FullCombo = false,
        Placement = 95
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 147
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 139
      }
    };

    var carpeAdamHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 67
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 132
      }
    };

    var chewieHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 11,
        FullCombo = false,
        Placement = 21
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 56
      }
    };

    var cheznoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 117
      }
    };

    var christophHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 21
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 26
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 3,
        FullCombo = false,
        Placement = 3
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 8
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 5,
        FullCombo = false,
        Placement = 17
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 102
      },
    };

    var couchBurgularHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 101
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 6,
        FullCombo = false,
        Placement = 85
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 119
      },
    };

    var cptCookieHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 4
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 33
      }
    };

    var crunchyGoblinHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 55
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 7,
        FullCombo = false,
        Placement = 50
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 10,
        FullCombo = false,
        Placement = 54
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 86
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 11,
        FullCombo = false,
        Placement = 78
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 11,
        FullCombo = false,
        Placement = 103
      },
    };

    var danTheWhaleHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 6
      }
    };

    var darkwingHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 4,
        FullCombo = false,
        Placement = 40
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 5,
        FullCombo = false,
        Placement = 28
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 3,
        FullCombo = false,
        Placement = 25
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 64
      }
    };

    var daveBodomHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 32
      }
    };

    var davidMcCHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 114
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 126
      }
    };

    var dubdeeHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 11,
        FullCombo = false,
        Placement = 11
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 46
      }
    };

    var dubstepHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 3,
        FullCombo = false,
        Placement = 2
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 1,
        FullCombo = false,
        Placement = 4
      }
    };

    var dudeWithTheFaceHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 121
      }
    };

    var elipheletHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 3,
        FullCombo = false,
        Placement = 57
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 1,
        FullCombo = false,
        Placement = 49
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 4,
        FullCombo = false,
        Placement = 82
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 94
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 11,
        FullCombo = false,
        Placement = 40
      }
    };

    var emzFergiHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 42
      }
    };

    var eohjayHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 7,
        FullCombo = false,
        Placement = 12
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = true,
        Placement = 5
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = true,
        Placement = 8
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 9,
        FullCombo = false,
        Placement = 17
      }
    };

    var erutaercHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = true,
        Placement = 7
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 16
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 47
      }
    };

    var fineFeatHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 1
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 3
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = true,
        Placement = 2
      }
    };

    var fistaRobotoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 3,
        FullCombo = false,
        Placement = 52
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 38
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 20
      }
    };

    var flutteryChickenHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 107
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 92
      }
    };

    var freakyHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 4,
        FullCombo = false,
        Placement = 75
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 101
      }
    };

    var freamHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 34
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 11,
        FullCombo = false,
        Placement = 27
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 59
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 35
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 8,
        FullCombo = false,
        Placement = 58
      }
    };

    var hattonHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 1,
        FullCombo = false,
        Placement = 83
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 6,
        FullCombo = false,
        Placement = 97
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 5,
        FullCombo = false,
        Placement = 128
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 130
      }
    };

    var hawkeyeBarryHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 9,
        FullCombo = false,
        Placement = 30
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 6,
        FullCombo = false,
        Placement = 65
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 46
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 76
      }
    };

    var hegemonicHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 15
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 5
      }
    };

    var henkeHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 19
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 12,
        FullCombo = true,
        Placement = 36
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 12,
        FullCombo = false,
        Placement = 36
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 64
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 61
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 51
      }
    };

    var hotcurlsHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 72
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 12,
        FullCombo = true,
        Placement = 43
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 5,
        FullCombo = false,
        Placement = 35
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 7,
        FullCombo = false,
        Placement = 75
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 5,
        FullCombo = false,
        Placement = 96
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 112
      }
    };

    var icyThrasherHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 19
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 38
      }
    };

    var infamousHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 7
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 6
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 3,
        FullCombo = false,
        Placement = 3
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 5
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 3,
        FullCombo = false,
        Placement = 15
      }
    };

    var infernoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 83
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 18
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 29
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 50
      }
    };

    var irishWarriorHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 81
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 142
      },
    };

    var ironHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 24
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 4,
        FullCombo = false,
        Placement = 12
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 6,
        FullCombo = false,
        Placement = 16
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 7,
        FullCombo = false,
        Placement = 50
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 23
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 3,
        FullCombo = false,
        Placement = 57
      },
    };

    var jBattlestarHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 26
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 52
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 69
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 69
      },
    };

    var jimbotHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 10
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 2
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 39
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 43
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 84
      }
    };

    var johnnyDeliciousHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 135
      }
    };

    var kaitehHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 43
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 55
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 80
      }
    };

    var kawiNinjaHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 62
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 69
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 81
      }
    };

    var kezHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 62
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 4,
        FullCombo = false,
        Placement = 35
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 6,
        FullCombo = false,
        Placement = 60
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 2,
        FullCombo = false,
        Placement = 93
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 5,
        FullCombo = false,
        Placement = 92
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 4,
        FullCombo = false,
        Placement = 97
      }
    };

    var kingsOfDispairHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 10,
        FullCombo = false,
        Placement = 87
      }
    };

    var kittySkiesHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 38
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 76
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 61
      }
    };

    var kooshMooseHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 59
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 80
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 84
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 62
      }
    };

    var lordOfDookieHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 1,
        FullCombo = false,
        Placement = 53
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 94
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 88
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 99
      }
    };

    var lucasHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 2,
        FullCombo = false,
        Placement = 25
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = true,
        Placement = 13
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 11,
        FullCombo = false,
        Placement = 6
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 12
      }
    };

    var lukeHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 5,
        FullCombo = false,
        Placement = 51
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = true,
        Placement = 1
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 29
      }
    };

    var madLeftyHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 12,
        FullCombo = false,
        Placement = 10
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 29
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 105
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 3,
        FullCombo = false,
        Placement = 77
      }
    };

    var matrarchHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 12,
        FullCombo = false,
        Placement = 46
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 12,
        FullCombo = false,
        Placement = 64
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 5,
        FullCombo = false,
        Placement = 20
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 46
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 7,
        FullCombo = false,
        Placement = 112
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 109
      }
    };

    var meanMachineHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 30
      }
    };

    var mephistoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 70
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 44
      }
    };

    var minPinHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 69
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 2,
        FullCombo = false,
        Placement = 30
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 6,
        FullCombo = false,
        Placement = 89
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 4,
        FullCombo = false,
        Placement = 67
      }
    };

    var mrGompersHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 77
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 122
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 80
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 132
      }
    };

    var muetschensHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 79
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 70
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 91
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 89
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 104
      }
    };

    var nothHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 19
      }
    };

    var nbaKirklandHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 42
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 7,
        FullCombo = false,
        Placement = 11
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 12,
        FullCombo = true,
        Placement = 8
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 12,
        FullCombo = false,
        Placement = 54
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = true,
        Placement = 18
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 59
      }
    };

    var northernLassHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 66
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 44
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 61
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 65
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 68
      }
    };

    var nuttyWrayHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 39
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 27
      }
    };

    var omgeezusHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 90
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 111
      }
    };

    var paunchyDeerHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 4,
        FullCombo = false,
        Placement = 18
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 14
      }
    };

    var prouxHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 11,
        FullCombo = false,
        Placement = 19
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 4,
        FullCombo = false,
        Placement = 22
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 2,
        FullCombo = false,
        Placement = 55
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 10,
        FullCombo = false,
        Placement = 56
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 9,
        FullCombo = false,
        Placement = 48
      }
    };

    var prtmHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 156
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 37
      }
    };

    var quantumHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 43
      }
    };

    var radicalSniperHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 3,
        FullCombo = false,
        Placement = 21
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 63
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 67
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 87
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 63
      }
    };

    var retstakHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 1,
        FullCombo = false,
        Placement = 124
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 4,
        FullCombo = false,
        Placement = 83
      }
    };

    var rosscoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 6,
        FullCombo = false,
        Placement = 24
      }
    };

    var sabenHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 27
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 6,
        FullCombo = false,
        Placement = 3
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 6,
        FullCombo = false,
        Placement = 14
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 6,
        FullCombo = false,
        Placement = 30
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 74
      }
    };

    var saurHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 130
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 32
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 35
      }
    };

    var scarHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 7
      }
    };

    var seamusHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 68
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 1,
        FullCombo = false,
        Placement = 108
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 133
      }
    };

    var shadowHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 79
      }
    };

    var simpsoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 112
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 143
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 128
      }
    };

    var sirPaulygonHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 1,
        FullCombo = false,
        Placement = 20
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = false,
        Placement = 11
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 49
      }
    };

    var skepticalMarioHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 92
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 4
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 64
      }
    };

    var skootHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 104
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 138
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 141
      }
    };

    var slayerReigningHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 82
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 65
      }
    };

    var smokenRocketHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = false,
        Placement = 34
      }
    };

    var smrnovHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = true,
        Placement = 2
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = true,
        Placement = 8
      }
    };

    var sprinkyDinkHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 1
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 91
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 2,
        FullCombo = false,
        Placement = 46
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 80
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 83
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 106
      }
    };

    var swiftHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = true,
        Placement = 10
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 12,
        FullCombo = true,
        Placement = 3
      }
    };

    var toadStyleHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 4,
        FullCombo = false,
        Placement = 42
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 5,
        FullCombo = false,
        Placement = 58
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 10,
        FullCombo = false,
        Placement = 53
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 94
      }
    };

    var tripleTriadHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 97
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 129
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 25
      }
    };

    var trueVeteranHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 12,
        FullCombo = true,
        Placement = 9
      }
    };

    var txMookHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 62
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 1,
        FullCombo = false,
        Placement = 45
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 11,
        FullCombo = false,
        Placement = 32
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 11,
        FullCombo = false,
        Placement = 12
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 21
      }
    };

    var ultimateDespairHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 11,
        FullCombo = false,
        Placement = 16
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 11,
        FullCombo = false,
        Placement = 53
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 13
      }
    };

    var wickedGirlHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 8
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 89
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 3,
        FullCombo = false,
        Placement = 47
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 118
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 2,
        FullCombo = false,
        Placement = 91
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 142
      }
    };

    var wasIThatBadHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 45
      }
    };

    var wattyHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 3
      }
    };

    var weezyfuzzHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 1,
        FullCombo = false,
        Placement = 75
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 76
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 1,
        FullCombo = false,
        Placement = 136
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 53
      }
    };

    var wellingtonBalboHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 38
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 0,
        FullCombo = false,
        Placement = 85
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 33
      }
    };

    var whisperinClownHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 0,
        FullCombo = false,
        Placement = 41
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 58
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 8,
        FullCombo = false,
        Placement = 41
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 6,
        FullCombo = false,
        Placement = 45
      }
    };

    var fuggHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 98
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 45
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 1,
        FullCombo = false,
        Placement = 48
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 2,
        FullCombo = false,
        Placement = 100
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 73
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 123
      }
    };

    var wildWhiteNoiseHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2018,
        Rgsc = 0,
        FullCombo = false,
        Placement = 17
      },
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 10,
        FullCombo = false,
        Placement = 14
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 7,
        FullCombo = false,
        Placement = 26
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 2,
        FullCombo = false,
        Placement = 25
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 6,
        FullCombo = false,
        Placement = 37
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 5,
        FullCombo = false,
        Placement = 18
      }
    };

    var wildwoodMikeHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 54
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 2,
        FullCombo = false,
        Placement = 52
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 72
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 97
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 2,
        FullCombo = false,
        Placement = 86
      }
    };

    var woodsMonkHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 0,
        FullCombo = false,
        Placement = 90
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 0,
        FullCombo = false,
        Placement = 93
      }
    };

    var xlaxJesterHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2019,
        Rgsc = 1,
        FullCombo = false,
        Placement = 74
      },
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 2,
        FullCombo = false,
        Placement = 79
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 0,
        FullCombo = false,
        Placement = 98
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 8,
        FullCombo = false,
        Placement = 102
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 1,
        FullCombo = false,
        Placement = 137
      }
    };

    var neoHistoricalStats = new List<BcmHistoricalStats>
    {
      new BcmHistoricalStats
      {
        Year = 2020,
        Rgsc = 6,
        FullCombo = false,
        Placement = 17
      },
      new BcmHistoricalStats
      {
        Year = 2021,
        Rgsc = 3,
        FullCombo = false,
        Placement = 84
      },
      new BcmHistoricalStats
      {
        Year = 2022,
        Rgsc = 4,
        FullCombo = false,
        Placement = 70
      },
      new BcmHistoricalStats
      {
        Year = 2023,
        Rgsc = 4,
        FullCombo = false,
        Placement = 78
      }
    };

    builder.HasData
    (
      new BcmMiscStat
      {
        Id = 1,
        PlayerId = 76,
        HistoricalStats = JsonConvert.SerializeObject(kTEchoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 2,
        PlayerId = 53,
        HistoricalStats = JsonConvert.SerializeObject(aeoMonkeyHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 3,
        PlayerId = 152,
        HistoricalStats = JsonConvert.SerializeObject(acaeulsTHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 4,
        PlayerId = 25,
        HistoricalStats = JsonConvert.SerializeObject(aceHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 5,
        PlayerId = 155,
        HistoricalStats = JsonConvert.SerializeObject(ahayzoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 6,
        PlayerId = 27,
        HistoricalStats = JsonConvert.SerializeObject(albinoKidEliteHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 7,
        PlayerId = 39,
        HistoricalStats = JsonConvert.SerializeObject(benL72HistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 8,
        PlayerId = 101,
        HistoricalStats = JsonConvert.SerializeObject(bigEllHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 9,
        PlayerId = 123,
        HistoricalStats = JsonConvert.SerializeObject(blazeFlareonHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 10,
        PlayerId = 148,
        HistoricalStats = JsonConvert.SerializeObject(carpeAdamHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 11,
        PlayerId = 29,
        HistoricalStats = JsonConvert.SerializeObject(chewieHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 12,
        PlayerId = 142,
        HistoricalStats = JsonConvert.SerializeObject(cheznoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 13,
        PlayerId = 50,
        HistoricalStats = JsonConvert.SerializeObject(christophHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 14,
        PlayerId = 163,
        HistoricalStats = JsonConvert.SerializeObject(couchBurgularHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 15,
        PlayerId = 57,
        HistoricalStats = JsonConvert.SerializeObject(cptCookieHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 16,
        PlayerId = 143,
        HistoricalStats = JsonConvert.SerializeObject(crunchyGoblinHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 17,
        PlayerId = 87,
        HistoricalStats = JsonConvert.SerializeObject(danTheWhaleHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 18,
        PlayerId = 32,
        HistoricalStats = JsonConvert.SerializeObject(darkwingHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 19,
        PlayerId = 22,
        HistoricalStats = JsonConvert.SerializeObject(daveBodomHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 20,
        PlayerId = 49,
        HistoricalStats = JsonConvert.SerializeObject(davidMcCHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 21,
        PlayerId = 10,
        HistoricalStats = JsonConvert.SerializeObject(dubdeeHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 22,
        PlayerId = 156,
        HistoricalStats = JsonConvert.SerializeObject(dubstepHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 23,
        PlayerId = 140,
        HistoricalStats = JsonConvert.SerializeObject(dudeWithTheFaceHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 24,
        PlayerId = 38,
        HistoricalStats = JsonConvert.SerializeObject(elipheletHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 25,
        PlayerId = 8,
        HistoricalStats = JsonConvert.SerializeObject(emzFergiHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 26,
        PlayerId = 3,
        HistoricalStats = JsonConvert.SerializeObject(eohjayHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 27,
        PlayerId = 16,
        HistoricalStats = JsonConvert.SerializeObject(erutaercHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 28,
        PlayerId = 134,
        HistoricalStats = JsonConvert.SerializeObject(fineFeatHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 29,
        PlayerId = 28,
        HistoricalStats = JsonConvert.SerializeObject(fistaRobotoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 30,
        PlayerId = 85,
        HistoricalStats = JsonConvert.SerializeObject(flutteryChickenHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 31,
        PlayerId = 92,
        HistoricalStats = JsonConvert.SerializeObject(freakyHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 32,
        PlayerId = 84,
        HistoricalStats = JsonConvert.SerializeObject(freamHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 33,
        PlayerId = 75,
        HistoricalStats = JsonConvert.SerializeObject(hattonHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 34,
        PlayerId = 35,
        HistoricalStats = JsonConvert.SerializeObject(hawkeyeBarryHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 35,
        PlayerId = 5,
        HistoricalStats = JsonConvert.SerializeObject(hegemonicHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 36,
        PlayerId = 14,
        HistoricalStats = JsonConvert.SerializeObject(henkeHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 37,
        PlayerId = 30,
        HistoricalStats = JsonConvert.SerializeObject(hotcurlsHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 38,
        PlayerId = 15,
        HistoricalStats = JsonConvert.SerializeObject(icyThrasherHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 39,
        PlayerId = 21,
        HistoricalStats = JsonConvert.SerializeObject(infamousHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 40,
        PlayerId = 58,
        HistoricalStats = JsonConvert.SerializeObject(infernoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 41,
        PlayerId = 124,
        HistoricalStats = JsonConvert.SerializeObject(irishWarriorHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 42,
        PlayerId = 12,
        HistoricalStats = JsonConvert.SerializeObject(ironHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 43,
        PlayerId = 81,
        HistoricalStats = JsonConvert.SerializeObject(jBattlestarHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 44,
        PlayerId = 56,
        HistoricalStats = JsonConvert.SerializeObject(jimbotHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 45,
        PlayerId = 133,
        HistoricalStats = JsonConvert.SerializeObject(johnnyDeliciousHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 46,
        PlayerId = 34,
        HistoricalStats = JsonConvert.SerializeObject(kaitehHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 47,
        PlayerId = 18,
        HistoricalStats = JsonConvert.SerializeObject(kawiNinjaHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 48,
        PlayerId = 73,
        HistoricalStats = JsonConvert.SerializeObject(kezHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 49,
        PlayerId = 141,
        HistoricalStats = JsonConvert.SerializeObject(kingsOfDispairHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 50,
        PlayerId = 105,
        HistoricalStats = JsonConvert.SerializeObject(kittySkiesHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 51,
        PlayerId = 43,
        HistoricalStats = JsonConvert.SerializeObject(kooshMooseHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 104,
        PlayerId = 71,
        HistoricalStats = JsonConvert.SerializeObject(lordOfDookieHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 52,
        PlayerId = 46,
        HistoricalStats = JsonConvert.SerializeObject(lucasHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 53,
        PlayerId = 93,
        HistoricalStats = JsonConvert.SerializeObject(lukeHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 54,
        PlayerId = 48,
        HistoricalStats = JsonConvert.SerializeObject(madLeftyHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 55,
        PlayerId = 100,
        HistoricalStats = JsonConvert.SerializeObject(matrarchHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 56,
        PlayerId = 69,
        HistoricalStats = JsonConvert.SerializeObject(meanMachineHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 57,
        PlayerId = 86,
        HistoricalStats = JsonConvert.SerializeObject(mephistoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 58,
        PlayerId = 19,
        HistoricalStats = JsonConvert.SerializeObject(minPinHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 59,
        PlayerId = 68,
        HistoricalStats = JsonConvert.SerializeObject(mrGompersHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 60,
        PlayerId = 63,
        HistoricalStats = JsonConvert.SerializeObject(muetschensHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 61,
        PlayerId = 33,
        HistoricalStats = JsonConvert.SerializeObject(nothHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 62,
        PlayerId = 44,
        HistoricalStats = JsonConvert.SerializeObject(nbaKirklandHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 63,
        PlayerId = 36,
        HistoricalStats = JsonConvert.SerializeObject(northernLassHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 64,
        PlayerId = 77,
        HistoricalStats = JsonConvert.SerializeObject(nuttyWrayHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 65,
        PlayerId = 59,
        HistoricalStats = JsonConvert.SerializeObject(omgeezusHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 66,
        PlayerId = 47,
        HistoricalStats = JsonConvert.SerializeObject(paunchyDeerHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 67,
        PlayerId = 62,
        HistoricalStats = JsonConvert.SerializeObject(prouxHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 68,
        PlayerId = 7,
        HistoricalStats = JsonConvert.SerializeObject(prtmHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 69,
        PlayerId = 91,
        HistoricalStats = JsonConvert.SerializeObject(quantumHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 70,
        PlayerId = 89,
        HistoricalStats = JsonConvert.SerializeObject(radicalSniperHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 71,
        PlayerId = 51,
        HistoricalStats = JsonConvert.SerializeObject(retstakHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 72,
        PlayerId = 146,
        HistoricalStats = JsonConvert.SerializeObject(rosscoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 73,
        PlayerId = 24,
        HistoricalStats = JsonConvert.SerializeObject(sabenHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 74,
        PlayerId = 64,
        HistoricalStats = JsonConvert.SerializeObject(saurHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 75,
        PlayerId = 6,
        HistoricalStats = JsonConvert.SerializeObject(scarHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 76,
        PlayerId = 145,
        HistoricalStats = JsonConvert.SerializeObject(seamusHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 77,
        PlayerId = 96,
        HistoricalStats = JsonConvert.SerializeObject(shadowHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 78,
        PlayerId = 41,
        HistoricalStats = JsonConvert.SerializeObject(simpsoHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 79,
        PlayerId = 60,
        HistoricalStats = JsonConvert.SerializeObject(sirPaulygonHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 80,
        PlayerId = 78,
        HistoricalStats = JsonConvert.SerializeObject(skepticalMarioHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 81,
        PlayerId = 103,
        HistoricalStats = JsonConvert.SerializeObject(skootHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 82,
        PlayerId = 23,
        HistoricalStats = JsonConvert.SerializeObject(slayerReigningHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 83,
        PlayerId = 144,
        HistoricalStats = JsonConvert.SerializeObject(smokenRocketHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 84,
        PlayerId = 20,
        HistoricalStats = JsonConvert.SerializeObject(smrnovHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 85,
        PlayerId = 70,
        HistoricalStats = JsonConvert.SerializeObject(sprinkyDinkHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 86,
        PlayerId = 26,
        HistoricalStats = JsonConvert.SerializeObject(swiftHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 87,
        PlayerId = 72,
        HistoricalStats = JsonConvert.SerializeObject(toadStyleHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 88,
        PlayerId = 55,
        HistoricalStats = JsonConvert.SerializeObject(tripleTriadHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 89,
        PlayerId = 97,
        HistoricalStats = JsonConvert.SerializeObject(trueVeteranHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 90,
        PlayerId = 45,
        HistoricalStats = JsonConvert.SerializeObject(txMookHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 91,
        PlayerId = 61,
        HistoricalStats = JsonConvert.SerializeObject(ultimateDespairHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 92,
        PlayerId = 165,
        HistoricalStats = JsonConvert.SerializeObject(wickedGirlHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 93,
        PlayerId = 54,
        HistoricalStats = JsonConvert.SerializeObject(wasIThatBadHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 94,
        PlayerId = 135,
        HistoricalStats = JsonConvert.SerializeObject(wattyHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 95,
        PlayerId = 65,
        HistoricalStats = JsonConvert.SerializeObject(weezyfuzzHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 96,
        PlayerId = 37,
        HistoricalStats = JsonConvert.SerializeObject(wellingtonBalboHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 97,
        PlayerId = 88,
        HistoricalStats = JsonConvert.SerializeObject(whisperinClownHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 98,
        PlayerId = 79,
        HistoricalStats = JsonConvert.SerializeObject(fuggHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 99,
        PlayerId = 17,
        HistoricalStats = JsonConvert.SerializeObject(wildWhiteNoiseHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 100,
        PlayerId = 52,
        HistoricalStats = JsonConvert.SerializeObject(wildwoodMikeHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 101,
        PlayerId = 161,
        HistoricalStats = JsonConvert.SerializeObject(woodsMonkHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 102,
        PlayerId = 42,
        HistoricalStats = JsonConvert.SerializeObject(xlaxJesterHistoricalStats)
      },
      new BcmMiscStat
      {
        Id = 103,
        PlayerId = 83,
        HistoricalStats = JsonConvert.SerializeObject(neoHistoricalStats)
      }
    );
    #endregion
  }
}
