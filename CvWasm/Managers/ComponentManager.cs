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
                Command = AboutCommand,
                Parameters = { [nameof(About.AboutDetails)] = PageDataLoader.GetAboutPageDataFromCv(cv.About!, language) }
            },
            [HardSkillsCommand] = new ComponentMetadata()
            {
                Type = typeof(HardSkills),
                Command = HardSkillsCommand,
                Parameters = { [nameof(HardSkills.HardSkillsDetails)] = PageDataLoader.GetHardSkillsPageDataFromCv(cv.Skills!.HardSkills!, language) }
            },
            [SoftSkillsCommand] = new ComponentMetadata()
            {
                Type = typeof(SoftSkills),
                Command = SoftSkillsCommand,
                Parameters = { [nameof(SoftSkills.SoftSkillsDetails)] = cv.Skills!.SoftSkills! }
            },
            [EducationCommand] = new ComponentMetadata()
            {
                Type = typeof(Education),
                Command = EducationCommand,
                Parameters = { [nameof(Education.EducationDetails)] = PageDataLoader.GetEducationPageDataFromCv(cv.Education!, language) }
            },
            [ExperienceCommand] = new ComponentMetadata()
            {
                Type = typeof(WorkExperience),
                Command = ExperienceCommand,
                Parameters = {
                [nameof(WorkExperience.ListOfExperienceDetails)] = PageDataLoader.GetWorkExperiencePageDataFromCv(cv.Experience!, language),
                [nameof(WorkExperience.CurrentSelectedLanguage)] = language
            }
            },
            [HelpCommand] = new ComponentMetadata()
            {
                Type = typeof(Help),
                Command = HelpCommand,
                Parameters = { [nameof(Help.CommandDescriptions)] = commandDescriptions }
            }
        };
    }

    public ComponentMetadata CreateResultCommandAndData(string message, string command)
    {
        return new ComponentMetadata()
        {
            Type = typeof(CommandResult),
            Command = command,
            Parameters = { [nameof(CommandResult.Result)] = message }
        };

        //return new()
        //{
        //    Command = command,
        //    MetaData = componentMetadata
        //};
    }

    public void AddComponentToLoadedComponentList(ComponentMetadata component)
    {       
        _componentList.AddNewComponent(component);
    }

    public ComponentMetadata GetExistingComponent(string command)
    {
        return Components![command];
        //var componentMetadata = Components![command];
        //return new()
        //{
        //    Command = command,
        //    MetaData = componentMetadata
        //};
    }

    public List<ComponentMetadata> GetLoadedComponents()
    {
        return _componentList.LoadedComponents;
    }

    public void ClearWindow()
    {
        _componentList.ClearList();
    }
}
