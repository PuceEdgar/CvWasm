namespace CvWasm;

public class FileUtil
{
    public static async Task<string> GetPdfCvBase64(HttpClient Http, string language)
    {
        // call your api to download the file you want to download
        var response = await Http.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        // convert to base64
        //var pdfExportBytes = await response.Content.ReadAsByteArrayAsync();
        var pdfExportB64 = Convert.ToBase64String(response);
        // invoke js download
        //await JSRuntime.InvokeVoidAsync("downloadFile", "FileName", pdfExportB64);
        //await JSRuntime.InvokeVoidAsync("scrollToInput");
        return pdfExportB64;
    }
}
