using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class General
{
    [Parameter]
    public string? Data { get; set; }
}