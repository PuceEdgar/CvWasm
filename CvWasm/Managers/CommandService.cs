namespace CvWasm.Managers;

public class CommandService : ICommandService
{
    private readonly IComponentManager _componentManager;
    private readonly IJsService _jsService;
    private readonly IFileManager _fileManager;
    private readonly StateContainer _stateContainer;

    public CommandService(IComponentManager componentManager, IJsService jsService, IFileManager fileManager, StateContainer stateContainer)
    {
        _componentManager = componentManager;
        _jsService = jsService;
        _fileManager = fileManager;
        _stateContainer = stateContainer;
    }

    public async Task ExecuteCommand(string command)
    {
        switch (command)
        {
            case ClearCommand:
                _componentManager.ClearWindow();
                break;
            case AboutCommand:
            case EducationCommand:
            case HardSkillsCommand:
            case SoftSkillsCommand:
            case ExperienceCommand:
            case HelpCommand:
                AddNewComponentForCommand(command);
                break;
            case OpenGitHubCommand:
                await OpenLinkInNewTab(GithubLink, command);
                break;
            case OpenLinkedInCommand:
                await OpenLinkInNewTab(LinkedInLink, command);
                break;
            case DownloadEngCvCommand:
                await DownloadCv(Languages.eng, command);
                break;
            case DownloadKorCvCommand:
                await DownloadCv(Languages.kor, command);
                break;
            case ShowEnglishCommand:
                await LoadCv(Languages.eng, command);
                break;
            case ShowKoreanCommand:
                await LoadCv(Languages.kor, command);
                break;
            default:
                LoadResultComponentForError(command);
                break;
        }
    }

    private void AddNewComponentForCommand(string command)
    {
        var component = _componentManager.GetExistingComponent(command);
        _componentManager.AddComponentToLoadedComponentList(component);
    }

    private void LoadResultComponentForError(string command)
    {
        var component = _componentManager.CreateResultComponent(ErrorManager.GenerateBadCommandErrorMessage(command, _stateContainer.CurrentSelectedLanguage), command);
        _componentManager.AddComponentToLoadedComponentList(component);
    }

    private async Task OpenLinkInNewTab(string url, string command)
    {
        var commandResult = "Result: ";
        try
        {
            await _jsService.CallJsFunctionToOpenUrl(url);
            commandResult += "Success";
        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        var component = _componentManager.CreateResultComponent(commandResult, command);
        _componentManager.AddComponentToLoadedComponentList(component);
    }

    private async Task DownloadCv(Languages language, string downloadCvCommand)
    {
        var commandResult = "Result: ";
        try
        {
            var base64 = await _fileManager.GetBase64FromPdfCv(language.ToString());
            await _jsService.CallJsFunctionToDownloadCv(language, base64);
            commandResult += "Success";
        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        var result = _componentManager.CreateResultComponent(commandResult, downloadCvCommand);
        _componentManager.AddComponentToLoadedComponentList(result);
    }

    private async Task LoadCv(Languages language, string command)
    {
        var commandResult = "Result: ";
        try
        {
            _stateContainer.CurrentSelectedLanguage = language;
            await _fileManager.LoadCvDataFromJson();
            _componentManager.InitializeComponentsWithParameters(_stateContainer.LoadedCvs[language], language, _stateContainer.CommandDescriptions[language]);
            commandResult += "Success";
        }
        catch (Exception)
        {
            commandResult += "Failed";
        }

        var result = _componentManager.CreateResultComponent(commandResult, command);
        _componentManager.AddComponentToLoadedComponentList(result);
    }
}
