using AutoFixture;
using CvWasm.Factory;
using CvWasm.Models;
using CvWasm.Pages;

namespace CvWasm.Tests;
public class ComponentFactoryTests
{
    private readonly Fixture _fixture = new();
    private readonly string _command = "abc";

    public ComponentFactoryTests()
    {
        StateContainer.LoadedCvs[Languages.eng] = _fixture.Create<CvModel>();
        StateContainer.LoadedCvs[Languages.kor] = _fixture.Create<CvModel>();
        StateContainer.CommandDescriptions[Languages.eng] = _fixture.Create<Dictionary<string, string>[]>();
        StateContainer.CommandDescriptions[Languages.kor] = _fixture.Create<Dictionary<string, string>[]>();
    }

    [Theory]
    [InlineData(Constants.AboutCommand, typeof(AboutComponent))]
    [InlineData(Constants.EducationCommand, typeof(EducationComponent))]
    [InlineData(Constants.HardSkillsCommand, typeof(HardSkillsComponent))]
    [InlineData(Constants.SoftSkillsCommand, typeof(SoftSkillsComponent))]
    [InlineData(Constants.ExperienceCommand, typeof(WorkExperienceComponent))]
    [InlineData(Constants.HelpCommand, typeof(HelpComponent))]
    [InlineData("abc", typeof(ResultComponent))]
    public void CreateComponent_CreatesCorrectComponentType(string command, Type componentType)
    {
        //Act
        var component = ComponentFactory.CreateComponent(command);

        //Assert
        Assert.NotNull(component);
        Assert.Equal(componentType, component.GetType());
    }

    [Theory]
    [InlineData(Constants.CvLoadFailed)]
    [InlineData(Constants.AsciiArtLoadFailed)]
    [InlineData(Constants.CommandDescriptionLoadFailed)]
    [InlineData(Constants.CvDownloadFailed)]
    public void CreateComponent_WhenErrorMessagePassed_DisplaysThatMessageInResultComponent(string errorMessage)
    {
        //Act
        var component = ComponentFactory.CreateComponent(_command, errorMessage);

        //Assert
        Assert.NotNull(component);
        Assert.Equal(errorMessage, component.Parameters[nameof(CommandResult.Result)]);
    }
}
