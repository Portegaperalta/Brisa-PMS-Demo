using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Commands.ChangePassword;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangePassword;

public class ChangePasswordUseCaseTests
{
  private readonly IUsersRepository _usersRepositoryMock;
  private readonly IIdentityService _identityServiceMock;
  private readonly ChangePasswordUseCase _useCase;

  public ChangePasswordUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _identityServiceMock = Substitute.For<IIdentityService>();
    _useCase = new ChangePasswordUseCase(_usersRepositoryMock, _identityServiceMock);
  }

  [Fact]
  public async Task Handle_UpdatesPasswordAndReturnsTrue()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new ChangePasswordCommand
    {
      UserId = userId,
      CurrentPassword = "CurrentPassword@123",
      NewPassword = "NewPassword@123"
    };

    _usersRepositoryMock.Exists(userId).Returns(true);
    _identityServiceMock.CheckPasswordAsync(command.UserId, command.CurrentPassword).Returns(true);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _usersRepositoryMock.Received(1).Exists(userId);
    await _identityServiceMock.Received(1).CheckPasswordAsync(command.UserId, command.CurrentPassword);
    await _identityServiceMock.Received(1).UpdatePasswordAsync(command.UserId, command.NewPassword);
    result.Should().BeTrue();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExist()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new ChangePasswordCommand
    {
      UserId = userId,
      CurrentPassword = "CurrentPassword@123",
      NewPassword = "NewPassword@123"
    };

    _usersRepositoryMock.Exists(userId).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _identityServiceMock.DidNotReceive().CheckPasswordAsync(Arg.Any<Guid>(), Arg.Any<string>());
    await _identityServiceMock.DidNotReceive().UpdatePasswordAsync(Arg.Any<Guid>(), Arg.Any<string>());
  }

  [Fact]
  public async Task Handle_ThrowsIncorrectPasswordException_WhenCurrentPasswordIsInvalid()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new ChangePasswordCommand
    {
      UserId = userId,
      CurrentPassword = "WrongPassword@123",
      NewPassword = "NewPassword@123"
    };

    _usersRepositoryMock.Exists(userId).Returns(true);
    _identityServiceMock.CheckPasswordAsync(command.UserId, command.CurrentPassword).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<IncorrectPasswordException>();
    await _identityServiceMock.Received(1).CheckPasswordAsync(command.UserId, command.CurrentPassword);
    await _identityServiceMock.DidNotReceive().UpdatePasswordAsync(Arg.Any<Guid>(), Arg.Any<string>());
  }
}