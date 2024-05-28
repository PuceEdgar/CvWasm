using CvWasm.DTO;
using CvWasm.Headers;
using CvWasm.Models;
using CvWasm.Pages;

namespace CvWasm.Managers;

public class ComponentManager : IComponentManager
{
    private readonly IComponentList _componentList;
    private readonly EnglishHeaders EnglishHeaders = new();
    private readonly KoreanHeaders KoreanHeaders = new();
    private Dictionary<string, ComponentMetadata> Components { get; set; } = [];

    public ComponentManager(IComponentList componentList)
    {
        _componentList = componentList;
    }

    private readonly Dictionary<string, string> ValidComponentCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        [AboutCommand] = nameof(About),
        [ExperienceCommand] = nameof(WorkExperience),
        [HardSkillsCommand] = nameof(HardSkills),
        [SoftSkillsCommand] = nameof(SoftSkills),
        [EducationCommand] = nameof(Education),
        [HelpCommand] = nameof(Help)
    };

    public void AddErrorComponentWithMessage(string errorMessage, string? command)
    {
        var component = new ComponentMetadata()
        {
            Type = typeof(Error),
            Name = "Error",
            Parameters = {
                [nameof(Error.ErrorMessage)] = errorMessage,
            }
        };

        _componentList.AddNewComponent(new()
        {
            Command = command,
            MetaData = component
        });
    }

    public void InitializeComponentsWithParameters(CvModel cv, Languages language)
    {
        Components = new(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(About)] = new ComponentMetadata()
            {
                Type = typeof(About),
                Name = "About",
                Parameters = { [nameof(About.AboutDetails)] = GetAboutPageDataFromCv(cv.About!, language) }
            },
            [nameof(HardSkills)] = new ComponentMetadata()
            {
                Type = typeof(HardSkills),
                Name = "Hard Skills",
                Parameters = { [nameof(HardSkills.HardSkillsDetails)] = GetHardSkillsPageDataFromCv(cv.Skills!.HardSkills!, language) }
            },
            [nameof(SoftSkills)] = new ComponentMetadata()
            {
                Type = typeof(SoftSkills),
                Name = "Soft Skills",
                Parameters = { [nameof(SoftSkills.SoftSkillsDetails)] = cv.Skills!.SoftSkills! }
            },
            [nameof(Education)] = new ComponentMetadata()
            {
                Type = typeof(Education),
                Name = "Education",
                Parameters = { [nameof(Education.EducationDetails)] = GetEducationPageDataFromCv(cv.Education!, language) }
            },
            [nameof(WorkExperience)] = new ComponentMetadata()
            {
                Type = typeof(WorkExperience),
                Name = "Work Experience",
                Parameters = {
                [nameof(WorkExperience.ListOfExperienceDetails)] = GetWorkExperiencePageDataFromCv(cv.Experience!, language),
                [nameof(WorkExperience.CurrentSelectedLanguage)] = language
            }
            },
            [nameof(Help)] = new ComponentMetadata()
            {
                Type = typeof(Help),
                Name = "Help",
                Parameters = { [nameof(Help.CurrentSelectedLanguage)] = language.ToString() }
            }
        };

    }

    public void LoadComponent(string command)
    {
        var componentName = ValidComponentCommands[command];
        var selectedComponent = Components![componentName];
        _componentList.AddNewComponent(new()
        {
            Command = command,
            MetaData = selectedComponent
        });
    }

    public void LoadCommandResultComponent(string message, string command)
    {
       var selectedComponent = new ComponentMetadata()
        {
            Type = typeof(CommandResult),
            Name = "Command Result",
            Parameters = { [nameof(CommandResult.Result)] = message }
        };
        _componentList.AddNewComponent(new()
        {
            Command = command,
            MetaData = selectedComponent
        });
    }

    private AboutPageData GetAboutPageDataFromCv(AboutModel about, Languages language)
    {
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
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

    private EducationPageData GetEducationPageDataFromCv(EducationModel education, Languages language)
    {
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
        EducationPageData educationPageData = new()
        {
            UniversityName = new(headers.UniversityName, education.UniversityName),
            Location = new(headers.Location, education.Location),
            PeriodAttended = new(headers.PeriodAttended, education.PeriodAttended),
            Degree = new(headers.Degree, education.Degree),
        };

        return educationPageData;
    }

    private HardSkillsPageData GetHardSkillsPageDataFromCv(HardSkillsModel hardSkills, Languages language)
    {
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
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

    private List<WorkExperiencePageData> GetWorkExperiencePageDataFromCv(WorkExperienceModel[] experiences, Languages language)
    {
        List<WorkExperiencePageData> listOfExperiencePageData = [];
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;

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
