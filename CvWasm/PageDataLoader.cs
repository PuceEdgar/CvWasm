using CvWasm.DTO;
using CvWasm.Headers;

namespace CvWasm;

public static class PageDataLoader
{
    private static readonly EnglishHeaders EnglishHeaders = new();
    private static readonly KoreanHeaders KoreanHeaders = new();

    public static AboutPageData GetAboutPageDataFromCv()
    {
        var selectedLanguage = StateContainer.CurrentSelectedLanguage;
        var about = StateContainer.LoadedCvs[selectedLanguage].About;
        IHeaders headers = selectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        AboutPageData aboutPageData = new()
        {
            FullName = new(headers.FullName, about!.FullName!),
            DateOfBirth = new(headers.DateOfBirth, about.DateOfBirth!),
            Nationality = new(headers.Nationality, about.Nationality!),
            Email = new(headers.Email, about.Email!),
            GitHubLink = new(headers.GitHubLink, about.GitHubLink!),
            LinkedInLink = new(headers.LinkedInLink, about.LinkedInLink!),
            PersonalStatement = new(headers.PersonalStatement, about.PersonalStatement!),
        };

        return aboutPageData;
    }

    public static EducationPageData GetEducationPageDataFromCv()
    {
        var selectedLanguage = StateContainer.CurrentSelectedLanguage;
        var education = StateContainer.LoadedCvs[selectedLanguage].Education;
        IHeaders headers = selectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        EducationPageData educationPageData = new()
        {
            UniversityName = new(headers.UniversityName, education.UniversityName),
            Location = new(headers.Location, education.Location),
            PeriodAttended = new(headers.PeriodAttended, education.PeriodAttended),
            Degree = new(headers.Degree, education.Degree),
        };

        return educationPageData;
    }

    public static HardSkillsPageData GetHardSkillsPageDataFromCv()
    {
        var selectedLanguage = StateContainer.CurrentSelectedLanguage;
        var hardSkills = StateContainer.LoadedCvs[selectedLanguage].Skills.HardSkills;
        IHeaders headers = selectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        HardSkillsPageData hardSkillsPageData = new()
        {
            Programming = new(headers.Programming, hardSkills.Programming!),
            Tools = new(headers.Tools, hardSkills.Tools!),
            Other = new(headers.Other, hardSkills.Other!),
            WayOfWorking = new(headers.WayOfWorking, hardSkills.WayOfWorking!),
            Languages = new(headers.Languages, hardSkills.Languages!)
        };

        return hardSkillsPageData;
    }

    public static List<WorkExperiencePageData> GetWorkExperiencePageDataFromCv()
    {
        var selectedLanguage = StateContainer.CurrentSelectedLanguage;
        var experiences = StateContainer.LoadedCvs[selectedLanguage].Experience;
        List<WorkExperiencePageData> listOfExperiencePageData = [];
        IHeaders headers = selectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;

        foreach (var experience in experiences)
        {
            WorkExperiencePageData workExperiencePageData = new()
            {
                TimePeriod = new(headers.TimePeriod, experience.TimePeriod!),
                Company = new(headers.Company, experience.Company!),
                Location = new(headers.Location, experience.Location!),
                Position = new(headers.Position, experience.Position!),
                JobDescription = new(headers.JobDescription, experience.JobDescription!)
            };
            listOfExperiencePageData.Add(workExperiencePageData);
        }

        return listOfExperiencePageData;
    }
}
