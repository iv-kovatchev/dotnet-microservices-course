using System.ComponentModel.DataAnnotations;

public class CommandCreateDto {
    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public required string HotTo { get; set; }

    [Required]
    [MinLength(4)]
    public required string CommandLine { get; set; }
}