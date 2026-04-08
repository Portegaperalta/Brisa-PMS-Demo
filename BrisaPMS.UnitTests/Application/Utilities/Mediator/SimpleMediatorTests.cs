using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.Utilities.Mediator;

public class SimpleMediatorTests
{
    public class FakeRequest : IRequest<bool> {}

    public class FakeHandler : IRequestHandler<FakeRequest, bool>
    {
        public Task<bool> Handle(FakeRequest request)
        {
            return Task.FromResult(true);
        }
    }

    [Fact]
    public async Task Send_CallsHandler()
    {
        // Arrange
        var request = new FakeRequest();
        var useCaseMock = Substitute.For<IRequestHandler<FakeRequest, bool>>();
        var serviceProviderMock = Substitute.For<IServiceProvider>();
        
        serviceProviderMock
            .GetService(typeof(IRequestHandler<FakeRequest, bool>))
            .Returns(useCaseMock);
        
        var mediator = new SimpleMediator(serviceProviderMock);
        
        // Act
        var result = await mediator.Send(request);
        
        // Assert
        await useCaseMock.Received(1).Handle(request);
    }
    
    [Fact]
    public async Task Send_ThrowsMediatorException_WhenHandlerIsNotRegistered()
    {
        // Arrange
        var request = new FakeRequest();
        var serviceProviderMock = Substitute.For<IServiceProvider>();
        var mediator = new SimpleMediator(serviceProviderMock);
        
        // Act
        var act = async () => await mediator.Send(request);
        
        // Assert
        await act.Should().ThrowAsync<MediatorException>();
    }
}