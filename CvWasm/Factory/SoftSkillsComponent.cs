using CvWasm.Pages;

namespace CvWasm.Factory;

public class SoftSkillsComponent : IComponent
{
    public ComponentMetadata CreateComponent()
    {
        var selectedLanguage = StateContainer.CurrentSelectedLanguage;
        return new ComponentMetadata()
        {
            Type = typeof(SoftSkills),
            Command = SoftSkillsCommand,
            Parameters = { [nameof(SoftSkills.SoftSkillsDetails)] = StateContainer.LoadedCvs[selectedLanguage].Skills!.SoftSkills! }
        };
    }
}
