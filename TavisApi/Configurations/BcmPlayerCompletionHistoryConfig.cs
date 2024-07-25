using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.Context;

public class BcmPlayerCompletionHistoryConfiguration : IEntityTypeConfiguration<BcmPlayerCompletionHistory> {
	public void Configure(EntityTypeBuilder<BcmPlayerCompletionHistory> builder)
	{
		builder
			.HasKey(c => c.Id);

		builder
			.HasOne(x => x.BcmPlayer)
			.WithMany(x => x.BcmPlayerCompletionHistories)
			.HasForeignKey(x => x.PlayerId);

		builder
			.HasOne(x => x.Game)
			.WithMany(x => x.PlayerCompletionHistories)
			.HasForeignKey(x => x.GameId);
	}
}
