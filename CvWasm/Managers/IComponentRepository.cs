using CvWasm.Factory;

namespace CvWasm.Managers;

public interface IComponentRepository
{
    List<BaseComponent> LoadedComponents { get; }
    void ClearWindow();
    BaseComponent CreateNewComponent(string command, string? message = null);
    void AddComponentToList(BaseComponent component);
}
