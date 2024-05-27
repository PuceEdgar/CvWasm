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

        if (Cv is not null) 
        {
            InitializeComponentsWithParameters();
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
                Parameters = { [nameof(HardSkills.HardSkillsDetails)] = GetHardSkillsPageDataFromCv() }
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
                Parameters = { [nameof(Education.EducationDetails)] = GetEducationPageDataFromCv() }
            },
            [nameof(Help)] = new ComponentMetadata()
            {
                Type = typeof(Help),
                Name = "Help",
                Parameters = { [nameof(Help.CurrentSelectedLanguage)] = CurrentSelectedLanguage.ToString(), [nameof(Help.CommandDescriptions)] = CommandDescription }
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

    private EducationPageData GetEducationPageDataFromCv() 
    {
        IHeaders headers = CurrentSelectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        EducationPageData educationPageData = new()
        {
            UniversityName = new(headers.UniversityName, Cv.Education.UniversityName),
            Location = new(headers.Location, Cv.Education.Location),
            PeriodAttended = new(headers.PeriodAttended,  Cv.Education.PeriodAttended),
            Degree = new(headers.Degree, Cv.Education.Degree),
        };

        return educationPageData;
    }

    private HardSkillsPageData GetHardSkillsPageDataFromCv() 
    {
        IHeaders headers = CurrentSelectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        HardSkillsPageData hardSkillsPageData = new()
        {
            Programming = new(headers.Programming, Cv.Skills.HardSkills.Programming),
            Tools = new(headers.Tools, Cv.Skills.HardSkills.Tools),
            Other = new(headers.Other, Cv.Skills.HardSkills.Other),
            WayOfWorking = new(headers.WayOfWorking, Cv.Skills.HardSkills.WayOfWorking),
            Languages = new(headers.Languages, Cv.Skills.HardSkills.Languages)
        };

        return hardSkillsPageData;
    }

    private WorkExperiencePageData GetWorkExperiencePageDataFromCv(int experienceIndex) 
    {
        IHeaders headers = CurrentSelectedLanguage == Languages.eng ? EnglishHeaders : KoreanHeaders;
        var selectedExperience = Cv.Experience[experienceIndex];
        WorkExperiencePageData workExperiencePageData = new()
        {
            TimePeriod = new(headers.TimePeriod, selectedExperience.TimePeriod),
            Company = new(headers.Company, selectedExperience.Company),
            Location = new(headers.Location, selectedExperience.Location),
            Position = new(headers.Position, selectedExperience.Position),
            JobDescription = new(headers.JobDescription, selectedExperience.JobDescription)
        };

        return workExperiencePageData;
    }

    private async Task KeyboardButtonPressed(KeyboardEventArgs e)
    {
        int componentCount = LoadedComponents.Count;
        if ((e.Code == "ArrowLeft" || e.Code == "ArrowRight") && componentCount > 0 && LoadedComponents[componentCount-1].MetaData!.Type == typeof(WorkExperience))
        {
            SelectCurrentWorkExperience(e.Code);
        }

        if (e.Code.Equals("Enter", StringComparison.InvariantCultureIgnoreCase)  
            || e.Code.Equals("NumpadEnter", StringComparison.InvariantCultureIgnoreCase) 
                || e.Key.Equals("Enter", StringComparison.InvariantCultureIgnoreCase))
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
        SelectedComponent.Parameters[nameof(WorkExperience.ExperienceDetails)] = GetWorkExperiencePageDataFromCv(CurrentExperienceIndex);
        SelectedComponent.Parameters[nameof(WorkExperience.CurrentIndex)] = CurrentExperienceIndex;
    }

    private async Task ExecuteCommand()
    {
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
            case HelpCommand:
                LoadComponent(ValidComponentCommands[lowerCaseCommand]);
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
                LoadErrorComponentWithMessage(ErrorManager.GenerateBadCommandErrorMessage(Command, CurrentSelectedLanguage));
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
                [nameof(WorkExperience.ExperienceDetails)] = GetWorkExperiencePageDataFromCv(CurrentExperienceIndex),
                [nameof(WorkExperience.TotalExperienceCount)] = Cv.Experience.Length,
                [nameof(WorkExperience.CurrentSelectedLanguage)] = CurrentSelectedLanguage
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
    
    private void LoadCommandResultComponent(string message)
    {
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(CommandResult),
            Name = "Command Result",
            Parameters = { [nameof(CommandResult.Result)] = message }
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

        LoadCommandResultComponent(commandResult);
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

        LoadCommandResultComponent(commandResult);
    }

    private async Task LoadCv(Languages language)
    {
        var commandResult = "Result: ";
        try
        {
            Cv = await Http.GetFromJsonAsync<CvModel>($"cv-data/cv-{language}.json");
            CurrentSelectedLanguage = language;
            InitializeComponentsWithParameters();
            commandResult += "Success";

        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        LoadCommandResultComponent(commandResult);
    }

    private void ClearWindow()
    {
        LoadedComponents = [];
    }


    private async Task LoadCvDataFromJson()
    {
        try
        {
            Cv = await FileManager.LoadDataFromJson<CvModel>(Http, $"cv-data/cv-{CurrentSelectedLanguage}.json");
        }
        catch (Exception)
        {
            LoadErrorComponentWithMessage(ErrorManager.FailedToLoadCvMessage);
        }
    }    

    private async Task LoadCommandDescriptionFromJson()
    {
        try
        {
            CommandDescription = await FileManager.LoadDataFromJson<Dictionary<string, string[]>>(Http, CommandDescriptionPath);
        }
        catch (Exception)
        {
            LoadErrorComponentWithMessage(ErrorManager.FailedToLoadCommandDescriptionMessage);
        }
    }

    private async Task LoadAsciiArtFromFile()
    {
        try
        {
            AsciiArt = await FileManager.LoadDataAsString(Http, AsciiArtPath);
        }
        catch (Exception)
        {
            LoadErrorComponentWithMessage(ErrorManager.FailedToLoadAsciiArtMessage);
        }
    }

    private void LoadErrorComponentWithMessage(string errorMessage)
    {
        SelectedComponent = new ComponentMetadata()
        {
            Type = typeof(Error),
            Name = "Error",
            Parameters = {
                [nameof(Error.ErrorMessage)] = errorMessage,
            }
        };
        LoadedComponents.Add(new()
        {
            Command = Command,
            MetaData = SelectedComponent
        });
    }
}