using CvWasm.Pages;

namespace CvWasm.Factory;

public class SoftSkillsComponent : BaseComponent
{
    public SoftSkillsComponent()
    {
        Type = typeof(SoftSkills);
        Command = SoftSkillsCommand;
        Parameters = new() { [nameof(SoftSkills.SoftSkillsDetails)] = StateContainer.LoadedCvs[StateContainer.CurrentSelectedLanguage].Skills!.SoftSkills! };
    }
}
