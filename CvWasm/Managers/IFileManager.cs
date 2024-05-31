namespace CvWasm.Managers;

public interface IFileManager
{
    Task<string> GetBase64FromPdfCv(string language);
    Task LoadCvDataFromJson();
    Task<string> LoadAsciiArtFromFile();
    Task LoadCommandDescriptionFromJson();
}
