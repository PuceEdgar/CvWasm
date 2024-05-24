namespace CvWasm.Models;

public record AboutModel
{
    public string? FullName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Email { get; set; }
    public string? GitHubLink { get; set; }
    public string? LinkedInLink { get; set; }
    public string? PersonalStatement { get; set; }
}