namespace CvWasm.Managers;

public interface IComponentListManager
{
    List<ComponentMetadata> LoadedComponents { get; }
    void AddNewComponent(ComponentMetadata componentMetadata);
    void ClearList();
}
