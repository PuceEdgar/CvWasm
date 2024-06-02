using CvWasm.Models;
using System.Net.Http.Json;

namespace CvWasm.Managers;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly IComponentService _componentManager;

    public FileService(HttpClient httpClient, IComponentService componentManager)
    {
       _httpClient = httpClient;
        _componentManager = componentManager;
    }

    public async Task<string> GetBase64FromPdfCv(string language)
    {
        var pdfAsByteArray = await _httpClient.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        return Convert.ToBase64String(pdfAsByteArray);
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
            var component = _componentManager.CreateNewComponent("load cv", ErrorService.FailedToLoadCvMessage);
            _componentManager.AddComponentToLoadedComponentList(component);
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
            var component = _componentManager.CreateNewComponent("load ascii art", ErrorService.FailedToLoadAsciiArtMessage);
            _componentManager.AddComponentToLoadedComponentList(component);
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
            var component = _componentManager.CreateNewComponent("load command descriptions", ErrorService.FailedToLoadCommandDescriptionMessage);
            _componentManager.AddComponentToLoadedComponentList(component);            
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
