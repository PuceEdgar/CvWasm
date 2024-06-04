using CvWasm.Models;

namespace CvWasm;

public static class StateContainer
{
    public static Languages CurrentSelectedLanguage { get; set; } = Languages.eng;
    public static Dictionary<Languages, CvModel> LoadedCvs { get; set; } = [];
    public static Dictionary<Languages, Dictionary<string, string>[]> CommandDescriptions { get; set; } = [];
}
