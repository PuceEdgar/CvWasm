using CvWasm.Factory;

namespace CvWasm.Managers;

public interface IComponentService
{
    List<BaseComponent> LoadedComponents { get; }
    void ClearWindow();
    void CreateNewComponentAndAddToList(string command, string? message = null);
}
