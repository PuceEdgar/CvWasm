using CvWasm.Factory;
using CvWasm.Managers;
using NSubstitute;

namespace CvWasm.Tests;
public class SharedMethods
{
    private readonly IComponentRepository _componentRepository;

    public SharedMethods(IComponentRepository componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public BaseComponent MockComponentCreation(string command, string? message = null)
    {
        var component = ComponentFactory.CreateComponent(command, message);
        _componentRepository.CreateNewComponent(command, message).Returns(component);
        return component;
    }
}
