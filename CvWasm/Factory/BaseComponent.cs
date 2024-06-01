namespace CvWasm.Factory;

public class BaseComponent
{
    public Type? Type { get; init; }
    public string? Command { get; init; }
    public Dictionary<string, object> Parameters { get; set; } = [];
}
