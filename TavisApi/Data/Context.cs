using Tavis.Models;
using Microsoft.EntityFrameworkCore;  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
  
namespace TavisApi.Context  
{ 
  public class TavisContext : DbContext  
  {  
    protected readonly IConfiguration Configuration;

    public TavisContext(DbContextOptions<TavisContext> options, IConfiguration configuration) : base(options)  
    {   
      Configuration = configuration;
    }  

    public DbSet<Game>? Games { get; set; }
    public DbSet<FeatureList>? FeatureLists { get; set; }
    public DbSet<Player>? Players {get;set;}
    public DbSet<PlayerGame>? PlayerGames {get;set;}
    public DbSet<Genre>? Genres {get;set;}
    public DbSet<GameGenre>? GameGenres {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var connectionString = configuration.GetConnectionString("WebApiDatabase");
      optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfiguration(new PlayerConfiguration());
      modelBuilder.ApplyConfiguration(new GenreConfiguration());
      modelBuilder.ApplyConfiguration(new GameConfiguration());
      modelBuilder.ApplyConfiguration(new PlayerGameConfiguration());
      modelBuilder.ApplyConfiguration(new GameGenreConfiguration());
    }
  }
}
