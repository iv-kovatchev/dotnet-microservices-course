using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Command {
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public required string HowTo { get; set; }

    [Required]
    public required string CommandLine { get; set; }

    [Required]
    public int PlatformId { get; set; }

    public required Platform Platform { get; set; }
}