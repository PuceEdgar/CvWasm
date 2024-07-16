namespace CvWasm.Managers;

public interface IFileService
{
    Task<string> GetBase64FromPdfCv(Languages language);
    Task LoadCvDataFromJson();
    Task<string> LoadAsciiArtFromFile();
    Task LoadCommandDescriptionFromJson();
}
