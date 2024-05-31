using CvWasm.Pages;

namespace CvWasm.Factory;

public class AboutComponent : IComponent
{
    public ComponentMetadata CreateComponent()
    {
        return new ComponentMetadata()
        {
            Type = typeof(About),
            Command = AboutCommand,
            Parameters = { [nameof(About.AboutDetails)] = PageDataLoader.GetAboutPageDataFromCv() }
        };
    }
}
