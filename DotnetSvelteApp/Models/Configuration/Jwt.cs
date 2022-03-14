using System.ComponentModel.DataAnnotations;

public class Jwt
    {
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
        [Required]
        public string? Secret { get; set; }
    }
