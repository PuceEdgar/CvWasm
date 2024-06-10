using AutoFixture;
using CvWasm.Factory;
using CvWasm.Managers;
using CvWasm.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CvWasm.Tests;
public class CommandServiceTests
{
    private readonly IComponentRepository _componentRepository = Substitute.For<IComponentRepository>();
    private readonly IJsService _jsService = Substitute.For<IJsService>();
    private readonly IFileService _fileManager = Substitute.For<IFileService>();
    private readonly Fixture _fixture = new();
    private readonly CommandService _sut;
    private readonly SharedMethods _sharedMethods;

    public CommandServiceTests()
    {
        _sut = new CommandService(_componentRepository, _jsService, _fileManager);
        StateContainer.LoadedCvs[Languages.eng] = _fixture.Create<CvModel>();
        StateContainer.LoadedCvs[Languages.kor] = _fixture.Create<CvModel>();
        _sharedMethods = new(_componentRepository);
    }

    [Fact]
    public async Task ExecuteCommand_WhenClearCommandPassed_ExecutesClearWindow()
    {
        //Act
        await _sut.ExecuteCommand(ClearCommand);

        //Arrange
        _componentRepository.Received().ClearWindow();
    }

    [Theory]
    [InlineData(AboutCommand)]
    [InlineData(EducationCommand)]
    [InlineData(HardSkillsCommand)]
    [InlineData(SoftSkillsCommand)]
    [InlineData(ExperienceCommand)]
    [InlineData(HelpCommand)]
    public async Task ExecuteCommand_WhenComponentCommandPassed_ExecutesAddComponent(string command)
    {
        //Arrange
        var component = _sharedMethods.MockComponentCreation(command);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        AssertCreateComponentAndAddComponentToListWereCalled(command, component);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public async Task ExecuteCommand_WhenBadCommandPassed_ExecutesAddComponent(Languages language)
    {
        //Arrange
        var command = "abc";
        StateContainer.CurrentSelectedLanguage = language;
        var message = ErrorService.GenerateBadCommandErrorMessage(command, language);
        var component = _sharedMethods.MockComponentCreation(command, message);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    [Theory]
    [InlineData(OpenGitHubCommand, GithubLink, ResultSuccess)]
    [InlineData(OpenLinkedInCommand, LinkedInLink, ResultSuccess)]
    public async Task ExecuteCommand_WhenOpenUrlCommandPassed_ExecutesOpenLinkInNewTab(string command, string url, string message)
    {
        //Arrange
        var component = _sharedMethods.MockComponentCreation(command, message);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _jsService.Received().CallJsFunctionToOpenUrl(url);
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    [Theory]
    [InlineData(OpenGitHubCommand, GithubLink, ResultFailed)]
    [InlineData(OpenLinkedInCommand, LinkedInLink, ResultFailed)]
    public async Task ExecuteCommand_WhenOpenUrlCommandCalled_JsFailsAndResultComponentShowsResultFailed(string command, string url, string message)
    {
        //Arrange
        var component = _sharedMethods.MockComponentCreation(command, message);
        _jsService.CallJsFunctionToOpenUrl(url).ThrowsAsync<Exception>();

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _jsService.Received().CallJsFunctionToOpenUrl(url);
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    [Theory]
    [InlineData(DownloadEngCvCommand, Languages.eng, ResultSuccess)]
    [InlineData(DownloadKorCvCommand, Languages.kor, ResultSuccess)]
    public async Task ExecuteCommand_WhenDownloadCvCommandPassed_ExecutesDownloadCv(string command, Languages language, string message)
    {
        //Arrange
        var base64 = "some string";
        _fileManager.GetBase64FromPdfCv(language).Returns(base64);
        var component = _sharedMethods.MockComponentCreation(command, message);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _fileManager.Received().GetBase64FromPdfCv(language);
        await _jsService.Received().CallJsFunctionToDownloadCv(language, base64);
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    [Theory]
    [InlineData(DownloadEngCvCommand, Languages.eng, ResultFailed)]
    [InlineData(DownloadKorCvCommand, Languages.kor, ResultFailed)]
    public async Task ExecuteCommand_WhenDownloadCvCommandPassed_JsFailsAndResultComponentShowsResultFailed(string command, Languages language, string message)
    {
        //Arrange
        var base64 = "some string";
        _fileManager.GetBase64FromPdfCv(language).Returns(base64);
        _jsService.CallJsFunctionToDownloadCv(language, base64).ThrowsAsync<Exception>();
        var component = _sharedMethods.MockComponentCreation(command, message);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _fileManager.Received().GetBase64FromPdfCv(language);
        await _jsService.Received().CallJsFunctionToDownloadCv(language, base64);
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    [Theory]
    [InlineData(DownloadEngCvCommand, Languages.eng, ResultFailed)]
    [InlineData(DownloadKorCvCommand, Languages.kor, ResultFailed)]
    public async Task ExecuteCommand_WhenDownloadCvCommandPassedAndBase64StringIsEmpty_ExecutesDownloadCvWithFailedResult(string command, Languages language, string message)
    {
        //Arrange
        var base64 = string.Empty;
       _fileManager.GetBase64FromPdfCv(language).Returns(base64);
        var component = _sharedMethods.MockComponentCreation(command, message);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _fileManager.Received().GetBase64FromPdfCv(language);
        await _jsService.DidNotReceive().CallJsFunctionToDownloadCv(language, base64);
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    [Theory]
    [InlineData(ChangeLanguageToEnglishCommand, Languages.eng, ResultSuccess)]
    [InlineData(ChangeLanguageToKoreanCommand, Languages.kor, ResultSuccess)]
    public async Task ExecuteCommand_WhenChangeLanguageCommandPassed_ExecutesChangeLanguage(string command, Languages language, string message)
    {
        //Asert
        var component = _sharedMethods.MockComponentCreation(command, message);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        Assert.Equal(StateContainer.CurrentSelectedLanguage, language);
        AssertCreateComponentAndAddComponentToListWereCalled(command, component, message);
    }

    private void AssertCreateComponentAndAddComponentToListWereCalled(string command, BaseComponent component, string? message = null)
    {
        _componentRepository.Received().CreateNewComponent(command, message);
        _componentRepository.Received().AddComponentToList(component);
    }

    
}
