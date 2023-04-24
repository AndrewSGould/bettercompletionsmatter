using Tavis.Models;
using Microsoft.EntityFrameworkCore;  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TavisApi.Context  
{ 
  public class TavisContext : DbContext  
  {  
    protected readonly IConfiguration Configuration;

    public TavisContext(DbContextOptions<TavisContext> options, IConfiguration configuration) : base(options)  
    {   
      Configuration = configuration;
    }  

    public DbSet<Game>? Games {get; set;}
    public DbSet<FeatureList>? FeatureLists {get; set;}
    public DbSet<Player>? Players {get;set;}
    public DbSet<PlayerGame>? PlayerGames {get;set;}
    public DbSet<Genre>? Genres {get;set;}
    public DbSet<GameGenre>? GameGenres {get;set;}
    public DbSet<Contest>? Contests {get;set;}
    public DbSet<PlayerContest>? PlayerContests {get;set;}
    public DbSet<Login>? Logins {get; set;}
    public DbSet<SyncHistory>? SyncHistory {get; set;}
    public DbSet<PlayerCompletionHistory>? PlayerCompletionHistory {get; set;}
    public DbSet<BcmCompletionHistory>? BcmCompletionHistory {get; set;}
    public DbSet<BcmStat>? BcmStats {get;set;}
    public DbSet<BcmRgsc>? BcmRgsc {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      // var configuration = new ConfigurationBuilder()
      //   .SetBasePath(Directory.GetCurrentDirectory())
      //   .AddJsonFile("appsettings.json")
      //   .Build();

      var connectionString = Configuration.GetConnectionString("DefaultConnection");
      optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfiguration(new PlayerConfiguration());
      modelBuilder.ApplyConfiguration(new GenreConfiguration());
      modelBuilder.ApplyConfiguration(new GameConfiguration());
      modelBuilder.ApplyConfiguration(new PlayerGameConfiguration());
      modelBuilder.ApplyConfiguration(new GameGenreConfiguration());
      modelBuilder.ApplyConfiguration(new ContestConfiguration());
      modelBuilder.ApplyConfiguration(new PlayerContestConfiguration());
      modelBuilder.ApplyConfiguration(new UserConfiguration());
      modelBuilder.ApplyConfiguration(new SyncHistoryConfiguration());

      var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
          v => v.ToUniversalTime(),
          v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

      var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
          v => v.HasValue ? v.Value.ToUniversalTime() : v,
          v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
          if (entityType.IsKeyless)
          {
              continue;
          }

          foreach (var property in entityType.GetProperties())
          {
              if (property.ClrType == typeof(DateTime))
              {
                  property.SetValueConverter(dateTimeConverter);
              }
              else if (property.ClrType == typeof(DateTime?))
              {
                  property.SetValueConverter(nullableDateTimeConverter);
              }
          }
      }
    }
  }
}
