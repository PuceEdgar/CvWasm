using CvWasm.DTO;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class Portfolio
{
    [Parameter]
    public List<PortfolioPageData>? ListOfPortfolioDetails { get; set; }

    private PortfolioPageData? ProjectDetails { get; set; }
    private int CurrentIndex { get; set; }
    private int TotalProjectCount { get; set; }

    public void SelectCurrentProject(string keyboardCode)
    {
        if (keyboardCode == "ArrowRight" && CurrentIndex < TotalProjectCount - 1)
        {
            CurrentIndex++;
        }
        if (keyboardCode == "ArrowLeft" && CurrentIndex > 0)
        {
            CurrentIndex--;
        }
        ProjectDetails = ListOfPortfolioDetails![CurrentIndex];
    }

    protected override void OnParametersSet()
    {
        TotalProjectCount = ListOfPortfolioDetails!.Count;
        ProjectDetails = ListOfPortfolioDetails[CurrentIndex];
    }
}