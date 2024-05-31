namespace CvWasm.Managers;

public interface IFileManager
{
    Task<string> GetBase64FromPdfCv(string language);
    //Task<T> LoadDataFromJson<T>(string pathToJson) where T : new();
    //Task<string> LoadDataAsString(string pathToFile);
    Task LoadCvDataFromJson();
    Task<string> LoadAsciiArtFromFile();
    Task LoadCommandDescriptionFromJson();
}
