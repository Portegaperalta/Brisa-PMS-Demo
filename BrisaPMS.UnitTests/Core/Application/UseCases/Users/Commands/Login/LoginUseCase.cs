using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.UseCases.Users.Commands.Login;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.Login;

public class LoginUseCaseTests
{
  private readonly IIdentityService _identityServiceMock;
  private readonly LoginUseCase _useCase;

  public LoginUseCaseTests()
  {
    _identityServiceMock = Substitute.For<IIdentityService>();
    _useCase = new LoginUseCase(_identityServiceMock);
  }

  [Fact]
  public async Task Handle_ReturnsToken_WhenCredentialsAreValid()
  {
    // Arrange
    var command = new LoginCommand
    {
      Email = "test@example.com",
      Password = "Test@1234"
    };

    var expectedToken = "auth-token";

    _identityServiceMock.LoginAsync(command.Email, command.Password).Returns(expectedToken);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _identityServiceMock.Received(1).LoginAsync(command.Email, command.Password);
    result.Should().Be(expectedToken);
  }

  [Fact]
  public async Task Handle_ThrowsException_WhenIdentityServiceFails()
  {
    // Arrange
    var command = new LoginCommand
    {
      Email = "test@example.com",
      Password = "Test@1234"
    };

    _identityServiceMock.LoginAsync(command.Email, command.Password).ThrowsAsync(new InvalidOperationException());

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _identityServiceMock.Received(1).LoginAsync(command.Email, command.Password);
  }
}