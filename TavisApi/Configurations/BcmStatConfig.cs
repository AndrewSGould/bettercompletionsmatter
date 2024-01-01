using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
      .HasOne(x => x.BcmPlayer)
      .WithOne(x => x.BcmMiscStats)
      .HasForeignKey<BcmMiscStat>(x => x.PlayerId);
  }
}
