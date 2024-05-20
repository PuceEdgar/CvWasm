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
    private string Command = string.Empty;
    private int CurrentExperienceIndex = 0;
    private List<CommandAndData> ListOfComponents = [];
    private string? AsciiArt;
    private string CurrentSelectedLanguage = Languages.eng.ToString();
    private Dictionary<string, string[]> CommandDescription = [];

    protected override async Task OnInitializedAsync()
    {
        Cv = await Http.GetFromJsonAsync<CvModel>("cv-data/cv-eng.json") ?? new();
        AsciiArt = await Http.GetStringAsync("file-data/ascii-welcome.txt");
        CommandDescription = await Http.GetFromJsonAsync<Dictionary<string, string[]>>("file-data/CommandDescription.json");
        InitializeComponentsWithParameters(Cv);
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

    private void InitializeComponentsWithParameters(CvModel cv)
    {
        Components = new(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(About)] = new ComponentMetadata()
            {
                Type = typeof(About),
                Name = "About",
                Parameters = { [nameof(About.Data)] = cv.About! }
            },
            [nameof(HardSkills)] = new ComponentMetadata()
            {
                Type = typeof(HardSkills),
                Name = "Hard Skills",
                Parameters = { [nameof(HardSkills.Data)] = cv.Skills!.HardSkills! }
            },
            [nameof(SoftSkills)] = new ComponentMetadata()
            {
                Type = typeof(SoftSkills),
                Name = "Soft Skills",
                Parameters = { [nameof(SoftSkills.Data)] = cv.Skills.SoftSkills! }
            },
            [nameof(Education)] = new ComponentMetadata()
            {
                Type = typeof(Education),
                Name = "Education",
                Parameters = { [nameof(Education.Data)] = cv.Education! }
            },
            [nameof(Help)] = new ComponentMetadata()
            {
                Type = typeof(Help),
                Name = "Help",
                Parameters = { [nameof(Help.DisplayLanguage)] = CurrentSelectedLanguage, [nameof(Help.CommandDescriptions)] = CommandDescription }
            }
        };
    }

    private readonly Dictionary<string, string> ValidComponentCommands = new()
    {
        [AboutCommand] = nameof(About),
        [ExperienceCommand] = nameof(WorkExperience),
        [HardSkillsCommand] = nameof(HardSkills),
        [SoftSkillsCommand] = nameof(SoftSkills),
        [EducationCommand] = nameof(Education),
        [HelpCommand] = nameof(Help)
    };

    private async Task Enter(KeyboardEventArgs e)
    {
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && ListOfComponents.Last().MetaData.Type == typeof(WorkExperience))
        {
            SelectCurrentWorkExperience(e.Code);
        }

        if ((e.Code == "Enter" || e.Code == "NumpadEnter"))
        {
            await ExecuteCommand();
        }

    }

    private void SelectCurrentWorkExperience(string code)
    {
        if (code == "ArrowRight" && CurrentExperienceIndex < Cv.Experience.Length - 1)
        {
            CurrentExperienceIndex++;
        }
        if (code == "ArrowLeft" && CurrentExperienceIndex > 0)
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
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(WorkExperience),
            Name = "Work Experience",
            Parameters = {
                [nameof(WorkExperience.Data)] = Cv!.Experience![0],
            [nameof(WorkExperience.TotalExperienceCount)] = Cv.Experience.Count()
            }
        };


        ListOfComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }

    private void LoadComponent(string componentName)
    {
        SelectedComponent = Components![componentName];
        ListOfComponents.Add(new()
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
        ListOfComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }

    private void ClearWindow()
    {
        ListOfComponents = [];
    }

    private async Task OpenLinkInNewTab(string url)
    {
        var result = "Result: ";
        try
        {
            await JSRuntime.InvokeVoidAsync("open", url, "_blank");
            result += "Success";
        }
        catch (Exception)
        {
            result += "Failed";
        }

        LoadGeneralComponent(result);
    }

    private async Task DownloadCv(Languages language)
    {
        var result = "Result: ";
        try
        {
            var base64 = await FileUtil.GetPdfCvBase64(Http, language.ToString());
            await JSRuntime.InvokeVoidAsync("downloadFile", $"cv_{language}.pdf", base64);
            result += "Success";
        }
        catch (Exception)
        {
            result += "Failed";
        }

        LoadGeneralComponent(result);
    }

    private async Task LoadCv(Languages language)
    {
        var result = "Result: ";
        try
        {
            Cv = await Http.GetFromJsonAsync<CvModel>($"cv-data/cv-{language}.json") ?? new();
            CurrentSelectedLanguage = language.ToString();
            InitializeComponentsWithParameters(Cv);
            result += "Success";

        }
        catch (Exception)
        {
            result += "Failed";
        }

        LoadGeneralComponent(result);
    }

    private void LoadGeneralComponent(string message)
    {
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(General),
            Name = "General",
            Parameters = { [nameof(General.Data)] = message }
        };
        ListOfComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }
}