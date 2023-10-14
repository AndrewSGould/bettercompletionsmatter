using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    //TODO: seed data for this many to many relationship with Login

    builder.HasData
    (
      new UserRole {
        Id = 1,
        RoleId = Guid.NewGuid(),
        RoleName = "Super Admin"
      },
      new UserRole {
        Id = 2,
        RoleId = Guid.NewGuid(),
        RoleName = "Bcm Admin"
      },
      new UserRole {
        Id = 3,
        RoleId = Guid.NewGuid(),
        RoleName = "User"
      }
    );
  }
}
