using CvWasm.Models;

namespace CvWasm.Managers;

public class FileLoader
{
    //public async Task LoadCvDataFromJson(string language)
    //{
    //    try
    //    {
    //        if (!LoadedCvs.ContainsKey(language))
    //        {
    //            LoadedCvs[language] = await FileManager.LoadDataFromJson<CvModel>($"cv-data/cv-{language}.json");
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        var result = ComponentManager.CreateResultCommandAndData(ErrorManager.FailedToLoadCvMessage, "load cv");
    //        ComponentManager.AddComponentToLoadedComponentList(result);
    //    }
    //}

    //private async Task LoadAsciiArtFromFile()
    //{
    //    try
    //    {
    //        AsciiArt = await FileManager.LoadDataAsString(AsciiArtPath);
    //    }
    //    catch (Exception)
    //    {
    //        var result = ComponentManager.CreateResultCommandAndData(ErrorManager.FailedToLoadAsciiArtMessage, "load ascii art");
    //        ComponentManager.AddComponentToLoadedComponentList(result);
    //    }
    //}

    //private async Task LoadCommandDescriptionFromJson()
    //{
    //    try
    //    {
    //        CommandDescriptions = await FileManager.LoadDataFromJson<Dictionary<Languages, Dictionary<string, string>[]>>(CommandDescriptionPath);
    //    }
    //    catch (Exception)
    //    {
    //        var result = ComponentManager.CreateResultCommandAndData(ErrorManager.FailedToLoadCommandDescriptionMessage, "load command descriptions");
    //        ComponentManager.AddComponentToLoadedComponentList(result);
    //    }
    //}
}
