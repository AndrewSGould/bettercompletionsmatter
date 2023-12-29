using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class BcmPlayerGameConfiguration : IEntityTypeConfiguration<BcmPlayerGame>
{
  public void Configure(EntityTypeBuilder<BcmPlayerGame> builder)
  {
    builder
      .Property(p => p.Ownership)
      .HasConversion(
        p => p!.Value,
        p => Ownership.FromValue(p));

    builder
      .Property(p => p.Platform)
      .HasConversion(
        p => p!.Value,
        p => Platform.FromValue(p));

    builder
      .HasKey(c => new { c.GameId, c.PlayerId });

    builder
      .HasOne(x => x.BcmPlayer)
      .WithMany(x => x.BcmPlayerGames)
      .HasForeignKey(x => x.PlayerId);

    builder
      .HasIndex(x => new { x.GameId, x.PlayerId })
      .IsUnique();
  }
}
