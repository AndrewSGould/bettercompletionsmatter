using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.Configurations {
	public class SepRecapConfiguration : IEntityTypeConfiguration<SepRecap> {
		public void Configure(EntityTypeBuilder<SepRecap> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.BcmPlayer)
				.WithOne(x => x.SeptemberRecap)
				.HasForeignKey<SepRecap>(x => x.PlayerId);
		}
	}
}
