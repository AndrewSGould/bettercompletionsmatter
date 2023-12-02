using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class ContestConfiguration : IEntityTypeConfiguration<Registration>
{
  public void Configure(EntityTypeBuilder<Registration> builder)
  {
    builder.HasData
    (
      new Registration
      {
        Id = 1,
        Name = "Better Completions Matter",
        StartDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc),
        EndDate = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc)
      },
      new Registration
      {
        Id = 2,
        Name = "Calendar Project",
      }
    );
  }
}
