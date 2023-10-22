using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tavis.Models;

public class User
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public string? Xuid { get; set; }
  public string? Gamertag { get; set; }
  public Login Login { get; set; }
  public DiscordLogin DiscordLogin { get; set; }
  public List<UserRole> UserRoles { get; } = new();
}
