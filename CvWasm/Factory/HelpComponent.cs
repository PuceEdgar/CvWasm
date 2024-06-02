using CvWasm.Pages;

namespace CvWasm.Factory;

public class HelpComponent : BaseComponent
{
    public HelpComponent()
    {
        Type = typeof(Help);
        Command = HelpCommand;
        Parameters = new() { [nameof(Help.CommandDescriptions)] = StateContainer.CommandDescriptions[StateContainer.CurrentSelectedLanguage] };
    }
}
