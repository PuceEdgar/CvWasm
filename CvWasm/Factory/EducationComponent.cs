using CvWasm.Pages;

namespace CvWasm.Factory;

public class EducationComponent : IComponent
{
    public ComponentMetadata CreateComponent()
    {
        return new ComponentMetadata()
        {
            Type = typeof(Education),
            Command = EducationCommand,
            Parameters = { [nameof(Education.EducationDetails)] = PageDataLoader.GetEducationPageDataFromCv() }
        };
    }
}
