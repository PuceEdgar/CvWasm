using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class Help
{
    [Parameter]
    public Dictionary<string, string>[]? CommandDescriptions { get; set; }
}
