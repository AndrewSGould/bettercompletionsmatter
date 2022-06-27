using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class PlayerGameConfiguration : IEntityTypeConfiguration<PlayerGame>
{
  public void Configure(EntityTypeBuilder<PlayerGame> builder)
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
      .HasKey(c => new {c.GameId, c.PlayerId});

    builder
      .HasOne<Player>(x => x.Player)
      .WithMany(x => x.PlayerGames)
      .HasForeignKey(x => x.PlayerId);

    builder
      .HasOne<Game>(x => x.Game)
      .WithMany(x => x.PlayerGames)
      .HasForeignKey(x => x.GameId);
  }
}
