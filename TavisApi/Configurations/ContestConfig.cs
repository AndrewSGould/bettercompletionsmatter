using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class ContestConfiguration : IEntityTypeConfiguration<Contest>
{
  public void Configure(EntityTypeBuilder<Contest> builder)
  {
    //TODO: change this to smart enum
    builder.HasData
    (
      new Contest {
        Id = 1,
        Name = "Better Completions Matter",
        StartDate = DateTime.SpecifyKind(new DateTime(2022, 1, 1), DateTimeKind.Utc),
        EndDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc)
      },
      new Contest {
        Id = 2,
        Name = "Raid Boss",
        StartDate = DateTime.SpecifyKind(new DateTime(2022, 7, 1), DateTimeKind.Utc),
        EndDate = DateTime.SpecifyKind(new DateTime(2022, 8, 1), DateTimeKind.Utc)
      }
    );
  }
}
