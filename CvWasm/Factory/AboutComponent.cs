using CvWasm.Pages;

namespace CvWasm.Factory;

public class AboutComponent : BaseComponent
{
    public AboutComponent()
    {
        Type = typeof(About);
        Command = AboutCommand;
        Parameters = new() { [nameof(About.AboutDetails)] = PageDataLoader.GetAboutPageDataFromCv() };
    }
}
