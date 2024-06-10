using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CvWasm.Pages;

public partial class Home
{
    private ElementReference TextInput;
    private string Command = string.Empty;
    private string? AsciiArt;
    private DynamicComponent ChildComponent { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataFromStaticFiles();
    }

    private async Task LoadDataFromStaticFiles()
    {
        await FileService.LoadCvDataFromJson();
        await FileService.LoadCommandDescriptionFromJson();
        AsciiArt = await FileService.LoadAsciiArtFromFile();
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

    private async Task KeyboardButtonPressed(KeyboardEventArgs e)
    {
        
        if (IsArrowCodeAndLastItemIsWorkExperience(e))
        {
            (ChildComponent?.Instance as WorkExperience)!.SelectCurrentWorkExperience(e.Code);
        }

        if (IsEnterCode(e))
        {
            await CommandService.ExecuteCommand(Command);
            Command = string.Empty;
        }
    }

    private bool IsArrowCodeAndLastItemIsWorkExperience(KeyboardEventArgs e)
    {
        int componentCount = ComponentRepository.LoadedComponents.Count;
        return (e.Code == "ArrowLeft" || e.Code == "ArrowRight") && componentCount > 0 && ComponentRepository.LoadedComponents[componentCount - 1].Type == typeof(WorkExperience);
    }

    private static bool IsEnterCode(KeyboardEventArgs e)
    {
        return e.Code.Equals("Enter", StringComparison.InvariantCultureIgnoreCase)
                    || e.Code.Equals("NumpadEnter", StringComparison.InvariantCultureIgnoreCase)
                        || e.Key.Equals("Enter", StringComparison.InvariantCultureIgnoreCase);
    }
}