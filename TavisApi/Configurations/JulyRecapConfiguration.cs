using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.Models;

namespace TavisApi.Configurations {
	public class JulyRecapConfiguration : IEntityTypeConfiguration<JulyRecap> {
		public void Configure(EntityTypeBuilder<JulyRecap> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.BcmPlayer)
				.WithOne(x => x.JulyRecap)
				.HasForeignKey<JulyRecap>(x => x.PlayerId);
		}
	}
}
