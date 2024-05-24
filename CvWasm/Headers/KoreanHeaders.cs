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

    const string _programming = "사용 프로그래밍";
    const string _tools = "사용 도구";
    const string _other = "기타";
    const string _wayOfWorking = "작업 환경";
    const string _languages = "언어";

    const string _timePeriod = "재직 기간";
    const string _company = "회사";
    const string _workLocation = "위치";
    const string _position = "직급";
    const string _jobDescription = "업무";

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

    public string Programming => _programming;

    public string Tools => _tools;

    public string Other => _other;

    public string WayOfWorking => _wayOfWorking;

    public string Languages => _languages;

    public string TimePeriod => _timePeriod;

    public string Company => _company;

    public string WorkLocation => _workLocation;

    public string Position => _position;

    public string JobDescription => _jobDescription;
}
