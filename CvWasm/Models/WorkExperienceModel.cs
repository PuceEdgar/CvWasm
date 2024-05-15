namespace CvWasm.Models;

public class WorkExperienceModel
{
    public string? TimePeriod { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public string? Position { get; set; }
    public string[]? JobDescription { get; set; } = [];
}