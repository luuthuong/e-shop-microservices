using System.ComponentModel.DataAnnotations;

namespace Core.BaseDTO;

public class AuthRequest
{
  [Required(ErrorMessage = "Username is required.")]
  public string UserName { get; set; }
  [Required(ErrorMessage = "Password is required.")]
  public string Password { get; set; }
}