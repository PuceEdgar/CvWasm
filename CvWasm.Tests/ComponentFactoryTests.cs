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
    [InlineData(AboutCommand, typeof(AboutComponent))]
    [InlineData(EducationCommand, typeof(EducationComponent))]
    [InlineData(HardSkillsCommand, typeof(HardSkillsComponent))]
    [InlineData(SoftSkillsCommand, typeof(SoftSkillsComponent))]
    [InlineData(ExperienceCommand, typeof(WorkExperienceComponent))]
    [InlineData(HelpCommand, typeof(HelpComponent))]
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
    [InlineData(CvLoadFailed)]
    [InlineData(AsciiArtLoadFailed)]
    [InlineData(CommandDescriptionLoadFailed)]
    [InlineData(CvDownloadFailed)]
    public void CreateComponent_WhenErrorMessagePassed_DisplaysThatMessageInResultComponent(string errorMessage)
    {
        //Act
        var component = ComponentFactory.CreateComponent(_command, errorMessage);

        //Assert
        Assert.NotNull(component);
        Assert.Equal(errorMessage, component.Parameters[nameof(CommandResult.Result)]);
    }
}
