using CvWasm.DTO;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class WorkExperience
{
    [Parameter]
    public WorkExperiencePageData? ExperienceDetails { get; set; }
    [Parameter]
    public int CurrentIndex { get; set; }
    [Parameter]
    public int TotalExperienceCount { get; set; }
    [Parameter]
    public Languages CurrentSelectedLanguage { get; set; }
}