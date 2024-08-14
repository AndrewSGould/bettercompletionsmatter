using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;
using TavisApi.TrueAchievements.Models;

namespace TavisApi.Configurations;

public class SyncHistoryConfiguration : IEntityTypeConfiguration<SyncHistory>
{
    public void Configure(EntityTypeBuilder<SyncHistory> builder)
    {
        builder
                        .Property(p => p.Profile)
                        .HasConversion(
                                        p => p!.Value,
                                        p => SyncProfileList.FromValue(p));
    }
}
