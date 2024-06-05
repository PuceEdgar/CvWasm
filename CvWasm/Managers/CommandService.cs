namespace CvWasm.Managers;

public class CommandService : ICommandService
{
    private readonly IComponentRepository _componentRepository;
    private readonly IJsService _jsService;
    private readonly IFileService _fileManager;

    public CommandService(IComponentRepository componentManager, IJsService jsService, IFileService fileManager)
    {
        _componentRepository = componentManager;
        _jsService = jsService;
        _fileManager = fileManager;
    }

    public async Task ExecuteCommand(string command)
    {
        switch (command)
        {
            case ClearCommand:
                _componentRepository.ClearWindow();
                break;
            case AboutCommand:
            case EducationCommand:
            case HardSkillsCommand:
            case SoftSkillsCommand:
            case ExperienceCommand:
            case HelpCommand:
                AddNewComponent(command);
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
            case ChangeLanguageToEnglishCommand:
                SetLanguageTo(Languages.eng, command);
                break;
            case ChangeLanguageToKoreanCommand:
                SetLanguageTo(Languages.kor, command);
                break;
            default:
                AddNewComponent(command, true);
                break;
        }
    }

    private void AddNewComponent(string command, bool isError = false)
    {
        if (isError)
        {
            _componentRepository.CreateNewComponentAndAddToList(command, ErrorService.GenerateBadCommandErrorMessage(command, StateContainer.CurrentSelectedLanguage));
        }
        else
        {
            _componentRepository.CreateNewComponentAndAddToList(command);
        }
    }

    private async Task OpenLinkInNewTab(string url, string command)
    {
        bool isSuccess = true;
        try
        {
            await _jsService.CallJsFunctionToOpenUrl(url);
        }
        catch (Exception)
        {
            isSuccess = false;
        }

        _componentRepository.CreateNewComponentAndAddToList(command, ErrorService.GetCommandResultMessage(isSuccess));
    }

    private async Task DownloadCv(Languages language, string command)
    {
        bool isSuccess = true;
        try
        {
            var base64 = await _fileManager.GetBase64FromPdfCv(language.ToString());
            if (string.IsNullOrWhiteSpace(base64)) 
            {
                isSuccess = false;
                
            } 
            else
            {
                await _jsService.CallJsFunctionToDownloadCv(language, base64);
            }
            
        }
        catch (Exception)
        {
            isSuccess = false;
        }

        _componentRepository.CreateNewComponentAndAddToList(command, ErrorService.GetCommandResultMessage(isSuccess));
    }

    private void SetLanguageTo(Languages language, string command)
    {
        StateContainer.CurrentSelectedLanguage = language;
        _componentRepository.CreateNewComponentAndAddToList(command, ErrorService.GetCommandResultMessage(true));
    }
}
