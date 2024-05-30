using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CvWasm.Pages;

public partial class Home
{
    private CvModel? Cv;
    private Dictionary<Languages, CvModel> LoadedCvs = [];
    private ElementReference TextInput;
    private string Command = string.Empty;
    private string? AsciiArt;
    private Languages CurrentSelectedLanguage = Languages.eng;
    private DynamicComponent ChildComponent { get; set; } = default!;

    //TODO: add try catch in case file reading fails. unit tests/integration tests
    //move file names to constants
    protected override async Task OnInitializedAsync()
    {
        await LoadDataFromStaticFiles();

        if (LoadedCvs[CurrentSelectedLanguage] is not null)
        {
            ComponentManager.InitializeComponentsWithParameters(LoadedCvs[CurrentSelectedLanguage], CurrentSelectedLanguage);
        }
    }

    private async Task LoadDataFromStaticFiles()
    {
        await LoadCvDataFromJson();
        await LoadAsciiArtFromFile();
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
        int componentCount = ComponentListManager.LoadedComponents.Count;
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && componentCount > 0 && ComponentListManager.LoadedComponents[componentCount - 1].MetaData!.Type == typeof(WorkExperience))
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
                ClearWindow();
                break;
            case AboutCommand:
            case EducationCommand:
            case HardSkillsCommand:
            case SoftSkillsCommand:
            case ExperienceCommand:
            case HelpCommand:
                ComponentManager.LoadComponent(Command);
                break;
            case OpenGitHubCommand:
                await OpenLinkInNewTab(Cv.About!.GitHubLink!);
                break;
            case OpenLinkedInCommand:
                await OpenLinkInNewTab(Cv.About!.LinkedInLink!);
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
                ComponentManager.AddErrorComponentWithMessage(ErrorManager.GenerateBadCommandErrorMessage(Command, CurrentSelectedLanguage), Command);
                break;
        }

        Command = string.Empty;
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

        ComponentManager.LoadCommandResultComponent(commandResult, Command);
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

        ComponentManager.LoadCommandResultComponent(commandResult, Command);
    }

    private async Task LoadCv(Languages language)
    {
        var commandResult = "Result: ";
        try
        {
            CurrentSelectedLanguage = language;
            await LoadCvDataFromJson();
            ComponentManager.InitializeComponentsWithParameters(LoadedCvs[CurrentSelectedLanguage], CurrentSelectedLanguage);
            commandResult += "Success";

        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        ComponentManager.LoadCommandResultComponent(commandResult, Command);
    }

    private void ClearWindow()
    {
        ComponentListManager.ClearList();
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
            ComponentManager.AddErrorComponentWithMessage(ErrorManager.FailedToLoadCvMessage, "load cv");
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
            ComponentManager.AddErrorComponentWithMessage(ErrorManager.FailedToLoadAsciiArtMessage, "load ascii art");
        }
    }
}