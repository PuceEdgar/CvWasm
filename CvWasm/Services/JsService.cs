using Microsoft.JSInterop;

namespace CvWasm.Managers;

public class JsService : IJsService
{
    private readonly IJSRuntime _runtime;

    public JsService(IJSRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task CallJsFunctionByName(string name)
    {
        await _runtime.InvokeVoidAsync(name);
    }

    public async Task CallJsFunctionToDownloadCv(Languages language, string base64)
    {
        await _runtime.InvokeVoidAsync("downloadFile", $"Edgars_Puce_CV_{language}.pdf", base64);
    }

    public async Task CallJsFunctionToOpenUrl(string url)
    {
        await _runtime.InvokeVoidAsync("open", url, "_blank");
    }
}
