using CvWasm.Models;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class HardSkills
{
    [Parameter]
    public HardSkillsModel? HardSkillsDetails { get; set; }
}