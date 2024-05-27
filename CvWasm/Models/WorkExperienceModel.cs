using System.Text.Json.Serialization;

namespace CvWasm.Models;

public record WorkExperienceModel(
    [property: JsonPropertyName("timePeriod")] string? TimePeriod,
    [property: JsonPropertyName("company")] string? Company,
    [property: JsonPropertyName("location")] string? Location,
    [property: JsonPropertyName("position")] string? Position,
    [property: JsonPropertyName("jobDescription")] string[]? JobDescription
    );