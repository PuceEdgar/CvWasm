using CvWasm.DTO;
using CvWasm.Models;
using Microsoft.AspNetCore.Components;

namespace CvWasm.Pages;
public partial class About
{
    [Parameter]
    public AboutPageData? AboutDetails { get; set; }
}