using System.Text.Json.Serialization;

namespace CvWasm.Models;

public record PortfolioModel(
    [property: JsonPropertyName("appName")] string? AppName,
    [property: JsonPropertyName("appUrl")] string? AppUrl,
    [property: JsonPropertyName("appDescription")] string? AppDescription,
    [property: JsonPropertyName("technologies")] string? Technologies
    );

