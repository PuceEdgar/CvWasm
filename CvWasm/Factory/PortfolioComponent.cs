using CvWasm.Pages;

namespace CvWasm.Factory;

public class PortfolioComponent : BaseComponent
{
    public PortfolioComponent()
    {
        Type = typeof(Portfolio);
        Command = PortfolioCommand;
        Parameters = new() { [nameof(Portfolio.ListOfPortfolioDetails)] = PageDataLoader.GetPortfolioPageDataFromCv() };
    }
}
