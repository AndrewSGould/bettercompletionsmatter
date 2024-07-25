using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.V2.Bcm.Rgsc.Models;

namespace TavisApi.Context;

public class BcmRgscConfiguration : IEntityTypeConfiguration<BcmRgsc> {
	public void Configure(EntityTypeBuilder<BcmRgsc> builder)
	{
		builder
			.HasOne(x => x.BcmPlayer)
			.WithMany(x => x.BcmRgscs)
			.HasForeignKey(x => x.BcmPlayerId);
	}
}
