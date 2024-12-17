using System.ComponentModel.DataAnnotations;

public class Platform
{
    public Platform()
    {
        Commands = new List<Command>();
    }

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int ExternalId { get; set; }

    public ICollection<Command> Commands { get; set; }
}