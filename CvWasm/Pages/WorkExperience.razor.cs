using CvWasm.DTO;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class WorkExperience
{
    [Parameter]
    public List<WorkExperiencePageData>? ListOfExperienceDetails { get; set; }

    private WorkExperiencePageData? ExperienceDetails { get; set; }
    private int CurrentIndex { get; set; }
    private int TotalExperienceCount { get; set; }

    public void SelectCurrentWorkExperience(string keyboardCode)
    {
        if (keyboardCode == "ArrowRight" && CurrentIndex < TotalExperienceCount - 1)
        {
            CurrentIndex++;
        }
        if (keyboardCode == "ArrowLeft" && CurrentIndex > 0)
        {
            CurrentIndex--;
        }
        ExperienceDetails = ListOfExperienceDetails![CurrentIndex];
    }

    protected override void OnParametersSet()
    {
        TotalExperienceCount = ListOfExperienceDetails!.Count;
        ExperienceDetails = ListOfExperienceDetails[CurrentIndex];
    }
}