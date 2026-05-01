using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Commands.ChangeRole;
using BrisaPMS.Domain.Users;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangeRole;

public class ChangeRoleUseCaseTests
{
  private readonly IUsersRepository _usersRepositoryMock;
  private readonly IIdentityService _identityServiceMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly ChangeRoleUseCase _useCase;

  public ChangeRoleUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _identityServiceMock = Substitute.For<IIdentityService>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new ChangeRoleUseCase(_usersRepositoryMock, _identityServiceMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_ChangesRoleAndReturnsTrue()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = CreateUser(userId, UserRole.Receptionist);
    var command = new ChangeRoleCommand
    {
      UserId = userId,
      Role = "Manager"
    };

    _usersRepositoryMock.GetById(userId).Returns(user);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _usersRepositoryMock.Received(1).GetById(userId);
    await _usersRepositoryMock.Received(1).Update(Arg.Is<User>(u => u.Role == UserRole.Manager));
    await _identityServiceMock.Received(1).AssignRoleAsync(command.UserId, UserRole.Manager);
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
    result.Should().BeTrue();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExist()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var command = new ChangeRoleCommand
    {
      UserId = userId,
      Role = "Admin"
    };

    _usersRepositoryMock.GetById(userId).Returns((User?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _identityServiceMock.DidNotReceive().AssignRoleAsync(Arg.Any<Guid>(), Arg.Any<UserRole>());
    await _usersRepositoryMock.DidNotReceive().Update(Arg.Any<User>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = CreateUser(userId, UserRole.Receptionist);
    var command = new ChangeRoleCommand
    {
      UserId = userId,
      Role = "Manager"
    };

    _usersRepositoryMock.GetById(userId).Returns(user);
    _usersRepositoryMock.Update(Arg.Any<User>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _identityServiceMock.DidNotReceive().AssignRoleAsync(Arg.Any<Guid>(), Arg.Any<UserRole>());
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  private static User CreateUser(Guid userId, UserRole role)
  {
    return new User.Builder(
        role,
        "John",
        "Doe",
        new Email("test@example.com"),
        UserPreferredLanguage.En)
    .WithHotelId(Guid.NewGuid())
    .WithPhoneNumber(new PhoneNumber("+18095551234"))
    .Build();
  }
}
