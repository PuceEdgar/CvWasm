using System.Text.Json.Serialization;

namespace CvWasm.Models;

public record HardSkillsModel(
   [property: JsonPropertyName("programming")] string[]? Programming,
   [property: JsonPropertyName("tools")] string? Tools,
   [property: JsonPropertyName("other")] string[]? Other,
   [property: JsonPropertyName("wayOfWorking")] string? WayOfWorking,
   [property: JsonPropertyName("languages")] string[]? Languages
    );