using CvWasm.DTO;
using CvWasm.Headers;
using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace CvWasm.Pages;

public partial class Home
{
    private CvModel? Cv;
    private Dictionary<string, ComponentMetadata>? Components;
    private ComponentMetadata? SelectedComponent;
    private ElementReference TextInput;
    private string? Command;
    private string? AsciiArt;
    private int CurrentExperienceIndex = 0;    
    private Languages CurrentSelectedLanguage = Languages.eng;
    private Dictionary<string, string[]> CommandDescription = [];
    private List<CommandAndData> LoadedComponents = [];
    private readonly EnglishHeaders EnglishHeaders = new();
    private readonly KoreanHeaders KoreanHeaders = new();
    private readonly Dictionary<string, string> ValidComponentCommands = new()
    {
        [AboutCommand] = nameof(About),
        [ExperienceCommand] = nameof(WorkExperience),
        [HardSkillsCommand] = nameof(HardSkills),
        [SoftSkillsCommand] = nameof(SoftSkills),
        [EducationCommand] = nameof(Education),
        [HelpCommand] = nameof(Help)
    };

    //TODO: add try catch in case file reading fails. unit tests/integration tests
    //move file names to constants
    protected override async Task OnInitializedAsync()
    {
        await LoadDataFromStaticFiles();
        InitializeComponentsWithParameters();
    }

    private async Task LoadDataFromStaticFiles()
    {
        Cv = await Http.GetFromJsonAsync<CvModel>($"cv-data/cv-eng.json");
        AsciiArt = await Http.GetStringAsync("file-data/ascii-welcome.txt");
        CommandDescription = await Http.GetFromJsonAsync<Dictionary<string, string[]>>("file-data/CommandDescription.json");        
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

    private void InitializeComponentsWithParameters()
    {
        Components = new(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(About)] = new ComponentMetadata()
            {
                Type = typeof(About),
                Name = "About",
                Parameters = { [nameof(About.AboutDetails)] = GetAboutPageDataFromCv() }
            },
            [nameof(HardSkills)] = new ComponentMetadata()
            {
                Type = typeof(HardSkills),
                Name = "Hard Skills",
                Parameters = { [nameof(HardSkills.HardSkillsDetails)] = Cv.Skills!.HardSkills! }
            },
            [nameof(SoftSkills)] = new ComponentMetadata()
            {
                Type = typeof(SoftSkills),
                Name = "Soft Skills",
                Parameters = { [nameof(SoftSkills.Data)] = Cv.Skills.SoftSkills! }
            },
            [nameof(Education)] = new ComponentMetadata()
            {
                Type = typeof(Education),
                Name = "Education",
                Parameters = { [nameof(Education.EducationDetails)] = Cv.Education! }
            },
            [nameof(Help)] = new ComponentMetadata()
            {
                Type = typeof(Help),
                Name = "Help",
                Parameters = { [nameof(Help.DisplayLanguage)] = CurrentSelectedLanguage, [nameof(Help.CommandDescriptions)] = CommandDescription }
            }
        };
    }

    private AboutPageData GetAboutPageDataFromCv()
    {
        IHeaders headers = CurrentSelectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        AboutPageData aboutPageData = new()
        {
            FullName = new(headers.FullName, Cv.About.FullName),
            DateOfBirth = new(headers.DateOfBirth, Cv.About.DateOfBirth),
            Nationality = new(headers.Nationality, Cv.About.Nationality),
            Email = new(headers.Email, Cv.About.Email),
            GitHubLink = new(headers.GitHubLink, Cv.About.GitHubLink),
            LinkedInLink = new(headers.LinkedInLink, Cv.About.LinkedInLink),
            PersonalStatement = new(headers.PersonalStatement, Cv.About.PersonalStatement),
        };

        return aboutPageData;
    }


    private async Task KeyboardButtonPressed(KeyboardEventArgs e)
    {
        int componentCount = LoadedComponents.Count;
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && componentCount > 0 && LoadedComponents[componentCount-1].MetaData!.Type == typeof(WorkExperience))
        {
            SelectCurrentWorkExperience(e.Code);
        }

        if ((e.Code == "Enter" || e.Code == "NumpadEnter"))
        {
            await ExecuteCommand();
        }

    }

    private void SelectCurrentWorkExperience(string keyboardCode)
    {
        if (keyboardCode == "ArrowRight" && CurrentExperienceIndex < Cv.Experience.Length - 1)
        {
            CurrentExperienceIndex++;
        }
        if (keyboardCode == "ArrowLeft" && CurrentExperienceIndex > 0)
        {
            CurrentExperienceIndex--;
        }
        SelectedComponent.Parameters[nameof(WorkExperience.Data)] = Cv.Experience[CurrentExperienceIndex];
        SelectedComponent.Parameters[nameof(WorkExperience.CurrentIndex)] = CurrentExperienceIndex;
    }

    private async Task ExecuteCommand()
    {
        switch (Command)
        {
            case ClearCommand:
                ClearWindow();
                break;
            case AboutCommand:
            case EducationCommand:
            case HardSkillsCommand:
            case SoftSkillsCommand:
            case HelpCommand:
                LoadComponent(ValidComponentCommands[Command]);
                break;
            case ExperienceCommand:
                LoadExperienceComponent();
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
                LoadErrorComponent();
                break;
        }

        Command = string.Empty;
    }

    private void LoadExperienceComponent()
    {
        CurrentExperienceIndex = 0;
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(WorkExperience),
            Name = "Work Experience",
            Parameters = {
                [nameof(WorkExperience.Data)] = Cv!.Experience![CurrentExperienceIndex],
                [nameof(WorkExperience.TotalExperienceCount)] = Cv.Experience.Length
            }
        };

        LoadedComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }

    private void LoadComponent(string componentName)
    {
        SelectedComponent = Components![componentName];
        LoadedComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }
    
    private void LoadGeneralComponent(string message)
    {
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(General),
            Name = "General",
            Parameters = { [nameof(General.Data)] = message }
        };
        LoadedComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }

    private void LoadErrorComponent()
    {
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(Error),
            Name = "Error",
            Parameters = { [nameof(Error.BadCommand)] = Command }
        };
        LoadedComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
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

        LoadGeneralComponent(commandResult);
    }

    private async Task DownloadCv(Languages language)
    {
        var commandResult = "Result: ";
        try
        {
            var base64 = await FileManager.GetBase64FromPdfCv(Http, language.ToString());
            await JSRuntime.InvokeVoidAsync("downloadFile", $"cv_{language}.pdf", base64);
            commandResult += "Success";
        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        LoadGeneralComponent(commandResult);
    }

    private async Task LoadCv(Languages language)
    {
        var commandResult = "Result: ";
        try
        {
            Cv = await Http.GetFromJsonAsync<CvModel>($"cv-data/cv-{language}.json") ?? new();
            CurrentSelectedLanguage = language;
            InitializeComponentsWithParameters();
            commandResult += "Success";

        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        LoadGeneralComponent(commandResult);
    }

    private void ClearWindow()
    {
        LoadedComponents = [];
    }
}