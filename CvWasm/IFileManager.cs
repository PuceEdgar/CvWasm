namespace CvWasm;

public interface IFileManager
{
    Task<string> GetBase64FromPdfCv(HttpClient Http, string language);
    Task<T> LoadDataFromJson<T>(HttpClient http, string pathToJson);
    Task<string> LoadDataAsString(HttpClient http, string pathToFile);
}
