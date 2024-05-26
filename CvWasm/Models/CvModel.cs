namespace CvWasm.Models;

public record CvModel(AboutModel? About, WorkExperienceModel[]? Experience, SkillsModel? Skills, EducationModel? Education);