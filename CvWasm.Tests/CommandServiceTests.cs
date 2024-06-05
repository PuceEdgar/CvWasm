using AutoFixture;
using CvWasm.DTO;
using CvWasm.Factory;
using CvWasm.Managers;
using CvWasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvWasm.Tests;
public class CommandServiceTests
{
    private readonly IComponentRepository _componentRepository = Substitute.For<IComponentRepository>();
    private readonly IJsService _jsService = Substitute.For<IJsService>();
    private readonly IFileService _fileManager = Substitute.For<IFileService>();
    private readonly Fixture _fixture = new();
    private readonly CommandService _sut;

    public CommandServiceTests()
    {
        _sut = new CommandService(_componentRepository, _jsService, _fileManager);
        StateContainer.LoadedCvs[Languages.eng] = _fixture.Create<CvModel>();
        StateContainer.LoadedCvs[Languages.kor] = _fixture.Create<CvModel>();
    }

    [Fact]
    public async Task ExecuteCommand_WhenClearCommandPassed_ExecutesClearWindow()
    {
        //Act
        await _sut.ExecuteCommand(Constants.ClearCommand);

        //Arrange
        _componentRepository.Received().ClearWindow();
    }

    [Theory]
    [InlineData(Constants.AboutCommand)]
    [InlineData(Constants.EducationCommand)]
    [InlineData(Constants.HardSkillsCommand)]
    [InlineData(Constants.SoftSkillsCommand)]
    [InlineData(Constants.ExperienceCommand)]
    [InlineData(Constants.HelpCommand)]
    public async Task ExecuteCommand_WhenComponentCommandPassed_ExecutesAddComponent(string command)
    {
        //Arrange
        var component = ComponentFactory.CreateComponent(command);
        _componentRepository.CreateNewComponent(command).Returns(component);
        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        _componentRepository.Received().CreateNewComponent(command);
        _componentRepository.Received().AddComponentToList(component);
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
        var component = ComponentFactory.CreateComponent(command, message);
        _componentRepository.CreateNewComponent(command, message).Returns(component);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        _componentRepository.Received().CreateNewComponent(command, message);
        _componentRepository.Received().AddComponentToList(component);
    }

    [Theory]
    [InlineData(Constants.OpenGitHubCommand, Constants.GithubLink)]
    [InlineData(Constants.OpenLinkedInCommand, Constants.LinkedInLink)]
    public async Task ExecuteCommand_WhenOpenUrlCommandPassed_ExecutesOpenLinkInNewTab(string command, string url)
    {
        //Arrange
        var component = ComponentFactory.CreateComponent(command, Constants.ResultSuccess);
        _componentRepository.CreateNewComponent(command, Constants.ResultSuccess).Returns(component);
        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _jsService.Received().CallJsFunctionToOpenUrl(url);
        _componentRepository.Received().CreateNewComponent(command, Constants.ResultSuccess);
        _componentRepository.Received().AddComponentToList(component);
    }

    [Theory]
    [InlineData(Constants.OpenGitHubCommand, Constants.GithubLink)]
    [InlineData(Constants.OpenLinkedInCommand, Constants.LinkedInLink)]
    public async Task ExecuteCommand_WhenOpenUrlCommandCalled_JsFailsAndResultComponentShowsResultFailed(string command, string url)
    {
        //Arrange
        //Arrange
        var component = ComponentFactory.CreateComponent(command, Constants.ResultFailed);
        _componentRepository.CreateNewComponent(command, Constants.ResultFailed).Returns(component);
        _jsService.CallJsFunctionToOpenUrl(url).ThrowsAsync<Exception>();

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _jsService.Received().CallJsFunctionToOpenUrl(url);
        _componentRepository.Received().CreateNewComponent(command, Constants.ResultFailed);
        _componentRepository.Received().AddComponentToList(component);

    }

    [Theory]
    [InlineData(Constants.DownloadEngCvCommand, Languages.eng)]
    [InlineData(Constants.DownloadKorCvCommand, Languages.kor)]
    public async Task ExecuteCommand_WhenDownloadCvCommandPassed_ExecutesDownloadCv(string command, Languages language)
    {
        //Arrange
        var base64 = "some string";
        _fileManager.GetBase64FromPdfCv(language.ToString()).Returns(base64);
        var component = ComponentFactory.CreateComponent(command, Constants.ResultSuccess);
        _componentRepository.CreateNewComponent(command, Constants.ResultSuccess).Returns(component);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _fileManager.Received().GetBase64FromPdfCv(language.ToString());
        await _jsService.Received().CallJsFunctionToDownloadCv(language, base64);
        _componentRepository.Received().CreateNewComponent(command, Constants.ResultSuccess);
        _componentRepository.Received().AddComponentToList(component);
    }

    [Theory]
    [InlineData(Constants.DownloadEngCvCommand, Languages.eng)]
    [InlineData(Constants.DownloadKorCvCommand, Languages.kor)]
    public async Task ExecuteCommand_WhenDownloadCvCommandPassed_JsFailsAndResultComponentShowsResultFailed(string command, Languages language)
    {
        //Arrange
        var base64 = "some string";
        _fileManager.GetBase64FromPdfCv(language.ToString()).Returns(base64);
        _jsService.CallJsFunctionToDownloadCv(language, base64).ThrowsAsync<Exception>();
        var component = ComponentFactory.CreateComponent(command, Constants.ResultFailed);
        _componentRepository.CreateNewComponent(command, Constants.ResultFailed).Returns(component);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _fileManager.Received().GetBase64FromPdfCv(language.ToString());
        await _jsService.Received().CallJsFunctionToDownloadCv(language, base64);
        _componentRepository.Received().CreateNewComponent(command, Constants.ResultFailed);
        _componentRepository.Received().AddComponentToList(component);
    }

    [Theory]
    [InlineData(Constants.DownloadEngCvCommand, Languages.eng)]
    [InlineData(Constants.DownloadKorCvCommand, Languages.kor)]
    public async Task ExecuteCommand_WhenDownloadCvCommandPassedAndBase64StringIsEmpty_ExecutesDownloadCvWithFailedResult(string command, Languages language)
    {
        //Arrange
        var base64 = string.Empty;
       _fileManager.GetBase64FromPdfCv(language.ToString()).Returns(base64);
        var component = ComponentFactory.CreateComponent(command, Constants.ResultFailed);
        _componentRepository.CreateNewComponent(command, Constants.ResultFailed).Returns(component);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        await _fileManager.Received().GetBase64FromPdfCv(language.ToString());
        await _jsService.DidNotReceive().CallJsFunctionToDownloadCv(language, base64);
        _componentRepository.Received().CreateNewComponent(command, Constants.ResultFailed);
        _componentRepository.Received().AddComponentToList(component);
    }

    [Theory]
    [InlineData(Constants.ChangeLanguageToEnglishCommand, Languages.eng)]
    [InlineData(Constants.ChangeLanguageToKoreanCommand, Languages.kor)]
    public async Task ExecuteCommand_WhenChangeLanguageCommandPassed_ExecutesChangeLanguage(string command, Languages language)
    {
        //Asert
        var component = ComponentFactory.CreateComponent(command, Constants.ResultSuccess);
        _componentRepository.CreateNewComponent(command, Constants.ResultSuccess).Returns(component);

        //Act
        await _sut.ExecuteCommand(command);

        //Assert
        Assert.Equal(StateContainer.CurrentSelectedLanguage, language);
        _componentRepository.Received().CreateNewComponent(command, Constants.ResultSuccess);
        _componentRepository.Received().AddComponentToList(component);
    }    
}
