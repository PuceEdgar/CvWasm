namespace CvWasm.Managers;

public static class ErrorService
{
    private readonly static string _cvLoadFailed = "Sorry, failed to load cv data!";
    private readonly static string _commandDescriptionLoadFailed = "Sorry, failed to load command description data!";
    private readonly static string _asciiArtLoadFailed = "Sorry, failed to load ascii art from file!";

    public static string FailedToLoadCvMessage => _cvLoadFailed;

    public static string FailedToLoadCommandDescriptionMessage => _commandDescriptionLoadFailed;

    public static string FailedToLoadAsciiArtMessage => _asciiArtLoadFailed;

    public static string GenerateBadCommandErrorMessage(string command, Languages currentLanguage)
    {
        if (currentLanguage == Languages.eng)
        {
            return $"""Command: <span style="font-weight:bold;"> '{command}' </span> is not recognized. Please use <span style="font-weight:bold;"> '{HelpCommand}' </span> for possible commands.""";
        }
        else
        {
            return $"""명령: <span style="font-weight:bold;">'{command}'</span> 이 존재하지 않습니다. 유효한 명령을 내리기 위해서는 <span style="font-weight:bold;">'{HelpCommand}'</span> 를 사용해 주세요.""";
        }
    }
}
