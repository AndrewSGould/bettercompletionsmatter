namespace Tavis.Models;

public class UserRegistration
{
  public long UserId { get; set; }
  public User User { get; set; } = new();

  public long RegistrationId { get; set; }
  public Registration Registration { get; set; } = new();

  public DateTime? RegistrationDate { get; set; }
}
