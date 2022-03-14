using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{

    [Required(ErrorMessage = "Username required")]
    public string UserName { get; set; } = null!;

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}