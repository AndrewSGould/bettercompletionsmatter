using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;
using TavisApi.Models;

namespace TavisApi.Context;

public class MonthlyExclusionsConfiguration : IEntityTypeConfiguration<MonthlyExclusion>
{
  public void Configure(EntityTypeBuilder<MonthlyExclusion> builder)
  {
    builder
      .HasKey(c => new { c.GameId, c.PlayerId });

    builder
      .HasOne(x => x.BcmPlayer)
      .WithMany(x => x.MonthlyExclusions)
      .HasForeignKey(x => x.PlayerId);

    builder
      .HasIndex(x => new { x.GameId, x.PlayerId })
      .IsUnique();
  }
}
