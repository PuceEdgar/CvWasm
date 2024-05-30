using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class Help
{
    [Parameter]
    public string? CurrentSelectedLanguage { get; set; }

    private Dictionary<string, Dictionary<string, string>[]> CommandDescriptions = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadCommandDescriptionFromJson();
    }

    private async Task LoadCommandDescriptionFromJson()
    {
        if (CommandDescriptions.Count == 0)
        {
            try
            {
                CommandDescriptions = await FileManager.LoadDataFromJson<Dictionary<string, Dictionary<string, string>[]>>(CommandDescriptionPath);
            }
            catch (Exception)
            {
              ComponentManager.AddErrorComponentWithMessage(ErrorManager.FailedToLoadCommandDescriptionMessage, "load command descriptions");
            }
        }        
    }
}
