using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TavisApi.Authentication.Models;
using TavisApi.Configurations;
using TavisApi.Discord.Models;
using TavisApi.Models;
using TavisApi.TrueAchievements.Models;
using TavisApi.Users.Models;

namespace TavisApi {
	public class TavisContext : DbContext {
		protected readonly IConfiguration Configuration;
		private readonly IEncryptionProvider _encryptProvider;

		public TavisContext(DbContextOptions<TavisContext> options, IConfiguration configuration) : base(options)
		{
			Configuration = configuration;
			_encryptProvider = new GenerateEncryptionProvider("+68TIPxJWUxxhjLMR9FGkQ==");
		}

		public DbSet<Game> Games { get; set; }
		public DbSet<FeatureList> FeatureLists { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<PlayerGame> PlayerGames { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<GameGenre> GameGenres { get; set; }
		public DbSet<Login> Logins { get; set; }
		public DbSet<DiscordLogin> DiscordLogins { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<SyncHistory> SyncHistory { get; set; }
		public DbSet<Role> Roles { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseNpgsql(connectionString);
			optionsBuilder.LogTo(Console.WriteLine);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.UseEncryption(_encryptProvider);

			modelBuilder.ApplyConfiguration(new GenreConfiguration());
			modelBuilder.ApplyConfiguration(new GameConfiguration());
			modelBuilder.ApplyConfiguration(new PlayerGameConfiguration());
			modelBuilder.ApplyConfiguration(new GameGenreConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new SyncHistoryConfiguration());
			modelBuilder.ApplyConfiguration(new UserRegistrationConfiguration());
			modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

			foreach (var entity in modelBuilder.Model.GetEntityTypes()) {
				entity.SetTableName(entity.GetTableName()?.ToLower());

				foreach (var property in entity.GetProperties()) {
					property.SetColumnName(property.GetColumnName()?.ToLower());
				}
			}

			var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
							v => v.ToUniversalTime(),
							v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

			var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
							v => v.HasValue ? v.Value.ToUniversalTime() : v,
							v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

			foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
				if (entityType.IsKeyless) {
					continue;
				}

				foreach (var property in entityType.GetProperties()) {
					if (property.ClrType == typeof(DateTime)) {
						property.SetValueConverter(dateTimeConverter);
					}
					else if (property.ClrType == typeof(DateTime?)) {
						property.SetValueConverter(nullableDateTimeConverter);
					}
				}
			}
		}
	}
}
