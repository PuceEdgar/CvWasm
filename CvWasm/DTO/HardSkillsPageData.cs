namespace CvWasm.DTO;

public record HardSkillsPageData
{
    public KeyValuePair<string, string[]> Programming { get; set; }
    public KeyValuePair<string, string> Tools { get; set; }
    public KeyValuePair<string, string[]> Other { get; set; }
    public KeyValuePair<string, string> WayOfWorking { get; set; }
    public KeyValuePair<string, string[]> Languages { get; set; }
}
