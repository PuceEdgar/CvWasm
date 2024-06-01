using CvWasm.Factory;

namespace CvWasm.Managers;

public class CommandService : ICommandService
{
    private readonly IComponentManager _componentManager;
    private readonly IJsService _jsService;
    private readonly IFileManager _fileManager;

    public CommandService(IComponentManager componentManager, IJsService jsService, IFileManager fileManager)
    {
        _componentManager = componentManager;
        _jsService = jsService;
        _fileManager = fileManager;
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
                SetLanguageTo(Languages.eng, command);
                break;
            case ShowKoreanCommand:
                SetLanguageTo(Languages.kor, command);
                break;
            default:
                LoadResultComponentForError(command);
                break;
        }
    }

    private void AddNewComponentForCommand(string command)
    {
        var component = _componentManager.CreateNewComponent(command);
        _componentManager.AddComponentToLoadedComponentList(component);
    }

    private void LoadResultComponentForError(string command)
    {
        var component = ComponentFactory.CreateComponent(command, ErrorManager.GenerateBadCommandErrorMessage(command, StateContainer.CurrentSelectedLanguage));
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

        var component = ComponentFactory.CreateComponent(command, commandResult);
        _componentManager.AddComponentToLoadedComponentList(component);
    }

    private async Task DownloadCv(Languages language, string command)
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

        var component = ComponentFactory.CreateComponent(command, commandResult);
        _componentManager.AddComponentToLoadedComponentList(component);
    }

    private void SetLanguageTo(Languages language, string command)
    {
        StateContainer.CurrentSelectedLanguage = language;
        var component = ComponentFactory.CreateComponent(command, "Result: Success");
        _componentManager.AddComponentToLoadedComponentList(component);
    }
}
