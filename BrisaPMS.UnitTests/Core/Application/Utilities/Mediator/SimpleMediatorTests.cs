using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.UnitTests.Application.Utilities.Mediator;

public class SimpleMediatorTests
{
    public class FakeRequest : IRequest<string>
    {
        public required string Name { get; set; }
    }

    public class FakeHandler : IRequestHandler<FakeRequest, string>
    {
        public Task<string> Handle(FakeRequest request)
        {
            return Task.FromResult("Correct response");
        }
    }

    public class FakeValidatorRequest : AbstractValidator<FakeRequest>
    {
        public FakeValidatorRequest()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    [Fact]
    public async Task Send_CallsHandler()
    {
        // Arrange
        var request = new FakeRequest() {Name = "Test name"};
        var useCaseMock = Substitute.For<IRequestHandler<FakeRequest, string>>();
        var serviceProviderMock = Substitute.For<IServiceProvider>();
        
        serviceProviderMock
            .GetService(typeof(IRequestHandler<FakeRequest, string>))
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
        var request = new FakeRequest() {Name =  "Test name"};
        var serviceProviderMock = Substitute.For<IServiceProvider>();
        var mediator = new SimpleMediator(serviceProviderMock);
        
        // Act
        var act = async () => await mediator.Send(request);
        
        // Assert
        await act.Should().ThrowAsync<MediatorException>();
    }

    [Fact]
    public async Task Send_ThrowsValidationException_WhenCommandIsNotValid()
    {
        // Arrange
        var request = new FakeRequest() {Name = ""};
        var serviceProviderMock = Substitute.For<IServiceProvider>();
        var validator = new FakeValidatorRequest();

        serviceProviderMock
            .GetService(typeof(IValidator<FakeRequest>))
            .Returns(validator);
        
        var mediator = new SimpleMediator(serviceProviderMock);
        
        // Act
        var act = async () => await mediator.Send(request);
        
        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}