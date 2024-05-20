using System.Net.Http.Json;

namespace CvWasm;

public static class FileUtil
{
    public static async Task<string> GetPdfCvBase64(HttpClient Http, string language)
    {
        var response = await Http.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        var pdfExportB64 = Convert.ToBase64String(response);
        return pdfExportB64;
    }

    public static async Task<Dictionary<string, string[]>> GetCommandDescription(HttpClient Http)
    {
        var response = await Http.GetFromJsonAsync<Dictionary<string, string[]>>($"file-data/CommandDescription.json");        
        return response;
    }
}
