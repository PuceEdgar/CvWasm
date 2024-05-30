namespace CvWasm;

public class ComponentMetadata
{
    public required Type Type { get; init; }
    public required string Command { get; init; }
    public Dictionary<string, object> Parameters { get; set; } = [];
}
