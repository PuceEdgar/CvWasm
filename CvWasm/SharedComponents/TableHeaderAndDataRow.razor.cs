using Microsoft.AspNetCore.Components;

namespace CvWasm.SharedComponents;

public partial class TableHeaderAndDataRow
{
    [Parameter]
    public string TableHeader { get; set; }
    [Parameter]
    public string SimpleData { get; set; }
    [Parameter]
    public string[] CollectionData { get; set; }
}