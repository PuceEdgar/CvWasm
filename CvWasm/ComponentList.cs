namespace CvWasm;

public class ComponentList : IComponentList
{
    private readonly List<CommandAndData> _componentList = [];

    public List<CommandAndData> LoadedComponents => _componentList;

    public void AddNewComponent(CommandAndData commandAndData)
    {
        _componentList.Add(commandAndData);
    }

    public void ClearList()
    {
        _componentList.Clear();
    }
}
