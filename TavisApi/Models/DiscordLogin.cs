using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace Tavis.Models;

public class DiscordLogin
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
  public string DiscordId { get; set; }
  public string TokenType { get; set; }
  [EncryptColumn]
  public string AccessToken { get; set; }
  public long UserId { get; set; }
  public User User { get; set; }
}
