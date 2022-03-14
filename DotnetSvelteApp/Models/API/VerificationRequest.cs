using System.ComponentModel.DataAnnotations;

public class VerificationRequest
{
    [Required] public string Email { get; set; } = null!;

    [Required] public string Token { get; set; } = null!;
}