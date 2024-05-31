using CvWasm.Pages;

namespace CvWasm.Factory;

public class WorkExperienceComponent : IComponent
{
    public ComponentMetadata CreateComponent()
    {
        return new ComponentMetadata()
        {
            Type = typeof(WorkExperience),
            Command = ExperienceCommand,
            Parameters = {
                [nameof(WorkExperience.ListOfExperienceDetails)] = PageDataLoader.GetWorkExperiencePageDataFromCv() }
        };
    }
}
