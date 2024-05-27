using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class CommandResult
{
    [Parameter]
    public string? Result { get; set; }
}