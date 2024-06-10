using AutoFixture;
using CvWasm.Factory;
using CvWasm.Managers;
using CvWasm.Models;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace CvWasm.Tests;
public class FileServiceTests
{
    private readonly IComponentRepository _componentRepository = Substitute.For<IComponentRepository>();
    private readonly SharedMethods _sharedMethods;
    private readonly Fixture _fixture = new();

    public FileServiceTests()
    {
        _sharedMethods = new(_componentRepository);
    }

    [Fact]
    public async Task GetBase64FromPdfCv_WhenCalled_ReturnsString()
    {
        //Arrange
        byte[] byteArray = _fixture.Create<byte[]>();
        string base64response = Convert.ToBase64String(byteArray);
        FakeHttpMessageHandler handler = new (HttpStatusCode.OK, byteArray, true);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        string result = await sut.GetBase64FromPdfCv(Languages.eng);

        //Assert
        Assert.Equal(base64response, result);
    }

    [Fact]
    public async Task GetBase64FromPdfCv_WhenCalled_ThrowsExceptionAndReturnsEmptyString()
    {
        //Arrange
        byte[] byteArray = _fixture.Create<byte[]>();
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, byteArray, false);
        BaseComponent component = _sharedMethods.MockComponentCreation("cv download", CvDownloadFailed);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        string result = await sut.GetBase64FromPdfCv(Languages.eng);

        //Assert
        _componentRepository.Received().CreateNewComponent(Arg.Any<string>(), Arg.Any<string>());
        _componentRepository.Received().AddComponentToList(component);
        Assert.Empty(result);
    }

    [Fact]
    public async Task LoadAsciiArtFromFile_WhenCalled_ReturnsString()
    {
        //Arrange
        string content = _fixture.Create<string>();
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, content, true);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        string result = await sut.LoadAsciiArtFromFile();

        //Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task LoadAsciiArtFromFile_WhenCalled_ThrowsExceptionAndReturnsEmptyString()
    {
        //Arrange
        string content = _fixture.Create<string>(); ;
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, content, false);
        BaseComponent component = _sharedMethods.MockComponentCreation("load ascii art", AsciiArtLoadFailed);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        string result = await sut.LoadAsciiArtFromFile();

        //Assert
        _componentRepository.Received().CreateNewComponent(Arg.Any<string>(), Arg.Any<string>());
        _componentRepository.Received().AddComponentToList(component);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public async Task LoadCvDataFromJson_WhenCalled_LoadsCvFromJson(Languages language)
    {
        //Arrange
        StateContainer.CurrentSelectedLanguage = language;
        CvModel cv = _fixture.Create<CvModel>();
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, cv, true);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        await sut.LoadCvDataFromJson();

        //Assert
        Assert.NotNull(StateContainer.LoadedCvs[language]);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public async Task LoadCvDataFromJson_WhenCalled_ThrowsException(Languages language)
    {
        //Arrange
        StateContainer.CurrentSelectedLanguage = language;
        CvModel cv = _fixture.Create<CvModel>();
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, cv, false);
        BaseComponent component = _sharedMethods.MockComponentCreation("load cv", CvLoadFailed);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        await sut.LoadCvDataFromJson();

        //Assert
        Assert.False(StateContainer.LoadedCvs.ContainsKey(language));
        _componentRepository.Received().CreateNewComponent(Arg.Any<string>(), Arg.Any<string>());
        _componentRepository.Received().AddComponentToList(component);
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public async Task LoadCommandDescriptionFromJson_WhenCalled_LoadsCommandDescriptionsFromJson(Languages language)
    {
        //Arrange
        Dictionary<Languages, Dictionary<string, string>[]> content = _fixture.Create<Dictionary<Languages, Dictionary<string, string>[]>>();
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, content, true);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        await sut.LoadCommandDescriptionFromJson();

        //Assert
        Assert.True(StateContainer.CommandDescriptions.ContainsKey(language));
    }

    [Theory]
    [InlineData(Languages.eng)]
    [InlineData(Languages.kor)]
    public async Task LoadCommandDescriptionFromJson_WhenCalled_ThrowsException(Languages language)
    {
        //Arrange
        Dictionary<Languages, Dictionary<string, string>[]> content = _fixture.Create<Dictionary<Languages, Dictionary<string, string>[]>>();
        FakeHttpMessageHandler handler = new(HttpStatusCode.OK, content, false);
        BaseComponent component = _sharedMethods.MockComponentCreation("load command descriptions", CommandDescriptionLoadFailed);
        FileService sut = CreateFileServiceForResponse(handler);

        //Act
        await sut.LoadCommandDescriptionFromJson();

        //Assert
        Assert.False(StateContainer.CommandDescriptions.ContainsKey(language));
        _componentRepository.Received().CreateNewComponent(Arg.Any<string>(), Arg.Any<string>());
        _componentRepository.Received().AddComponentToList(component);
    }

    private FileService CreateFileServiceForResponse(FakeHttpMessageHandler fakeHandler)
    {
        var httpClient = new HttpClient(fakeHandler)
        {
            BaseAddress = new Uri("https://localhost")
        };
        return new FileService(httpClient, _componentRepository);
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly HttpContent _content;
    private readonly bool _isSuccess;

    public FakeHttpMessageHandler(HttpStatusCode statusCode, string response, bool isSuccess)
    {
        _statusCode = statusCode;
        _content = new StringContent(response);
        _isSuccess = isSuccess;
    }

    public FakeHttpMessageHandler(HttpStatusCode statusCode, byte[] response, bool isSuccess)
    {
        _statusCode = statusCode;
        _content = new ByteArrayContent(response);
        _isSuccess = isSuccess;
    }

    public FakeHttpMessageHandler(HttpStatusCode statusCode, CvModel response, bool isSuccess)
    {
        _statusCode = statusCode;
        _content = JsonContent.Create(response);
        _isSuccess = isSuccess;
    }
    public FakeHttpMessageHandler(HttpStatusCode statusCode, Dictionary<Languages, Dictionary<string, string>[]> response, bool isSuccess)
    {
        _statusCode = statusCode;
        _content = JsonContent.Create(response);
        _isSuccess = isSuccess;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_isSuccess)
        {
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = _statusCode,
                Content = _content
            });
        }
        else
        {
            throw new Exception();
        }
    }
}
