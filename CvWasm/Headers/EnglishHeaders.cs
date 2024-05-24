namespace CvWasm.Headers;

public class EnglishHeaders : IHeaders
{
    const string _fullName = "Full Name";
    const string _dateOfBirth = "Date of Birth";
    const string _nationality = "Nationality";
    const string _email = "Email";
    const string _gitHubLink = "GitHubLink";
    const string _linkedInLink = "LinkedInLink";
    const string _personalStatement = "Personal Statement";

    const string _universityName = "University";
    const string _location = "Location";
    const string _periodAttended = "Period Attended";
    const string _degree = "Degree";

    public string FullName => _fullName;

    public string DateOfBirth => _dateOfBirth;

    public string Nationality => _nationality;

    public string Email => _email;

    public string GitHubLink => _gitHubLink;

    public string LinkedInLink => _linkedInLink;

    public string PersonalStatement => _personalStatement;

    public string UniversityName => _universityName;

    public string Location => _location;

    public string PeriodAttended => _periodAttended;

    public string Degree => _degree;
}
