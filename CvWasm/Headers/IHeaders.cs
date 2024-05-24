namespace CvWasm.Headers;

public interface IHeaders
{
    //About
    string FullName { get; }
    string DateOfBirth { get; }
    string Nationality { get; }
    string Email { get; }
    string GitHubLink { get; }
    string LinkedInLink { get; }
    string PersonalStatement { get; }

    //Education
    string UniversityName { get; }
    string Location { get; }
    string PeriodAttended { get; }
    string Degree { get; }
}
