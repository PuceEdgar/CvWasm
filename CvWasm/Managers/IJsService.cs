namespace CvWasm.Managers;

public interface IJsService
{
    Task CallJsFunctionToDownloadCv(Languages language, string base64);
    Task CallJsFunctionToOpenUrl(string url);
    Task CallJsFunctionByName(string name);
}
