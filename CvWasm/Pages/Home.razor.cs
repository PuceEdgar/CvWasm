using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CvWasm.Pages;

public partial class Home
{
    //private CvModel? Cv;
    private Dictionary<Languages, CvModel> LoadedCvs = [];
    private Dictionary<Languages, Dictionary<string, string>[]>? CommandDescriptions = [];
    private ElementReference TextInput;
    private string Command = string.Empty;
    private string? AsciiArt;
    private Languages CurrentSelectedLanguage = Languages.eng;
    private DynamicComponent ChildComponent { get; set; } = default!;

    //TODO: unit tests/integration tests
    protected override async Task OnInitializedAsync()
    {
        await LoadDataFromStaticFiles();

        if (LoadedCvs.TryGetValue(CurrentSelectedLanguage, out CvModel cv))
        {
            ComponentManager.InitializeComponentsWithParameters(cv, CurrentSelectedLanguage, CommandDescriptions[CurrentSelectedLanguage]);
        }
    }

    private async Task LoadDataFromStaticFiles()
    {
        await LoadCvDataFromJson();
        await LoadAsciiArtFromFile();
        await LoadCommandDescriptionFromJson();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await FocusElement();
        }

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
            await ExecuteCommand();
        }
    }

    private async Task ExecuteCommand()
    {
        //TODO: refactor this. tolower creates new string. if using equals then no new string is created
        string lowerCaseCommand = Command.ToLower();
        switch (lowerCaseCommand)
        {
            case ClearCommand:
                ComponentManager.ClearWindow();
                break;
            case AboutCommand:
            case EducationCommand:
            case HardSkillsCommand:
            case SoftSkillsCommand:
            case ExperienceCommand:
            case HelpCommand:
                AddNewComponentForCommand(Command);
                break;
            case OpenGitHubCommand:
                await OpenLinkInNewTab(LoadedCvs[CurrentSelectedLanguage].About!.GitHubLink!);
                break;
            case OpenLinkedInCommand:
                await OpenLinkInNewTab(LoadedCvs[CurrentSelectedLanguage].About!.LinkedInLink!);
                break;
            case DownloadEngCvCommand:
                await DownloadCv(Languages.eng);
                break;
            case DownloadKorCvCommand:
                await DownloadCv(Languages.kor);
                break;
            case ShowEnglishCommand:
                await LoadCv(Languages.eng);
                break;
            case ShowKoreanCommand:
                await LoadCv(Languages.kor);
                break;
            default:
                LoadResultComponentForError();
                break;
        }

        Command = string.Empty;
    }

    private void AddNewComponentForCommand(string command)
    {
        var result = ComponentManager.GetExistingComponent(command);
        ComponentManager.AddComponentToLoadedComponentList(result);
    }

    private void LoadResultComponentForError()
    {
        var result = ComponentManager.CreateResultCommandAndData(ErrorManager.GenerateBadCommandErrorMessage(Command, CurrentSelectedLanguage), Command);
        ComponentManager.AddComponentToLoadedComponentList(result);
    }

    private async Task OpenLinkInNewTab(string url)
    {
        var commandResult = "Result: ";
        try
        {
            await JsService.CallJsFunctionToOpenUrl(url);
            commandResult += "Success";
        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        var result = ComponentManager.CreateResultCommandAndData(commandResult, Command);
        ComponentManager.AddComponentToLoadedComponentList(result);
    }

    private async Task DownloadCv(Languages language)
    {
        var commandResult = "Result: ";
        try
        {
            var base64 = await FileManager.GetBase64FromPdfCv(language.ToString());
            await JsService.CallJsFunctionToDownloadCv(language, base64);
            commandResult += "Success";
        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        var result = ComponentManager.CreateResultCommandAndData(commandResult, Command);
        ComponentManager.AddComponentToLoadedComponentList(result);
    }

    private async Task LoadCv(Languages language)
    {
        var commandResult = "Result: ";
        try
        {
            CurrentSelectedLanguage = language;
            await LoadCvDataFromJson();
            ComponentManager.InitializeComponentsWithParameters(LoadedCvs[CurrentSelectedLanguage], CurrentSelectedLanguage, CommandDescriptions[CurrentSelectedLanguage]);
            commandResult += "Success";

        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        var result = ComponentManager.CreateResultCommandAndData(commandResult, Command);
        ComponentManager.AddComponentToLoadedComponentList(result);
    }

    private async Task LoadCvDataFromJson()
    {
        try
        {
            if (!LoadedCvs.ContainsKey(CurrentSelectedLanguage))
            {
                LoadedCvs[CurrentSelectedLanguage] = await FileManager.LoadDataFromJson<CvModel>($"cv-data/cv-{CurrentSelectedLanguage}.json");
            }
        }
        catch (Exception)
        {
            var result = ComponentManager.CreateResultCommandAndData(ErrorManager.FailedToLoadCvMessage, "load cv");
            ComponentManager.AddComponentToLoadedComponentList(result);
        }
    }

    private async Task LoadAsciiArtFromFile()
    {
        try
        {
            AsciiArt = await FileManager.LoadDataAsString(AsciiArtPath);
        }
        catch (Exception)
        {
            var result = ComponentManager.CreateResultCommandAndData(ErrorManager.FailedToLoadAsciiArtMessage, "load ascii art");
            ComponentManager.AddComponentToLoadedComponentList(result);
        }
    }

    private async Task LoadCommandDescriptionFromJson()
    {
        try
        {
            CommandDescriptions = await FileManager.LoadDataFromJson<Dictionary<Languages, Dictionary<string, string>[]>>(CommandDescriptionPath);
        }
        catch (Exception)
        {
            var result = ComponentManager.CreateResultCommandAndData(ErrorManager.FailedToLoadCommandDescriptionMessage, "load command descriptions");
            ComponentManager.AddComponentToLoadedComponentList(result);
        }
    }
}