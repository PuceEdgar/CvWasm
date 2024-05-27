using CvWasm.Models;

namespace CvWasm.Managers;

public interface IComponentManager
{
    void InitializeComponentsWithParameters(CvModel cv, Languages language);
    void AddErrorComponentWithMessage(string errorMessage, string action);
    void LoadComponent(string command);
    void LoadCommandResultComponent(string message, string command);
}
