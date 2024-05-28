namespace CvWasm.Managers;

public class ComponentListManager : IComponentListManager
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
