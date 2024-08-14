using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.Authentication.Models;
using TavisApi.Discord.Models;
using TavisApi.Models;
using TavisApi.Users.Models;

namespace TavisApi.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User> {
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasOne(x => x.Login)
						.WithOne(x => x.User)
						.HasForeignKey<Login>(x => x.UserId);

		builder.HasOne(x => x.Player)
						.WithOne(x => x.User)
						.HasForeignKey<Player>(x => x.UserId);

		builder.HasOne(x => x.DiscordLogin)
						.WithOne(x => x.User)
						.HasForeignKey<DiscordLogin>(x => x.UserId);
	}
}
