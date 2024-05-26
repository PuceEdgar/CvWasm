using System.Text.Json.Serialization;

namespace CvWasm.Models;

public record EducationModel(
   [property: JsonPropertyName("universityName")] string UniversityName,
   [property: JsonPropertyName("location")] string Location,
   [property: JsonPropertyName("periodAttended")] string PeriodAttended,
   [property: JsonPropertyName("degree")] string Degree
    );
