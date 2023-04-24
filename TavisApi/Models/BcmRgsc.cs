using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tavis.Models;

public class BcmRgsc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id {get; set;}
    public DateTime? Issued {get; set;}
    public bool Rerolled {get; set;}

    public int? GameId {get; set;}
    public int? PlayerId {get;set;}
}
