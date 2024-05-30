using CvWasm.Models;

namespace CvWasm.Managers;

public interface IComponentManager
{
    void InitializeComponentsWithParameters(CvModel cv, Languages language, Dictionary<string, string>[] commandDescriptions);
    void AddComponentToLoadedComponentList(CommandAndData component);
    CommandAndData CreateResultCommandAndData(string message, string command);
    CommandAndData CreateCommandAndDataFromExistingComponent(string command);
    List<CommandAndData> GetLoadedComponents();
    void ClearWindow();
}
