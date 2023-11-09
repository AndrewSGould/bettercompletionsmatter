using DocumentFormat.OpenXml.Office2010.Excel;

namespace Tavis.Models;

public class DiscordSigninResponse
{
  public string Id { get; set; }
  public string Username { get; set; } // the user's username, not unique across the platform
  public string Discriminator { get; set; } // the user's Discord-tag
  public string? Global_Name { get; set; } // the user's display name, if it is set. For bots, this is the application name
  public string? Avatar { get; set; } // the user's avatar hash
  public bool Bot { get; set; } // whether the user belongs to an OAuth2 application 
  public bool System { get; set; } // whether the user is an Official Discord System user (part of the urgent message system) 
  public bool Mfa_Enabled { get; set; } // whether the user has two factor enabled on their account
  public string? Banner { get; set; } // the user's banner hash
  public string? Banner_Color { get; set; }
  public int? Accent_Color { get; set; } // the user's banner color encoded as an integer representation of hexadecimal color code
  public string Locale { get; set; } // the user's chosen language option
  public int Flags { get; set; } // the flags on a user's account
  public int Premium_Type { get; set; } // the type of Nitro subscription on a user's account
  public int Public_Flags { get; set; } // the public flags on a user's account
  // public string? Avatar_Decoration_Data { get; set; } // the user's avatar decoration hash // \"avatar_decoration_data\":{\"asset\":\"a_b9a64088e30fd3a6f2456c2e0f44f173\",\"sku_id\":\"1157411685687115858\"}
}
