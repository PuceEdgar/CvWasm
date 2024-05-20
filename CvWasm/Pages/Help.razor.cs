using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class Help
{
    [Parameter]
    public bool IsEnglishVersion { get; set; }

    private static readonly Dictionary<string, string> EnglishCommandDescription = new()
    {
        [AboutCommand] = "Shows about page",
        [ExperienceCommand] = "Shows work experience page",
        [HardSkillsCommand] = "Shows hard skills page",
        [SoftSkillsCommand] = "Shows soft skills page",
        [EducationCommand] = "Shows education page",
        [HelpCommand] = "Shows help page",
        [ClearCommand] = "Clear window",
        [OpenGitHubCommand] = "Opens Github profile in new tab",
        [OpenLinkedInCommand] = "Opens LinkedIn profile in new tab",
        [DownloadEngCvCommand] = "Download English version of CV in PDF format",
        [DownloadKorCvCommand] = "Download Korean version of CV in PDF format",
        [ShowEnglishCommand] = "View content in English",
        [ShowKoreanCommand] = "View content in Korean"
    };

    private static readonly Dictionary<string, string> KoreanCommandDescription = new()
    {
        [AboutCommand] = "About",
        [ExperienceCommand] = "경력사항",
        [HardSkillsCommand] = "Hard skills",
        [SoftSkillsCommand] = "Soft skills",
        [EducationCommand] = "학력사항",
        [HelpCommand] = "도움말",
        [ClearCommand] = "모두 지우기",
        [OpenGitHubCommand] = "Github 바로가기",
        [OpenLinkedInCommand] = "LinkedIn 바로가기",
        [DownloadEngCvCommand] = "영어 버전으로 보기",
        [DownloadKorCvCommand] = "한국어 버전으로 보기",
        [ShowEnglishCommand] = "영어",
        [ShowKoreanCommand] = "한국어"
    };
}