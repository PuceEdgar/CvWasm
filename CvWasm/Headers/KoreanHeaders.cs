namespace CvWasm.Headers;

public class KoreanHeaders : IHeaders
{
    const string _fullName = "이름";
    const string _dateOfBirth = "생년월일";
    const string _nationality = "국적";
    const string _email = "이메일";
    const string _gitHubLink = "GitHubLink";
    const string _linkedInLink = "LinkedInLink";
    const string _personalStatement = "Personal Statement";

    const string _universityName = "대학교";
    const string _location = "위치";
    const string _periodAttended = "재학 기간";
    const string _degree = "학위";

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
