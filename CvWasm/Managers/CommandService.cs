﻿using CvWasm.Factory;

namespace CvWasm.Managers;

public class CommandService : ICommandService
{
    private readonly IComponentRepository _componentRepository;
    private readonly IJsService _jsService;
    private readonly IFileService _fileManager;
    private readonly string[] _componentCommands = [AboutCommand, EducationCommand, HardSkillsCommand, SoftSkillsCommand, ExperienceCommand, HelpCommand];

    public CommandService(IComponentRepository componentManager, IJsService jsService, IFileService fileManager)
    {
        _componentRepository = componentManager;
        _jsService = jsService;
        _fileManager = fileManager;
    }

    public async Task ExecuteCommand(string command)
    {
        if (command.Equals(ClearCommand))
        {
            _componentRepository.ClearWindow();
            return;
        }

        var component = command switch
        {
            var c when _componentCommands.Contains(c) => AddNewComponent(command),
            OpenGitHubCommand => await OpenLinkInNewTab(GithubLink, command),
            OpenLinkedInCommand => await OpenLinkInNewTab(LinkedInLink, command),
            DownloadEngCvCommand => await DownloadCv(Languages.eng, command),
            DownloadKorCvCommand => await DownloadCv(Languages.kor, command),
            ChangeLanguageToEnglishCommand => SetLanguageTo(Languages.eng, command),
            ChangeLanguageToKoreanCommand => SetLanguageTo(Languages.kor, command),
            _ => AddNewComponent(command, true),
        };

        _componentRepository.AddComponentToList(component);
    }

    private BaseComponent AddNewComponent(string command, bool isError = false)
    {
        return isError ? _componentRepository.CreateNewComponent(command, ErrorService.GenerateBadCommandErrorMessage(command, StateContainer.CurrentSelectedLanguage))
            : _componentRepository.CreateNewComponent(command);
    }

    private async Task<BaseComponent> OpenLinkInNewTab(string url, string command)
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

        return _componentRepository.CreateNewComponent(command, ErrorService.GetCommandResultMessage(isSuccess));
    }

    private async Task<BaseComponent> DownloadCv(Languages language, string command)
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

        return _componentRepository.CreateNewComponent(command, ErrorService.GetCommandResultMessage(isSuccess));
    }

    private BaseComponent SetLanguageTo(Languages language, string command)
    {
        StateContainer.CurrentSelectedLanguage = language;
        return _componentRepository.CreateNewComponent(command, ErrorService.GetCommandResultMessage(true));
    }
}
