using AutoFixture;
using CvWasm.Headers;
using CvWasm.Models;

namespace CvWasm.Tests;
public class PageDataLoaderTests
{
    private readonly Fixture _fixture = new();
    private readonly EnglishHeaders _englishHeaders = new();
    private readonly KoreanHeaders _koreanHeaders = new();

    public PageDataLoaderTests()
    {
        StateContainer.LoadedCvs[Languages.eng] = _fixture.Create<CvModel>();
        StateContainer.LoadedCvs[Languages.kor] = _fixture.Create<CvModel>();
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public void GetAboutPageDataFromCv_WhenCalled_ReturnsAboutPageData(Languages currentLanguage)
    {
        //Arrange
        var headers = GetHeaderBasedOnSelectedLanguage(currentLanguage);
        var expected = StateContainer.LoadedCvs[StateContainer.CurrentSelectedLanguage].About;

        //Act
        var actual = PageDataLoader.GetAboutPageDataFromCv();

        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expected!.FullName, actual.FullName.Value);
        Assert.Equal(headers.FullName, actual.FullName.Key);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public void GetEducationPageDataFromCv_WhenCalled_ReturnsEducationPageData(Languages currentLanguage)
    {
        //Arrange
        var headers = GetHeaderBasedOnSelectedLanguage(currentLanguage);
        var expected = StateContainer.LoadedCvs[StateContainer.CurrentSelectedLanguage].Education;       

        //Act
        var actual = PageDataLoader.GetEducationPageDataFromCv();

        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expected!.UniversityName, actual.UniversityName.Value);
        Assert.Equal(headers.UniversityName, actual.UniversityName.Key);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public void GetHardSkillsPageDataFromCv_WhenCalled_ReturnsHardSkillsPageData(Languages currentLanguage)
    {
        //Arrange
        var headers = GetHeaderBasedOnSelectedLanguage(currentLanguage);
        var expected = StateContainer.LoadedCvs[StateContainer.CurrentSelectedLanguage].Skills!.HardSkills;

        //Act
        var actual = PageDataLoader.GetHardSkillsPageDataFromCv();

        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expected!.Programming, actual.Programming.Value);
        Assert.Equal(headers.Programming, actual.Programming.Key);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public void GetWorkExperiencePageDataFromCv_WhenCalled_ReturnsWorkExperiencePageData(Languages currentLanguage)
    {
        //Arrange
        var headers = GetHeaderBasedOnSelectedLanguage(currentLanguage);
        var expected = StateContainer.LoadedCvs[StateContainer.CurrentSelectedLanguage].Experience;

        //Act
        var actual = PageDataLoader.GetWorkExperiencePageDataFromCv();

        //Assert
        Assert.NotNull(actual);
        Assert.True(expected!.Length == actual.Count);
        Assert.Equal(expected[0].Company, actual.First().Company.Value);
        Assert.Equal(headers.Company, actual.First().Company.Key);        
    }

    private IHeaders GetHeaderBasedOnSelectedLanguage(Languages currentLanguage)
    {
        StateContainer.CurrentSelectedLanguage = currentLanguage;
        return currentLanguage == Languages.eng ? _englishHeaders : _koreanHeaders;
    }
}
