using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class Error
{
    [Parameter]
    public string? BadCommand { get; set; }
}