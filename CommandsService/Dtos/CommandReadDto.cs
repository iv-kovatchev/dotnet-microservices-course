public class CommandReadDto {
    public int Id { get; set; }

    public required string HotTo { get; set; }

    public required string CommandLine { get; set; }

    public int PlatformId { get; set; }
}