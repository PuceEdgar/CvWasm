using Microsoft.AspNetCore.Components;

namespace CvWasm.SharedComponents;

public partial class TextArray
{
    [Parameter]
    public string[] TextCollection { get; set; } = [];
}