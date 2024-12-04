using System.ComponentModel.DataAnnotations;

public class PlatformCreateDto {
    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    public required string Publisher { get; set; }

    [Required]
    public required string Cost { get; set; }
}