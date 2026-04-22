using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Commands.UpdateUserName;
using BrisaPMS.Domain.Users;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.UpdateUserName;

public class UpdateUserNameUseCaseTests
{
    private readonly IUsersRepository _usersRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateUserNameUseCase _useCase;

    public UpdateUserNameUseCaseTests()
    {
        _usersRepositoryMock = Substitute.For<IUsersRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateUserNameUseCase(_usersRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesNameAndReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = CreateUser(userId, UserRole.Receptionist);
        var command = new UpdateUserNameCommand
        {
            UserId = userId,
            FirstName = "Jane",
            LastName = "Smith"
        };

        _usersRepositoryMock.GetById(userId).Returns(user);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _usersRepositoryMock.Received(1).GetById(userId);
        await _usersRepositoryMock.Received(1).Update(Arg.Is<User>(u => 
            u.FirstName == "Jane" && 
            u.LastName == "Smith"));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserNameCommand
        {
            UserId = userId,
            FirstName = "Jane",
            LastName = "Smith"
        };

        _usersRepositoryMock.GetById(userId).Returns((User?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
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
        var command = new UpdateUserNameCommand
        {
            UserId = userId,
            FirstName = "Jane",
            LastName = "Smith"
        };

        _usersRepositoryMock.GetById(userId).Returns(user);
        _usersRepositoryMock.Update(Arg.Any<User>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
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
        .WithPhoneNumber(new PhoneNumber("+18095551234"))
        .Build();
    }
}