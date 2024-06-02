namespace CvWasm.Factory;

public static class ComponentFactory
{
    public static BaseComponent CreateComponent(string command, string? message = null)
    {
        return command switch
        {
            AboutCommand => new AboutComponent(),
            EducationCommand => new EducationComponent(),
            ExperienceCommand => new WorkExperienceComponent(),
            HardSkillsCommand => new HardSkillsComponent(),
            SoftSkillsCommand => new SoftSkillsComponent(),
            HelpCommand => new HelpComponent(),
            _ => new ResultComponent(message, command),
        };
    }
}
