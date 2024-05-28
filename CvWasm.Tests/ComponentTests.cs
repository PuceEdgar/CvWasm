using Bunit;
using CvWasm.Pages;
using CvWasm.SharedComponents;

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

    [Fact]
    public void TableHeaderAndDataRowComponentDisplaysHeaderAndData()
    {
        string header = "header";
        string data = "data";
        var cut = RenderComponent<TableHeaderAndDataRow>(
            parameters => parameters.Add(p => p.TableHeader, header).Add(p => p.TableData, data)
            );

        var tr = cut.Find("tr");
        var th = cut.Find("th");
        var td = cut.Find("td");

        Assert.Equal(2, tr.Children.Length);
        Assert.Equal(th, tr.Children[0]);
        Assert.Equal(td, tr.Children[1]);

        th.MarkupMatches($"<th>{header}</th>");
        td.MarkupMatches($"<td>{data}</td>");
    }
}