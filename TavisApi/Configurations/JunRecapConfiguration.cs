using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.Models;

namespace TavisApi.Configurations {
	public class JunRecapConfiguration : IEntityTypeConfiguration<JunRecap> {
		public void Configure(EntityTypeBuilder<JunRecap> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.BcmPlayer)
				.WithOne(x => x.JunRecap)
				.HasForeignKey<JunRecap>(x => x.PlayerId);
		}
	}
}
