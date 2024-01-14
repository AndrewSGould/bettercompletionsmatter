using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;
using TavisApi.Models;

namespace TavisApi.Context;

public class BcmMonthlyStatConfiguration : IEntityTypeConfiguration<BcmMonthlyStat>
{
  public void Configure(EntityTypeBuilder<BcmMonthlyStat> builder)
  {
    builder
      .HasOne(x => x.BcmPlayer)
      .WithMany(x => x.BcmMonthlyStats)
      .HasForeignKey(x => x.BcmPlayerId);
  }
}
