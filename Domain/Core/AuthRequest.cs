using System.ComponentModel.DataAnnotations;

namespace Domain.Core;

public class AuthRequest
{
  [Required(ErrorMessage = "Username is required.")]
  public string UserName { get; set; }
  [Required(ErrorMessage = "Password is required.")]
  public string Password { get; set; }
}