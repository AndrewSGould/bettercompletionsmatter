using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class ContestConfiguration : IEntityTypeConfiguration<Contest>
{
  public void Configure(EntityTypeBuilder<Contest> builder)
  {
    builder.HasData
    (
      new Contest {
        Id = 1,
        Name = "Better Completions Matter",
        StartDate = new DateTime(2022, 1, 1),
        EndDate = new DateTime(2023, 1, 1)
      },
      new Contest {
        Id = 2,
        Name = "Raid Boss",
        StartDate = new DateTime(2022, 7, 1),
        EndDate = new DateTime(2022, 8, 1)
      }
    );
  }
}
