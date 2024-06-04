using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasOne(x => x.Login)
      .WithOne(x => x.User)
      .HasForeignKey<Login>(x => x.UserId);

    builder.HasOne(x => x.BcmPlayer)
      .WithOne(x => x.User)
      .HasForeignKey<BcmPlayer>(x => x.UserId);

    builder.HasOne(x => x.DiscordLogin)
      .WithOne(x => x.User)
      .HasForeignKey<DiscordLogin>(x => x.UserId);
  }
}
