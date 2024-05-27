using System.Net.Http.Json;

namespace CvWasm;

public class FileManager : IFileManager
{
    public async Task<string> GetBase64FromPdfCv(HttpClient http, string language)
    {
        var pdfAsByteArray = await http.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        return Convert.ToBase64String(pdfAsByteArray);
    }

    public async Task<T> LoadDataFromJson<T>(HttpClient http, string pathToJson)
    {
        return await http.GetFromJsonAsync<T>(pathToJson);
    }

    public async Task<string> LoadDataAsString(HttpClient http, string pathToFile)
    {
        return await http.GetStringAsync(pathToFile);
    }
}
