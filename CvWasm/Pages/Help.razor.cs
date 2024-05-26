using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class Help
{
    [Parameter]
    public string? CurrentSelectedLanguage { get; set; }

    [Parameter]
    public Dictionary<string, string[]>? CommandDescriptions { get; set; }
}