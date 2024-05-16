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
    public const string OpenGitHubCommand = "github -o";
    public const string OpenLinkedInCommand = "linkedin -o";
    public const string NavigationCommand = "navigation";
    public const string HomeCommand = "home";

    public const string NameOfAbout = nameof(About);
    public const string NameOfExperience = nameof(WorkExperience);
    public const string NameOfHardSkills = nameof(HardSkills);
    public const string NameOfSoftSkills = nameof(SoftSkills);
    public const string NameOfEducation = nameof(Education);
    public const string NameOfHelp = nameof(Help);
    public const string NameOfHome = nameof(Home);
    //public const string NameOfNavigation = nameof(About);

    public static readonly string[] ArrayOfCommands = [AboutCommand, ExperienceCommand, HardSkillsCommand, SoftSkillsCommand, EducationCommand, HelpCommand, ClearCommand, OpenGitHubCommand, OpenLinkedInCommand, NavigationCommand, HomeCommand];
}
