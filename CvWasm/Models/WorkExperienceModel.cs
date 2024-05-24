namespace CvWasm.Models;

public record WorkExperienceModel(string? TimePeriod, string? Company, string? Location, string? Position, string[]? JobDescription);
//{
//    public string? TimePeriod { get; set; }
//    public string? Company { get; set; }
//    public string? Location { get; set; }
//    public string? Position { get; set; }
//    public string[]? JobDescription { get; set; } = [];
//}