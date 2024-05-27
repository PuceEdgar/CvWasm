using System.Net.Http.Json;

namespace CvWasm.Managers;

public class FileManager : IFileManager
{
    private readonly HttpClient _httpClient;

    public FileManager(HttpClient httpClient)
    {
       _httpClient = httpClient;
    }

    public async Task<string> GetBase64FromPdfCv(string language)
    {
        var pdfAsByteArray = await _httpClient.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        return Convert.ToBase64String(pdfAsByteArray);
    }

    public async Task<T> LoadDataFromJson<T>(string pathToJson)
    {
        return await _httpClient.GetFromJsonAsync<T>(pathToJson);
    }

    public async Task<string> LoadDataAsString(string pathToFile)
    {
        return await _httpClient.GetStringAsync(pathToFile);
    }
}
