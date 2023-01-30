using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class BcmCompletionHistoryConfiguration : IEntityTypeConfiguration<BcmCompletionHistory>
{
  public void Configure(EntityTypeBuilder<BcmCompletionHistory> builder)
  {
    builder
      .HasKey(c => c.Id);

    builder
      .HasOne<Game>(x => x.Game)
      .WithMany(x => x.BcmCompletionHistories)
      .HasForeignKey(x => x.GameId);
  }
}
