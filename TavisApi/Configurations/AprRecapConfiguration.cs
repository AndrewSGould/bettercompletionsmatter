using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.Models;

namespace TavisApi.Configurations
{
	public class AprRecapConfiguration : IEntityTypeConfiguration<AprRecap>
	{
		public void Configure(EntityTypeBuilder<AprRecap> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.BcmPlayer)
				.WithOne(x => x.AprRecap)
				.HasForeignKey<AprRecap>(x => x.PlayerId);
		}
	}
}
