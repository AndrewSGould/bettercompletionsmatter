using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class UserConfiguration : IEntityTypeConfiguration<Login>
{
  public void Configure(EntityTypeBuilder<Login> builder)
  {
    //TODO: change this to smart enum
    builder.HasData
    (
      new Login {
        Id = 1,
        Username = "johndoe",
        Password = "def@123"
      }
    );
  }
}
