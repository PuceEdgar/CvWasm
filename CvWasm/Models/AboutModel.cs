namespace CvWasm.Models;

public record AboutModel(
    string? FullName, 
    string? DateOfBirth, 
    string? Nationality, 
    string? Email, 
    string? GitHubLink, 
    string? LinkedInLink, 
    string? PersonalStatement
    );
