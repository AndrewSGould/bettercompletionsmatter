using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class BcmStatConfiguration : IEntityTypeConfiguration<BcmStat>
{
  public void Configure(EntityTypeBuilder<BcmStat> builder)
  {
    builder
      .HasKey(c => c.Id);

    builder
      .HasOne<Player>(x => x.Player)
      .WithOne(x => x.BcmStats)
      .HasForeignKey<BcmStat>(x => x.PlayerId);
  }
}
