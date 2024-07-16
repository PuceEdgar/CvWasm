namespace CvWasm.Managers;

public static class ErrorService
{
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

    public static string GetCommandResultMessage(bool isSuccess)
    {
        return isSuccess ? ResultSuccess : ResultFailed;
    }
}
