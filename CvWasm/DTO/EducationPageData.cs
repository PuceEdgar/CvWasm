namespace CvWasm.DTO;

public record EducationPageData
{
    public KeyValuePair<string, string> UniversityName { get; set; }
    public KeyValuePair<string, string> Location { get; set; }
    public KeyValuePair<string, string> PeriodAttended { get; set; }
    public KeyValuePair<string, string> Degree { get; set; }
}
