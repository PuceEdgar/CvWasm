using CvWasm.Factory;

namespace CvWasm.Managers;

public interface IComponentManager
{
    List<BaseComponent> LoadedComponents { get; }
    void AddComponentToLoadedComponentList(BaseComponent componentMetadata);
    BaseComponent CreateNewComponent(string command);
    void ClearWindow();
}
