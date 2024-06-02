using CvWasm.Factory;

namespace CvWasm.Managers;

public interface IComponentService
{
    List<BaseComponent> LoadedComponents { get; }
    void AddComponentToLoadedComponentList(BaseComponent componentMetadata);
    BaseComponent CreateNewComponent(string command, string? message = null);
    void ClearWindow();
}
