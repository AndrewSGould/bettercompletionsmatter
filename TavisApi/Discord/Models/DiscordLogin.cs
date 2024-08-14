using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TavisApi.V2.Users;

namespace TavisApi.Discord.Models;
public class DiscordLogin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; } // TODO: change my longs to ulongs
    public ulong DiscordId { get; set; }
    public string TokenType { get; set; }
    [EncryptColumn]
    public string? AccessToken { get; set; }
    [EncryptColumn]
    public string? RefreshToken { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
}

public class DiscordConnect
{
    public string TokenType { get; set; } = "";
    public string AccessToken { get; set; } = "";
}

public class DiscordConnectV2
{
    public string? Code { get; set; }
}
