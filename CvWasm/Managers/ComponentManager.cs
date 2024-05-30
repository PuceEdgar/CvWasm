using CvWasm.Models;
using CvWasm.Pages;

namespace CvWasm.Managers;

public class ComponentManager : IComponentManager
{
    private readonly IComponentListManager _componentList;
    private Dictionary<string, ComponentMetadata> Components { get; set; } = [];

    public ComponentManager(IComponentListManager componentList)
    {
        _componentList = componentList;
    }

    public void InitializeComponentsWithParameters(CvModel cv, Languages language, Dictionary<string, string>[] commandDescriptions)
    {
        Components = new(StringComparer.OrdinalIgnoreCase)
        {
            [AboutCommand] = new ComponentMetadata()
            {
                Type = typeof(About),
                Name = "About",
                Parameters = { [nameof(About.AboutDetails)] = PageDataLoader.GetAboutPageDataFromCv(cv.About!, language) }
            },
            [HardSkillsCommand] = new ComponentMetadata()
            {
                Type = typeof(HardSkills),
                Name = "Hard Skills",
                Parameters = { [nameof(HardSkills.HardSkillsDetails)] = PageDataLoader.GetHardSkillsPageDataFromCv(cv.Skills!.HardSkills!, language) }
            },
            [SoftSkillsCommand] = new ComponentMetadata()
            {
                Type = typeof(SoftSkills),
                Name = "Soft Skills",
                Parameters = { [nameof(SoftSkills.SoftSkillsDetails)] = cv.Skills!.SoftSkills! }
            },
            [EducationCommand] = new ComponentMetadata()
            {
                Type = typeof(Education),
                Name = "Education",
                Parameters = { [nameof(Education.EducationDetails)] = PageDataLoader.GetEducationPageDataFromCv(cv.Education!, language) }
            },
            [ExperienceCommand] = new ComponentMetadata()
            {
                Type = typeof(WorkExperience),
                Name = "Work Experience",
                Parameters = {
                [nameof(WorkExperience.ListOfExperienceDetails)] = PageDataLoader.GetWorkExperiencePageDataFromCv(cv.Experience!, language),
                [nameof(WorkExperience.CurrentSelectedLanguage)] = language
            }
            },
            [HelpCommand] = new ComponentMetadata()
            {
                Type = typeof(Help),
                Name = "Help",
                Parameters = { [nameof(Help.CommandDescriptions)] = commandDescriptions }
            }
        };
    }

    public void LoadComponent(string command)
    {
        var selectedComponent = Components![command];
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
}
