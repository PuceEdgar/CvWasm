namespace CvWasm.Managers;

public class ComponentListManager : IComponentListManager
{
    private readonly List<ComponentMetadata> _componentList = [];

    public List<ComponentMetadata> LoadedComponents => _componentList;

    public void AddNewComponent(ComponentMetadata componentMetadata)
    {
        _componentList.Add(componentMetadata);
    }

    public void ClearList()
    {
        _componentList.Clear();
    }
}
