using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Commands.ChangePreferredLanguage;
using BrisaPMS.Domain.Users;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangePreferredLanguage;

public class ChangePreferredLanguageUseCaseTests
{
  private readonly IUsersRepository _usersRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly ChangePreferredLanguageUseCase _useCase;

  public ChangePreferredLanguageUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new ChangePreferredLanguageUseCase(_usersRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_UpdatesPreferredLanguageAndReturnsTrue()
  {
    var userId = Guid.NewGuid();
    var user = CreateUser(userId, UserRole.Receptionist);
    var command = new ChangePreferredLanguageCommand
    {
      UserId = userId,
      PreferredLanguage = "Es"
    };

    _usersRepositoryMock.GetById(userId).Returns(user);

    var result = await _useCase.Handle(command);

    await _usersRepositoryMock.Received(1).GetById(userId);
    await _usersRepositoryMock.Received(1).Update(user);
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
    result.Should().BeTrue();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExist()
  {
    var userId = Guid.NewGuid();
    var command = new ChangePreferredLanguageCommand
    {
      UserId = userId,
      PreferredLanguage = "Es"
    };

    _usersRepositoryMock.GetById(userId).Returns((User?)null);

    var act = async () => await _useCase.Handle(command);

    await act.Should().ThrowAsync<NotFoundException>();
    await _usersRepositoryMock.DidNotReceive().Update(Arg.Any<User>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
  {
    var userId = Guid.NewGuid();
    var user = CreateUser(userId, UserRole.Receptionist);
    var command = new ChangePreferredLanguageCommand
    {
      UserId = userId,
      PreferredLanguage = "Es"
    };

    _usersRepositoryMock.GetById(userId).Returns(user);
    _usersRepositoryMock.Update(Arg.Any<User>()).Throws<InvalidOperationException>();

    var act = async () => await _useCase.Handle(command);

    await act.Should().ThrowAsync<InvalidOperationException>();
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
        new Password("Test@1234"),
        UserPreferredLanguage.En)
    .WithHotelId(Guid.NewGuid())
    .Build();
  }
}