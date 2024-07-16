namespace CvWasm;

public static class Constants
{
    public const string AboutCommand = "about";
    public const string ExperienceCommand = "experience";
    public const string HardSkillsCommand = "skills -h";
    public const string SoftSkillsCommand = "skills -s";
    public const string EducationCommand = "education";
    public const string PortfolioCommand = "portfolio";
    public const string HelpCommand = "help";
    public const string ClearCommand = "clear";
    public const string OpenGitHubCommand = "open -g";
    public const string OpenLinkedInCommand = "open -l";
    public const string DownloadEngCvCommand = "cv -d -eng";
    public const string DownloadKorCvCommand = "cv -d -kor";
    public const string ChangeLanguageToEnglishCommand = "lang -eng";
    public const string ChangeLanguageToKoreanCommand = "lang -kor";

    //public const string EnglishCvJsonPath = "cv-data/cv-eng.json";
    //public const string KoreanCvJsonPath = "cv-data/cv-kor.json";
    //public const string EnglishCvPdfPath = "cv-data/Edgars_Puce_eng.pdf";
    //public const string KoreanCvPdfPath = "cv-data/Edgars_Puce_kor.pdf";
    public const string CommandDescriptionPath = "file-data/CommandDescription.json";
    public const string AsciiArtPath = "file-data/ascii-welcome.txt";

    public const string GithubLink = "https://github.com/PuceEdgar";
    public const string LinkedInLink = "https://www.linkedin.com/in/edgars-puce/";

    public const string ResultSuccess = "Result: Success!";
    public const string ResultFailed = "Result: Failed!";

    public const string CvLoadFailed = "Sorry, failed to load cv data!";
    public const string CommandDescriptionLoadFailed = "Sorry, failed to load command description data!";
    public const string AsciiArtLoadFailed = "Sorry, failed to load ascii art from file!";
    public const string CvDownloadFailed = "Sorry, failed to download CV!";
}
