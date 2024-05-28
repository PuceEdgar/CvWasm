using Bunit;
using CvWasm.Pages;

namespace CvWasm.Tests;

public class ComponentTests : TestContext
{
    [Theory]
    [InlineData("success")]
    [InlineData("error")]
    public void CommandResultComponentDisplaysResult(string message)
    {
        var cut = RenderComponent<CommandResult>(
            parameters => parameters.Add(p => p.Result, message)
            );

        cut.Find("p").MarkupMatches($"<p>{message}</p>");
    }

    [Theory]
    [InlineData("success")]
    [InlineData("fail")]
    public void ErrorComponentDisplaysError(string message)
    {
        var cut = RenderComponent<Error>(
            parameters => parameters.Add(p => p.ErrorMessage, message)
            );

        cut.Find("p").MarkupMatches($"<p>{message}</p>");
    }
}