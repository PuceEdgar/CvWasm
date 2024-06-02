using CvWasm.Factory;

namespace CvWasm.Managers;

public class ComponentService : IComponentService
{
    private readonly List<BaseComponent> _componentList = [];

    public List<BaseComponent> LoadedComponents => _componentList;

    public void ClearWindow()
    {
        _componentList.Clear();
    }

    public void CreateNewComponentAndAddToList(string command, string? message = null)
    {
        var component = ComponentFactory.CreateComponent(command, message);
        _componentList.Add(component);
    }
}
