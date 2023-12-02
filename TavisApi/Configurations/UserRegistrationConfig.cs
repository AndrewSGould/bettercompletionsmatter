using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class UserRegistrationConfiguration : IEntityTypeConfiguration<UserRegistration>
{
  public void Configure(EntityTypeBuilder<UserRegistration> builder)
  {
    builder.HasKey(ur => new { ur.UserId, ur.RegistrationId });

    builder.HasOne(ur => ur.User)
        .WithMany(u => u.UserRegistrations)
        .HasForeignKey(ur => ur.UserId);

    builder.HasOne(ur => ur.Registration)
        .WithMany(r => r.UserRegistrations)
        .HasForeignKey(ur => ur.RegistrationId);
  }
}
