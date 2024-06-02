using CvWasm.Pages;

namespace CvWasm.Factory;

public class EducationComponent : BaseComponent
{
    public EducationComponent()
    {
        Type = typeof(Education);
        Command = EducationCommand;
        Parameters = new() { [nameof(Education.EducationDetails)] = PageDataLoader.GetEducationPageDataFromCv() };
    }
}
