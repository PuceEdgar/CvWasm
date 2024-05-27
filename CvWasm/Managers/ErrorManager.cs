namespace CvWasm.Managers;

public class ErrorManager : IErrorManager
{
    private readonly string _cvLoadFailed = "Sorry, failed to load cv data!";
    private readonly string _commandDescriptionLoadFailed = "Sorry, failed to load command description data!";
    private readonly string _asciiArtLoadFailed = "Sorry, failed to load ascii art from file!";

    public string FailedToLoadCvMessage => _cvLoadFailed;

    public string FailedToLoadCommandDescriptionMessage => _commandDescriptionLoadFailed;

    public string FailedToLoadAsciiArtMessage => _asciiArtLoadFailed;

    public string GenerateBadCommandErrorMessage(string command, Languages currentLanguage)
    {
        if (currentLanguage == Languages.eng)
        {
            return $"""<p> Command: <span style="font-weight:bold;"> '{command}' </span> is not recognized.Please use <span style="font-weight:bold;"> '{HelpCommand}' </span> for possible commands.</p>""";
        }
        else
        {
            return $"""<p>명령: <span style="font-weight:bold;">'{command}'</span> 이 존재하지 않습니다. 유효한 명령을 내리기 위해서는 <span style="font-weight:bold;">'{HelpCommand}'</span> 를 사용해 주세요.</p>""";
        }
    }
}
