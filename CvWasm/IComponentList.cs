namespace CvWasm;

public interface IComponentList
{
    List<CommandAndData> LoadedComponents { get; }
    void AddNewComponent(CommandAndData commandAndData);
    void ClearList();
}
