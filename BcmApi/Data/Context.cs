using BcmApi.Models;
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
    public DbSet<PlayerCompletedGame> PlayerCompletedGames {get;set;}

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
      modelBuilder.Entity<Game>().ToTable("Game");
      modelBuilder.Entity<Player>().ToTable("Player");
      modelBuilder.Entity<PlayerCompletedGame>().ToTable("PlayersCompletedGames").HasKey(c => new {c.GameId, c.PlayerId});
    }
  }
}