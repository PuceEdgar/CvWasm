using CvWasm.Pages;

namespace CvWasm;

public static class Constants
{
    public const string AboutCommand = "about";
    public const string ExperienceCommand = "experience";
    public const string HardSkillsCommand = "skills -h";
    public const string SoftSkillsCommand = "skills -s";
    public const string EducationCommand = "education";
    public const string HelpCommand = "help";
    public const string ClearCommand = "clear";
    public const string OpenGitHubCommand = "open -g";
    public const string OpenLinkedInCommand = "open -l";
    public const string DownloadEngCvCommand = "cv -d -eng";
    public const string DownloadKorCvCommand = "cv -d -kor";
    public const string ShowEnglishCommand = "lang -eng";
    public const string ShowKoreanCommand = "lang -kor";

    public const string NameOfAbout = nameof(About);
    public const string NameOfExperience = nameof(WorkExperience);
    public const string NameOfHardSkills = nameof(HardSkills);
    public const string NameOfSoftSkills = nameof(SoftSkills);
    public const string NameOfEducation = nameof(Education);
    public const string NameOfHelp = nameof(Help);
}
