using CvWasm.Factory;

namespace CvWasm.Managers;

public class ComponentRepository : IComponentRepository
{
    private readonly List<BaseComponent> _componentList = [];

    public List<BaseComponent> LoadedComponents => _componentList;

    public void AddComponentToList(BaseComponent component)
    {
        _componentList.Add(component);
    }

    public void ClearWindow()
    {
        _componentList.Clear();
    }

    public BaseComponent CreateNewComponent(string command, string? message = null)
    {
        return ComponentFactory.CreateComponent(command, message);
    }

    public void CreateNewComponentAndAddToList(string command, string? message = null)
    {
        var component = ComponentFactory.CreateComponent(command, message);
        _componentList.Add(component);
    }
}
