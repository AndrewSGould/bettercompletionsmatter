using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    builder.HasKey(ur => new { ur.UserId, ur.RoleId });

    builder.HasOne(u => u.User)
        .WithMany(ur => ur.UserRoles)
        .HasForeignKey(ur => ur.UserId);

    builder.HasOne(r => r.Role)
        .WithMany(ur => ur.UserRoles)
        .HasForeignKey(ur => ur.RoleId);
  }
}
