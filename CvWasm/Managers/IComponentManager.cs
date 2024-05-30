using CvWasm.Models;

namespace CvWasm.Managers;

public interface IComponentManager
{
    List<ComponentMetadata> LoadedComponents { get; }
    void InitializeComponentsWithParameters(CvModel cv, Languages language, Dictionary<string, string>[] commandDescriptions);
    void AddComponentToLoadedComponentList(ComponentMetadata componentMetadata);
    ComponentMetadata CreateResultComponent(string message, string command);
    ComponentMetadata GetExistingComponent(string command);
    void ClearWindow();
}
