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
        StartDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc),
        EndDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc)
      }
    );
  }
}
