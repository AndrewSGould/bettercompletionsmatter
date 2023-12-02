using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tavis.Models;

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public ulong DiscordId { get; set; }
    public string RoleName { get; set; } = "";
    public List<UserRole> UserRoles { get; } = new();
}
