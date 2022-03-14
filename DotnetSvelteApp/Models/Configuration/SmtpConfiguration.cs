using System.ComponentModel.DataAnnotations;

public class SmtpConfiguration
{
    [Required]
    public string Sender { get; set; } = null!;
    [Required]

    public string Host { get; set; } = null!;
    public int Port { get; set; } = 587;

    [Required]
    public string UserName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}