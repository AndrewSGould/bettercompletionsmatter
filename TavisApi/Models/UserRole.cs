using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tavis.Models;

public class UserRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public Guid RoleId { get; set; } = Guid.NewGuid();
    public string RoleName { get; set; }
    public List<Login> Logins { get; } = new();
}
