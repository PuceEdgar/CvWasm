using CvWasm.Factory;

namespace CvWasm.Managers;

public class ComponentManager : IComponentManager
{
    private readonly List<BaseComponent> _componentList = [];

    public List<BaseComponent> LoadedComponents => _componentList;

    public void AddComponentToLoadedComponentList(BaseComponent component)
    {       
        _componentList.Add(component);
    }

    public BaseComponent CreateNewComponent(string command)
    {
        return ComponentFactory.CreateComponent(command);
    }

    public void ClearWindow()
    {
        _componentList.Clear();
    }
}
