using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CvWasm.Pages;

public partial class Home
{
    private ElementReference TextInput;
    private string Command = string.Empty;
    private string? AsciiArt;
    private DynamicComponent ChildComponent { get; set; } = default!;

    //TODO: unit tests/integration tests
    protected override async Task OnInitializedAsync()
    {
        await LoadDataFromStaticFiles();

        if (StateContainer.LoadedCvs.TryGetValue(StateContainer.CurrentSelectedLanguage, out CvModel cv))
        {
            ComponentManager.InitializeComponentsWithParameters(cv, StateContainer.CurrentSelectedLanguage, StateContainer.CommandDescriptions[StateContainer.CurrentSelectedLanguage]);
        }
    }

    private async Task LoadDataFromStaticFiles()
    {
        await FileManager.LoadCvDataFromJson();
        await FileManager.LoadCommandDescriptionFromJson();
        AsciiArt = await FileManager.LoadAsciiArtFromFile();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await FocusElement();
        await JsService.CallJsFunctionByName("scrollToInput");
    }

    private async Task FocusElement()
    {
        await TextInput.FocusAsync();
    }

    //TODO:
    //1. implement back functionality. probably use list of navigated pages/commands and on arrow up or command back load previos command and maybe on arrow down/next show next if exists
    //2. change from input field to form? so that 'Enter' method is not called on each input change. Or change when 'Enter' is called

    private async Task KeyboardButtonPressed(KeyboardEventArgs e)
    {
        int componentCount = ComponentManager.LoadedComponents.Count;
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && componentCount > 0 && ComponentManager.LoadedComponents[componentCount - 1].Type == typeof(WorkExperience))
        {
            (ChildComponent?.Instance as WorkExperience)!.SelectCurrentWorkExperience(e.Code);
        }

        if (e.Code.Equals("Enter", StringComparison.InvariantCultureIgnoreCase)
            || e.Code.Equals("NumpadEnter", StringComparison.InvariantCultureIgnoreCase)
                || e.Key.Equals("Enter", StringComparison.InvariantCultureIgnoreCase))
        {
            await CommandService.ExecuteCommand(Command);
            Command = string.Empty;
        }
    }
}