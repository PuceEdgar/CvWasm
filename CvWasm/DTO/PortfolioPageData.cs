namespace CvWasm.DTO;

public record PortfolioPageData
{
    public KeyValuePair<string, string> AppName { get; set; }
    public KeyValuePair<string, string> AppUrl { get; set; }
    public KeyValuePair<string, string> Description { get; set; }
    public KeyValuePair<string, string> Technologies { get; set; }
}
