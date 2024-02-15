using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class FebRecapConfiguration : IEntityTypeConfiguration<FebRecap>
{
  public void Configure(EntityTypeBuilder<FebRecap> builder)
  {
    builder.HasKey(x => x.Id);

    builder
      .HasOne(x => x.BcmPlayer)
      .WithOne(x => x.FebRecap)
      .HasForeignKey<FebRecap>(x => x.PlayerId);
  }
}
