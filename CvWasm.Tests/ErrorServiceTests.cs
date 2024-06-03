using CvWasm.Managers;

namespace CvWasm.Tests;

public class ErrorServiceTests
{
    private readonly string _command = "abc";

    [Theory]
    [InlineData(true, Constants.ResultSuccess)]
    [InlineData(false, Constants.ResultFailed)]
    public void GetCommandResultMessage_WhenResultIsSuccess_ReturnsSuccessMessage(bool isSuccess, string expected)
    {
        //Act
        var actual = ErrorService.GetCommandResultMessage(isSuccess);
        
        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GenerateBadCommandErrorMessage_WhenSelectedLanguageIsEnglish_DisplaysErrorInEnglish()
    {
        //Arrange
        var selectedLanguage = Languages.eng;
        var expected = $"""Command: <span style="font-weight:bold;"> '{_command}' </span> is not recognized. Please use <span style="font-weight:bold;"> '{Constants.HelpCommand}' </span> for possible commands.""";
        
        //Act
        var actual = ErrorService.GenerateBadCommandErrorMessage(_command, selectedLanguage);
        
        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GenerateBadCommandErrorMessage_WhenSelectedLanguageIsKorean_DisplaysErrorInKorean()
    {
        //Arrange
        var selectedLanguage = Languages.kor;
        var expected = $"""명령: <span style="font-weight:bold;">'{_command}'</span> 이 존재하지 않습니다. 유효한 명령을 내리기 위해서는 <span style="font-weight:bold;">'{Constants.HelpCommand}'</span> 를 사용해 주세요.""";
        
        //Act
        var actual = ErrorService.GenerateBadCommandErrorMessage(_command, selectedLanguage);
       
        //Assert
        Assert.Equal(expected, actual);
    }
}
