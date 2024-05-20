using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace CvWasm.Pages;
public partial class Home
{
    private CvModel? cv;
    private Dictionary<string, ComponentMetadata>? components;
    private ComponentMetadata? selectedComponent;
    private ElementReference textInput;
    private string Command = string.Empty;
    private int currentExperienceIndex = 0;
    private List<CommandAndData> listOfComponents = [];
    private string? AsciiArt;
    private bool isEngVersion = true;

    protected override async Task OnInitializedAsync()
    {
        cv = await Http.GetFromJsonAsync<CvModel>("cv-data/cv-eng.json") ?? new();
        AsciiArt = await Http.GetStringAsync("cv-data/ascii-welcome.txt");
        InitializeComponentsWithParameters(cv);
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
        await textInput.FocusAsync();
    }

    //TODO:
    //1. implement back functionality. probably use list of navigated pages/commands and on arrow up or command back load previos command and maybe on arrow down/next show next if exists
    //2. change from input field to form? so that 'Enter' method is not called on each input change. Or change when 'Enter' is called

    private void InitializeComponentsWithParameters(CvModel cv)
    {
        components = new(StringComparer.OrdinalIgnoreCase)
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
                Parameters = { [nameof(Help.IsEnglishVersion)] = isEngVersion }
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
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && listOfComponents.Last().MetaData.Type == typeof(WorkExperience))
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
        if (code == "ArrowRight" && currentExperienceIndex < cv.Experience.Length - 1)
        {
            currentExperienceIndex++;
        }
        if (code == "ArrowLeft" && currentExperienceIndex > 0)
        {
            currentExperienceIndex--;
        }
        selectedComponent.Parameters[nameof(WorkExperience.Data)] = cv.Experience[currentExperienceIndex];
        selectedComponent.Parameters[nameof(WorkExperience.CurrentIndex)] = currentExperienceIndex;
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
                await OpenLinkInNewTab(cv.About.GitHubLink);
                break;
            case OpenLinkedInCommand:
                await OpenLinkInNewTab(cv.About.LinkedInLink);
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
        selectedComponent = new ComponentMetadata()
        {
            Type = typeof(WorkExperience),
            Name = "Work Experience",
            Parameters = {
                [nameof(WorkExperience.Data)] = cv!.Experience![0],
            [nameof(WorkExperience.TotalExperienceCount)] = cv.Experience.Count()
            }
        };


        listOfComponents.Add(new()
        {
            Command = Command,
            MetaData = selectedComponent
        });
    }

    private void LoadComponent(string componentName)
    {
        selectedComponent = components![componentName];
        listOfComponents.Add(new()
        {
            Command = Command,
            MetaData = selectedComponent
        });
    }

    private void LoadErrorComponent()
    {
        selectedComponent = new ComponentMetadata()
        {
            Type = typeof(Error),
            Name = "Error",
            Parameters = { [nameof(Error.BadCommand)] = Command }
        };
        listOfComponents.Add(new()
        {
            Command = Command,
            MetaData = selectedComponent
        });
    }

    private void ClearWindow()
    {
        listOfComponents = [];
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
            cv = await Http.GetFromJsonAsync<CvModel>($"cv-data/cv-{language.ToString()}.json") ?? new();
            isEngVersion = language == Languages.eng;
            InitializeComponentsWithParameters(cv);
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
        selectedComponent = new ComponentMetadata()
        {
            Type = typeof(General),
            Name = "General",
            Parameters = { [nameof(General.Data)] = message }
        };
        listOfComponents.Add(new()
        {
            Command = Command,
            MetaData = selectedComponent
        });
    }
}