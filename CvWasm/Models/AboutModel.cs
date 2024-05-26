using System.Text.Json.Serialization;

namespace CvWasm.Models;

public record AboutModel(
   [property: JsonPropertyName("fullName")] string? FullName,
   [property: JsonPropertyName("dateOfBirth")] string? DateOfBirth,
   [property: JsonPropertyName("nationality")] string? Nationality,
   [property: JsonPropertyName("email")] string? Email,
   [property: JsonPropertyName("gitHubLink")] string? GitHubLink,
   [property: JsonPropertyName("linkedInLink")] string? LinkedInLink,
   [property: JsonPropertyName("personalStatement")] string? PersonalStatement
    );
