using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.V2.Bcm.Models;

namespace TavisApi.Context;

public class JanRecapConfiguration : IEntityTypeConfiguration<JanRecap> {
	public void Configure(EntityTypeBuilder<JanRecap> builder)
	{
		builder.HasKey(x => x.Id);

		builder
			.HasOne(x => x.BcmPlayer)
			.WithOne(x => x.JanRecap)
			.HasForeignKey<JanRecap>(x => x.PlayerId);
	}
}
