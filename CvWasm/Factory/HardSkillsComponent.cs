using CvWasm.Pages;

namespace CvWasm.Factory;

public class HardSkillsComponent : IComponent
{
    public ComponentMetadata CreateComponent()
    {
        return new ComponentMetadata()
        {
            Type = typeof(HardSkills),
            Command = HardSkillsCommand,
            Parameters = { [nameof(HardSkills.HardSkillsDetails)] = PageDataLoader.GetHardSkillsPageDataFromCv() }
        };
    }
}
