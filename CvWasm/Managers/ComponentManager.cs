using CvWasm.Factory;
using CvWasm.Pages;

namespace CvWasm.Managers;

public class ComponentManager : IComponentManager
{
    private Dictionary<string, ComponentMetadata> Components = [];
    private readonly List<ComponentMetadata> _componentList = [];

    public List<ComponentMetadata> LoadedComponents => _componentList;

    public ComponentMetadata CreateResultComponent(string message, string command)
    {
        return new ComponentMetadata()
        {
            Type = typeof(CommandResult),
            Command = command,
            Parameters = { [nameof(CommandResult.Result)] = message }
        };
    }

    public void AddComponentToLoadedComponentList(ComponentMetadata component)
    {       
        _componentList.Add(component);
    }

    public ComponentMetadata GetExistingComponent(string command)
    {
        var componentType = ComponentFactory.CreateComponent(command);
        return componentType.CreateComponent();
    }

    public void ClearWindow()
    {
        _componentList.Clear();
    }
}
