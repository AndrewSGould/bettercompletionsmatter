using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tavis.Models;
using TavisApi.Configurations;
using TavisApi.V2.Authentication;
using TavisApi.V2.Bcm.Models;
using TavisApi.V2.Bcm.Rgsc.Models;
using TavisApi.V2.Discord.Models;
using TavisApi.V2.Models;
using TavisApi.V2.TrueAchievements.Models;
using TavisApi.V2.Users;

namespace TavisApi.Context {
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
		public DbSet<BcmPlayer> BcmPlayers { get; set; }
		public DbSet<BcmPlayerGame> BcmPlayerGames { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<GameGenre> GameGenres { get; set; }
		public DbSet<Registration> Registrations { get; set; }
		public DbSet<Login> Logins { get; set; }
		public DbSet<DiscordLogin> DiscordLogins { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<SyncHistory> SyncHistory { get; set; }
		public DbSet<BcmPlayerCompletionHistory> BcmPlayerCompletionHistory { get; set; }
		public DbSet<BcmCompletionHistory> BcmCompletionHistory { get; set; }
		public DbSet<BcmStat> BcmStats { get; set; }
		public DbSet<BcmRgsc> BcmRgsc { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<BcmMiscStat> BcmMiscStats { get; set; }
		public DbSet<YearlyChallenge> YearlyChallenges { get; set; }
		public DbSet<PlayerYearlyChallenge> PlayerYearlyChallenges { get; set; }
		public DbSet<JanRecap> JanRecap { get; set; }
		public DbSet<FebRecap> FebRecap { get; set; }
		public DbSet<MarRecap> MarRecap { get; set; }
		public DbSet<MonthlyExclusion> MonthlyExclusions { get; set; }
		public DbSet<AprRecap> AprRecap { get; set; }
		public DbSet<JunRecap> JunRecap { get; set; }
		public DbSet<JulyRecap> JulyRecap { get; set; }
		public DbSet<PlayerTopGenre> PlayerTopGenres { get; set; }
		public DbSet<FakeCompletion> FakeCompletions { get; set; }
		public DbSet<MayRecap> MayRecap { get; set; }

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
			modelBuilder.ApplyConfiguration(new BcmPlayerGameConfiguration());
			modelBuilder.ApplyConfiguration(new GameGenreConfiguration());
			modelBuilder.ApplyConfiguration(new ContestConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new SyncHistoryConfiguration());
			modelBuilder.ApplyConfiguration(new BcmStatConfiguration());
			modelBuilder.ApplyConfiguration(new UserRegistrationConfiguration());
			modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
			modelBuilder.ApplyConfiguration(new BcmRgscConfiguration());
			modelBuilder.ApplyConfiguration(new BcmMiscStatConfiguration());
			modelBuilder.ApplyConfiguration(new YearlyChallengeConfiguration());
			modelBuilder.ApplyConfiguration(new PlayerYearlyChallengeConfiguration());
			modelBuilder.ApplyConfiguration(new MonthlyExclusionsConfiguration());
			modelBuilder.ApplyConfiguration(new JanRecapConfiguration());
			modelBuilder.ApplyConfiguration(new FebRecapConfiguration());
			modelBuilder.ApplyConfiguration(new AprRecapConfiguration());
			modelBuilder.ApplyConfiguration(new PlayerTopGenreConfiguration());
			modelBuilder.ApplyConfiguration(new MayRecapConfiguration());
			modelBuilder.ApplyConfiguration(new JunRecapConfiguration());
			modelBuilder.ApplyConfiguration(new JulyRecapConfiguration());

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
