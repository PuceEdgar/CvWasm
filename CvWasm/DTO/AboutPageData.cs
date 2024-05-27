namespace CvWasm.DTO;

public record AboutPageData
{
    public KeyValuePair<string, string> FullName { get; set; }
    public KeyValuePair<string, string> DateOfBirth { get; set; }
    public KeyValuePair<string, string> Nationality { get; set; }
    public KeyValuePair<string, string> Email { get; set; }
    public KeyValuePair<string, string> GitHubLink { get; set; }
    public KeyValuePair<string, string> LinkedInLink { get; set; }
    public KeyValuePair<string, string> PersonalStatement { get; set; }
}
