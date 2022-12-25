using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
  public void Configure(EntityTypeBuilder<Game> builder)
  {
    builder
      .HasOne<FeatureList>(x => x.FeatureList)
      .WithOne(x => x.Game)
      .HasForeignKey<FeatureList>(x => x.FeatureListOfGameId);
  }
}
