namespace CvWasm.Factory;

public static class ComponentFactory
{
    public static IComponent CreateComponent(string command)
    {
        return command switch
        {
            AboutCommand => new AboutComponent(),
            EducationCommand => new EducationComponent(),
            ExperienceCommand => new WorkExperienceComponent(),
            HardSkillsCommand => new HardSkillsComponent(),
            SoftSkillsCommand => new SoftSkillsComponent(),
            HelpCommand => new HelpComponent(),
            _ => new ResultComponent(),
        };
    }
}
