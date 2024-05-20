using CvWasm.Models;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class WorkExperience
{
    [Parameter]
    public WorkExperienceModel? Data { get; set; }
    [Parameter]
    public int CurrentIndex { get; set; }
    [Parameter]
    public int TotalExperienceCount { get; set; }
}