using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class Error
{
    [Parameter]
    public string? BadCommand { get; init; }
    [Parameter]
    public Languages CurrentSelectedLanguage { get; init; }
    [Parameter]
    public string? ErrorMessage { get; init; }
}