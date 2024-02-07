using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;

namespace TavisApi.Context;

public class PlayerYearlyChallengeConfiguration : IEntityTypeConfiguration<PlayerYearlyChallenge>
{
  public void Configure(EntityTypeBuilder<PlayerYearlyChallenge> builder)
  {
    builder.HasKey(x => new { x.YearlyChallengeId, x.PlayerId });

    builder.HasOne(x => x.YearlyChallenge)
              .WithMany()
              .HasForeignKey(x => x.YearlyChallengeId);
  }
}
