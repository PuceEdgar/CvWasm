using CvWasm.DTO;
using CvWasm.Headers;
using CvWasm.Managers;
using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace CvWasm.Pages;

public partial class Home
{
    private CvModel? Cv;
    private Dictionary<string, ComponentMetadata>? Components;
    //private ComponentMetadata? SelectedComponent;
    private ElementReference TextInput;
    private string? Command;
    private string? AsciiArt;
    //private int CurrentExperienceIndex = 0;    
    private Languages CurrentSelectedLanguage = Languages.eng;


    //List<CommandAndData> LoadedComponents { get; set; }
    //private readonly IComponentList? _componentList;

    private DynamicComponent ChildComponent {  get; set; } = default!;


    //TODO: add try catch in case file reading fails. unit tests/integration tests
    //move file names to constants
    protected override async Task OnInitializedAsync()
    {
        await LoadDataFromStaticFiles();

        if (Cv is not null) 
        {
           ComponentManager.InitializeComponentsWithParameters(Cv, CurrentSelectedLanguage);
        }        
    }

    private async Task LoadDataFromStaticFiles()
    {
        await LoadCvDataFromJson();
        await LoadAsciiArtFromFile();
        //await LoadCommandDescriptionFromJson();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await FocusElement();
        }

        await JSRuntime.InvokeVoidAsync("scrollToInput");
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
        int componentCount = ComponentList.LoadedComponents.Count;
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && componentCount > 0 && ComponentList.LoadedComponents[componentCount-1].MetaData!.Type == typeof(WorkExperience))
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
            //case ExperienceCommand:
            //    LoadExperienceComponent();
                break;
            case OpenGitHubCommand:
                await OpenLinkInNewTab(Cv.About.GitHubLink);
                break;
            case OpenLinkedInCommand:
                await OpenLinkInNewTab(Cv.About.LinkedInLink);
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
            await JSRuntime.InvokeVoidAsync("open", url, "_blank");
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
            await JSRuntime.InvokeVoidAsync("downloadFile", $"cv_{language}.pdf", base64);
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
            ComponentManager.InitializeComponentsWithParameters(Cv!, CurrentSelectedLanguage);
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
        ComponentList.ClearList();
    }


    private async Task LoadCvDataFromJson()
    {
        try
        {
            Cv = await FileManager.LoadDataFromJson<CvModel>($"cv-data/cv-{CurrentSelectedLanguage}.json");
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