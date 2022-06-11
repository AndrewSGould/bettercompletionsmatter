using Bcm.Models;
using Microsoft.EntityFrameworkCore;  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
  
namespace BcmApi.Context  
{ 
  public class BcmContext : DbContext  
  {  
    protected readonly IConfiguration Configuration;

    public BcmContext(DbContextOptions<BcmContext> options, IConfiguration configuration) : base(options)  
    {   
      Configuration = configuration;
    }  

    public DbSet<Game>? Games { get; set; }
    public DbSet<Player>? Players {get; set;}
    public DbSet<PlayersGame> PlayersGames {get;set;}
    public DbSet<Diagnostic> Diagnostics {get;set;}

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

      modelBuilder.Entity<PlayersGame>()
        .Property(p => p.Ownership)
        .HasConversion(
            p => p.Value,
            p => Ownership.FromValue(p));

      modelBuilder.Entity<PlayersGame>()
        .Property(p => p.Platform)
        .HasConversion(
            p => p.Value,
            p => Platform.FromValue(p));

      modelBuilder.Entity<Game>().ToTable("Game").HasKey("Id");
      modelBuilder.Entity<Player>().ToTable("Player").HasKey("Id");
      modelBuilder.Entity<PlayersGame>().ToTable("PlayersGame").HasKey(c => new {c.GameId, c.PlayerId});
      modelBuilder.Entity<Diagnostic>().ToTable("Diagnostic").HasKey("Id");
    }
  }
}
