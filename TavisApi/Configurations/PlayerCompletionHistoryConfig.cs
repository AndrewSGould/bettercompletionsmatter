using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class PlayerCompletionHistoryConfiguration : IEntityTypeConfiguration<PlayerCompletionHistory>
{
  public void Configure(EntityTypeBuilder<PlayerCompletionHistory> builder)
  {
    builder
      .HasKey(c => c.Id);

    builder
      .HasOne<Player>(x => x.Player)
      .WithMany(x => x.PlayerCompletionHistories)
      .HasForeignKey(x => x.PlayerId);

    builder
      .HasOne<Game>(x => x.Game)
      .WithMany(x => x.PlayerCompletionHistories)
      .HasForeignKey(x => x.GameId);
  }
}
