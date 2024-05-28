namespace CvWasm.Managers;

public interface IComponentListManager
{
    List<CommandAndData> LoadedComponents { get; }
    void AddNewComponent(CommandAndData commandAndData);
    void ClearList();
}
