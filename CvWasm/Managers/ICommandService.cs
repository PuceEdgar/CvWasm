namespace CvWasm.Managers;

public interface ICommandService
{
    Task ExecuteCommand(string command);
}
