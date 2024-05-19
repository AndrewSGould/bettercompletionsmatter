using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Configurations
{
	public class MayRecapConfiguration : IEntityTypeConfiguration<MayRecap>
	{
		public void Configure(EntityTypeBuilder<MayRecap> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.BcmPlayer)
				.WithOne(x => x.MayRecap)
				.HasForeignKey<MayRecap>(x => x.PlayerId);
		}
	}
}
