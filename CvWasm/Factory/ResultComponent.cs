using CvWasm.Pages;

namespace CvWasm.Factory;

public class ResultComponent : BaseComponent
{
    public ResultComponent(string message, string command)
    {
        Type = typeof(CommandResult);
        Command = command;
        Parameters =new () { [nameof(CommandResult.Result)] = message };
    }
}
