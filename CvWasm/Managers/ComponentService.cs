using CvWasm.Factory;

namespace CvWasm.Managers;

public class ComponentService : IComponentService
{
    private readonly List<BaseComponent> _componentList = [];

    public List<BaseComponent> LoadedComponents => _componentList;

    public void AddComponentToLoadedComponentList(BaseComponent component)
    {       
        _componentList.Add(component);
    }

    public BaseComponent CreateNewComponent(string command, string? message = null)
    {
        return ComponentFactory.CreateComponent(command, message);
    }

    public void ClearWindow()
    {
        _componentList.Clear();
    }
}
