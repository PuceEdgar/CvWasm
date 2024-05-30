using CvWasm.Models;

namespace CvWasm;

public class StateContainer
{
    public Languages CurrentSelectedLanguage { get; set; } = Languages.eng;
    public Dictionary<Languages, CvModel> LoadedCvs { get; set; } = [];
    public Dictionary<Languages, Dictionary<string, string>[]>? CommandDescriptions { get; set; } = [];
}
