using CvWasm.Factory;

namespace CvWasm.Managers;

public interface IComponentManager
{
    List<ComponentMetadata> LoadedComponents { get; }
    void AddComponentToLoadedComponentList(ComponentMetadata componentMetadata);
    ComponentMetadata CreateResultComponent(string message, string command);
    ComponentMetadata GetExistingComponent(string command);
    void ClearWindow();
}
