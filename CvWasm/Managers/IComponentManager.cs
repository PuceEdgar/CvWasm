using CvWasm.Models;

namespace CvWasm.Managers;

public interface IComponentManager
{
    void InitializeComponentsWithParameters(CvModel cv, Languages language, Dictionary<string, string>[] commandDescriptions);
    void AddComponentToLoadedComponentList(ComponentMetadata componentMetadata);
    ComponentMetadata CreateResultCommandAndData(string message, string command);
    ComponentMetadata GetExistingComponent(string command);
    List<ComponentMetadata> GetLoadedComponents();
    void ClearWindow();
}
