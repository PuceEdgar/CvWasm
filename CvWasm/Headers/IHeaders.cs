namespace CvWasm.Headers;

public interface IHeaders
{
    //TODO: should i split this into smaller interfaces?
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

    //Hard skills
    string Programming { get; }
    string Tools { get; }
    string Other { get; }
    string WayOfWorking { get; }
    string Languages { get; }

    //Work experience
     string TimePeriod { get; }
     string Company { get; }
     string WorkLocation { get; }
     string Position { get; }
     string JobDescription { get; }

    //Portfolio
    string AppName { get; }
    string AppUrl { get; }
    string AppDescription { get; }
    string AppTechnologies { get; }
}
