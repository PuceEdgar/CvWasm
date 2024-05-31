using CvWasm.Models;
using System.Net.Http.Json;

namespace CvWasm.Managers;

public class FileManager : IFileManager
{
    private readonly HttpClient _httpClient;
    private readonly IComponentManager _componentManager;

    public FileManager(HttpClient httpClient, IComponentManager componentManager)
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
            var result = _componentManager.CreateResultComponent(ErrorManager.FailedToLoadCvMessage, "load cv");
            _componentManager.AddComponentToLoadedComponentList(result);
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
            var result = _componentManager.CreateResultComponent(ErrorManager.FailedToLoadAsciiArtMessage, "load ascii art");
            _componentManager.AddComponentToLoadedComponentList(result);
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
            var result = _componentManager.CreateResultComponent(ErrorManager.FailedToLoadCommandDescriptionMessage, "load command descriptions");
            _componentManager.AddComponentToLoadedComponentList(result);            
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
