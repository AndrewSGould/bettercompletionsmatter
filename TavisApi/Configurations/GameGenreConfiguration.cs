using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
{
  public void Configure(EntityTypeBuilder<GameGenre> builder)
  {
    builder
      .Property(p => p.GenreId)
      .HasConversion(
        p => p.Value,
        p => GenreList.FromValue(p));

    builder
      .HasKey(x => new {x.GameId, x.GenreId});
  }
}
