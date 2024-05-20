using CvWasm.Models;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class Education
{
    [Parameter]
    public EducationModel? Data { get; set; }
}