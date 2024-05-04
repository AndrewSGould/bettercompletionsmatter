using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class PlayerTopGenreConfiguration : IEntityTypeConfiguration<PlayerTopGenre>
{
  public void Configure(EntityTypeBuilder<PlayerTopGenre> builder)
  {
    builder
      .Property(p => p.GenreId)
      .HasConversion(
        p => p.Value,
        p => GenreList.FromValue(p));

    builder
      .HasKey(x => new {x.PlayerId, x.GenreId});
  }
}
