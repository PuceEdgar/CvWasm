using CvWasm.DTO;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class HardSkills
{
    [Parameter]
    public HardSkillsPageData? HardSkillsDetails { get; set; }
}