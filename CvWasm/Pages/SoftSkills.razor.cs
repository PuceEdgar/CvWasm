using CvWasm.Models;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;

public partial class SoftSkills
{
    [Parameter]
    public SoftSkillsModel? SoftSkillsDetails { get; set; }
}