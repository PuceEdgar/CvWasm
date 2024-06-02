using CvWasm.Pages;

namespace CvWasm.Factory;

public class HardSkillsComponent : BaseComponent
{
    public HardSkillsComponent()
    {
        Type = typeof(HardSkills);
        Command = HardSkillsCommand;
        Parameters = new() { [nameof(HardSkills.HardSkillsDetails)] = PageDataLoader.GetHardSkillsPageDataFromCv() };
    }
}
