namespace CvWasm.DTO;

public record WorkExperiencePageData
{
    public KeyValuePair<string, string> TimePeriod { get; set; }
    public KeyValuePair<string, string> Company { get; set; }
    public KeyValuePair<string, string> Location { get; set; }
    public KeyValuePair<string, string> Position { get; set; }
    public KeyValuePair<string, string[]> JobDescription { get; set; }
}
