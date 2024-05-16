namespace CvWasm.Models;

public record SkillsModel
{
    public HardSkillsModel? HardSkills { get; set; }
    public SoftSkillsModel? SoftSkills { get; set; }
}