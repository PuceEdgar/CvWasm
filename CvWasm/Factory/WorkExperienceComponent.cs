using CvWasm.Pages;

namespace CvWasm.Factory;

public class WorkExperienceComponent : BaseComponent
{
    public WorkExperienceComponent()
    {
        Type = typeof(WorkExperience);
        Command = ExperienceCommand;
        Parameters = new() { [nameof(WorkExperience.ListOfExperienceDetails)] = PageDataLoader.GetWorkExperiencePageDataFromCv() };
    }
}
