using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.Configurations {
	public class AugustRecapConfiguration : IEntityTypeConfiguration<AugRecap> {
		public void Configure(EntityTypeBuilder<AugRecap> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.BcmPlayer)
				.WithOne(x => x.AugustRecap)
				.HasForeignKey<AugRecap>(x => x.PlayerId);
		}
	}
}
