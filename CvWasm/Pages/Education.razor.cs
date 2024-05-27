using CvWasm.DTO;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class Education
{
    [Parameter]
    public EducationPageData? EducationDetails { get; set; }
}