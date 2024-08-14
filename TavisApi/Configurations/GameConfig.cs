using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.Models;
using TavisApi.TrueAchievements.Models;

namespace TavisApi.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

        builder
                .HasOne(x => x.FeatureList)
                .WithOne(x => x.Game)
                .HasForeignKey<FeatureList>(x => x.FeatureListOfGameId);
    }
}
