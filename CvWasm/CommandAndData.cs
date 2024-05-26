namespace CvWasm;

public record CommandAndData
{
    public string? Command { get; set; }
    public ComponentMetadata? MetaData { get; set; }
}
