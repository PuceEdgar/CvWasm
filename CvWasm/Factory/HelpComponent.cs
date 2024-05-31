using CvWasm.Pages;

namespace CvWasm.Factory;

public class HelpComponent : IComponent
{
    public ComponentMetadata CreateComponent()
    {
        var selectedLanguage = StateContainer.CurrentSelectedLanguage;
        return new ComponentMetadata()
        {
            Type = typeof(Help),
            Command = HelpCommand,
            Parameters = { [nameof(Help.CommandDescriptions)] = StateContainer.CommandDescriptions[selectedLanguage] }
        };
    }
}
