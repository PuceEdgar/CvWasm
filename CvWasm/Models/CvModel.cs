namespace CvWasm.Models;

public record CvModel
{
    public AboutModel? About { get; set; }
    public WorkExperienceModel[]? Experience { get; set; }
    public SkillsModel? Skills { get; set; }
    public EducationModel? Education { get; set; }
}