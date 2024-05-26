using System.Text.Json.Serialization;

namespace CvWasm.Models;

public record SoftSkillsModel(
    [property: JsonPropertyName("skills")] string[]? Skills
    );