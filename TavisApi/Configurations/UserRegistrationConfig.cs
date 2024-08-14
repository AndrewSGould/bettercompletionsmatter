using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TavisApi.Users.Models;

namespace TavisApi.Configurations;

public class UserRegistrationConfiguration : IEntityTypeConfiguration<UserRegistration> {
	public void Configure(EntityTypeBuilder<UserRegistration> builder)
	{
		builder.HasKey(ur => new { ur.UserId });

		builder.HasOne(ur => ur.User)
										.WithMany(u => u.UserRegistrations)
										.HasForeignKey(ur => ur.UserId);
	}
}
