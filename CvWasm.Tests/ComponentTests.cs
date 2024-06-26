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
        //Arrange
        var cut = RenderComponent<CommandResult>(
            parameters => parameters.Add(p => p.Result, message)
            );

        //Assert
        cut.Find("p").MarkupMatches($"<p>{message}</p>");
    }

    [Fact]
    public void TableHeaderAndDataRowComponentDisplaysHeaderAndData()
    {
        //Arrange
        string header = "header";
        string data = "data";
        var cut = RenderComponent<TableHeaderAndDataRow>(
            parameters => parameters.Add(p => p.TableHeader, header).Add(p => p.SimpleData, data)
            );

        var tr = cut.Find("tr");
        var th = cut.Find("th");
        var td = cut.Find("td");

        //Assert
        Assert.Equal(2, tr.Children.Length);
        Assert.Equal(th, tr.Children[0]);
        Assert.Equal(td, tr.Children[1]);

        th.MarkupMatches($"<th>{header}</th>");
        td.MarkupMatches($"<td>{data}</td>");
    }

    [Fact]
    public void TextArrayComponentDisplaysContent()
    {
        //Arrange
        string[] content = ["a", "b", "c"];
        var cut = RenderComponent<TextArray>(
            parameters => parameters.Add(p => p.TextCollection, content));

        var nodes = cut.Nodes;

        //Assert
        Assert.Equal(content.Length, nodes.Length);
        Assert.Multiple(() =>
        {
            for (int i = 0; i < content.Length; i++)
            {
                nodes.ElementAt(i).MarkupMatches($"<p>{content[i]}</P>");
            }
        });
    }
}