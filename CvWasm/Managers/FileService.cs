using CvWasm.Models;
using System.Net.Http.Json;

namespace CvWasm.Managers;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly IComponentService _componentService;

    public FileService(HttpClient httpClient, IComponentService componentManager)
    {
       _httpClient = httpClient;
        _componentService = componentManager;
    }

    public async Task<string> GetBase64FromPdfCv(string language)
    {
        try
        {
            var pdfAsByteArray = await _httpClient.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
            return Convert.ToBase64String(pdfAsByteArray);
        }
        catch (Exception)
        {
            _componentService.CreateNewComponentAndAddToList("cv download", CvDownloadFailed);
            return string.Empty;
        }
        
    }

    public async Task LoadCvDataFromJson()
    {
        try
        {
            StateContainer.LoadedCvs[Languages.eng] = await LoadDataFromJson<CvModel>($"cv-data/cv-{Languages.eng}.json");
            StateContainer.LoadedCvs[Languages.kor] = await LoadDataFromJson<CvModel>($"cv-data/cv-{Languages.kor}.json");
        }
        catch (Exception)
        {
            _componentService.CreateNewComponentAndAddToList("load cv", CvLoadFailed);
        }
    }

    public async Task<string> LoadAsciiArtFromFile()
    {
        try
        {
            return await LoadDataAsString(AsciiArtPath);
        }
        catch (Exception)
        {
            _componentService.CreateNewComponentAndAddToList("load ascii art", AsciiArtLoadFailed);
            return string.Empty;
        }
    }

    public async Task LoadCommandDescriptionFromJson()
    {
        try
        {
            StateContainer.CommandDescriptions = await LoadDataFromJson<Dictionary<Languages, Dictionary<string, string>[]>>(CommandDescriptionPath);
        }
        catch (Exception)
        {
            _componentService.CreateNewComponentAndAddToList("load command descriptions", CommandDescriptionLoadFailed);       
        }
    }

    private async Task<T> LoadDataFromJson<T>(string pathToJson) where T : new()
    {
        return await _httpClient.GetFromJsonAsync<T>(pathToJson) ?? new T();
    }

    private async Task<string> LoadDataAsString(string pathToFile)
    {
        return await _httpClient.GetStringAsync(pathToFile);
    }
}
