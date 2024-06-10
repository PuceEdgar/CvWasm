using CvWasm.Models;
using System.Net.Http.Json;

namespace CvWasm.Managers;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly IComponentRepository _componentRepository;

    public FileService(HttpClient httpClient, IComponentRepository componentManager)
    {
       _httpClient = httpClient;
        _componentRepository = componentManager;
    }

    public async Task<string> GetBase64FromPdfCv(Languages language)
    {
        var pdfAsByteArray = await _httpClient.GetByteArrayAsync($"cv-data/Edgars_Puce_{language}.pdf");
        return Convert.ToBase64String(pdfAsByteArray);        
    }

    public async Task LoadCvDataFromJson()
    {
        try
        {
            var language = StateContainer.CurrentSelectedLanguage;
            if (!StateContainer.LoadedCvs.ContainsKey(language))
            {
                StateContainer.LoadedCvs[language] = await LoadDataFromJson<CvModel>($"cv-data/cv-{language}.json");
            }            
        }
        catch (Exception)
        {
            var component = _componentRepository.CreateNewComponent("load cv", CvLoadFailed);
            _componentRepository.AddComponentToList(component);
        }
    }

    public async Task<string> LoadAsciiArtFromFile()
    {
        try
        {
            return await _httpClient.GetStringAsync(AsciiArtPath);
        }
        catch (Exception)
        {
            var component = _componentRepository.CreateNewComponent("load ascii art", AsciiArtLoadFailed);
            _componentRepository.AddComponentToList(component);
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
            var component = _componentRepository.CreateNewComponent("load command descriptions", CommandDescriptionLoadFailed);
            _componentRepository.AddComponentToList(component);
        }
    }

    private async Task<T> LoadDataFromJson<T>(string pathToJson) where T : new()
    {
        return await _httpClient.GetFromJsonAsync<T>(pathToJson) ?? new T();
    }
}
