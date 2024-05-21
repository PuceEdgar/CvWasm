namespace CvWasm;

public static class FileManager
{
    public static async Task<string> GetBase64FromPdfCv(HttpClient Http, string language)
    {
        var pdfAsByteArray = await Http.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        return Convert.ToBase64String(pdfAsByteArray);
    }
}
