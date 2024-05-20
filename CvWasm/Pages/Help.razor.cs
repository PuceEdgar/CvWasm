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
        [ExperienceCommand] = "��»���",
        [HardSkillsCommand] = "Hard skills",
        [SoftSkillsCommand] = "Soft skills",
        [EducationCommand] = "�з»���",
        [HelpCommand] = "����",
        [ClearCommand] = "��� �����",
        [OpenGitHubCommand] = "Github �ٷΰ���",
        [OpenLinkedInCommand] = "LinkedIn �ٷΰ���",
        [DownloadEngCvCommand] = "���� �������� ����",
        [DownloadKorCvCommand] = "�ѱ��� �������� ����",
        [ShowEnglishCommand] = "����",
        [ShowKoreanCommand] = "�ѱ���"
    };
}