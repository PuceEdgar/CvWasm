using CvWasm.DTO;
using CvWasm.Headers;
using CvWasm.Models;
using CvWasm.Pages;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Managers;

public class ComponentManager : ComponentBase, IComponentManager
{
    private readonly EnglishHeaders EnglishHeaders = new();
    private readonly KoreanHeaders KoreanHeaders = new();
    private Dictionary<string, ComponentMetadata> Components {  get; set; }

    //[CascadingParameter]
    //List<CommandAndData> LoadedComponents { get; set; }
    private readonly IComponentList _componentList;

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
    //private readonly IErrorManager _errorManager;

    //public ComponentManager(IErrorManager errorManager)
    //{
    //    _errorManager = errorManager;
    //}

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
                Parameters = { [nameof(About.AboutDetails)] = GetAboutPageDataFromCv(cv, language) }
            },
            [nameof(HardSkills)] = new ComponentMetadata()
            {
                Type = typeof(HardSkills),
                Name = "Hard Skills",
                Parameters = { [nameof(HardSkills.HardSkillsDetails)] = GetHardSkillsPageDataFromCv(cv, language) }
            },
            [nameof(SoftSkills)] = new ComponentMetadata()
            {
                Type = typeof(SoftSkills),
                Name = "Soft Skills",
                Parameters = { [nameof(SoftSkills.SoftSkillsDetails)] = cv.Skills.SoftSkills! }
            },
            [nameof(Education)] = new ComponentMetadata()
            {
                Type = typeof(Education),
                Name = "Education",
                Parameters = { [nameof(Education.EducationDetails)] = GetEducationPageDataFromCv(cv, language) }
            },
            [nameof(WorkExperience)] = new ComponentMetadata()
            {
                Type = typeof(WorkExperience),
                Name = "Work Experience",
                Parameters = {
                [nameof(WorkExperience.ListOfExperienceDetails)] = GetWorkExperiencePageDataFromCv(cv, language),
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

    private AboutPageData GetAboutPageDataFromCv(CvModel cv, Languages language)
    {
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
        AboutPageData aboutPageData = new()
        {
            FullName = new(headers.FullName, cv.About.FullName),
            DateOfBirth = new(headers.DateOfBirth, cv.About.DateOfBirth),
            Nationality = new(headers.Nationality, cv.About.Nationality),
            Email = new(headers.Email, cv.About.Email),
            GitHubLink = new(headers.GitHubLink, cv.About.GitHubLink),
            LinkedInLink = new(headers.LinkedInLink, cv.About.LinkedInLink),
            PersonalStatement = new(headers.PersonalStatement, cv.About.PersonalStatement),
        };

        return aboutPageData;
    }

    private EducationPageData GetEducationPageDataFromCv(CvModel cv, Languages language)
    {
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
        EducationPageData educationPageData = new()
        {
            UniversityName = new(headers.UniversityName, cv.Education.UniversityName),
            Location = new(headers.Location, cv.Education.Location),
            PeriodAttended = new(headers.PeriodAttended, cv.Education.PeriodAttended),
            Degree = new(headers.Degree, cv.Education.Degree),
        };

        return educationPageData;
    }

    private HardSkillsPageData GetHardSkillsPageDataFromCv(CvModel cv, Languages language)
    {
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
        HardSkillsPageData hardSkillsPageData = new()
        {
            Programming = new(headers.Programming, cv.Skills.HardSkills.Programming),
            Tools = new(headers.Tools, cv.Skills.HardSkills.Tools),
            Other = new(headers.Other, cv.Skills.HardSkills.Other),
            WayOfWorking = new(headers.WayOfWorking, cv.Skills.HardSkills.WayOfWorking),
            Languages = new(headers.Languages, cv.Skills.HardSkills.Languages)
        };

        return hardSkillsPageData;
    }

    private List<WorkExperiencePageData> GetWorkExperiencePageDataFromCv(CvModel cv, Languages language)
    {
        List<WorkExperiencePageData> listOfExperiencePageData = [];
        IHeaders headers = language == Languages.eng ? EnglishHeaders : KoreanHeaders;
        //var selectedExperience = cv.Experience[experienceIndex];

        foreach (var experience in cv.Experience)
        {
            WorkExperiencePageData workExperiencePageData = new()
            {
                TimePeriod = new(headers.TimePeriod, experience.TimePeriod),
                Company = new(headers.Company, experience.Company),
                Location = new(headers.Location, experience.Location),
                Position = new(headers.Position, experience.Position),
                JobDescription = new(headers.JobDescription, experience.JobDescription)
            };
            listOfExperiencePageData.Add(workExperiencePageData);
        }        

        return listOfExperiencePageData;
    }

    

    //private void LoadExperienceComponent()
    //{
    //    int currentExperienceIndex = 0;
    //    var SelectedComponent = new ComponentMetadata()
    //    {
    //        Type = typeof(WorkExperience),
    //        Name = "Work Experience",
    //        Parameters = {
    //            [nameof(WorkExperience.ExperienceDetails)] = GetWorkExperiencePageDataFromCv(currentExperienceIndex),
    //            [nameof(WorkExperience.TotalExperienceCount)] = Cv.Experience.Length,
    //            [nameof(WorkExperience.CurrentSelectedLanguage)] = CurrentSelectedLanguage
    //        }
    //    };

    //    LoadedComponents.Add(new()
    //    {
    //        Command = "experience",
    //        MetaData = SelectedComponent
    //    });
    //}
}
