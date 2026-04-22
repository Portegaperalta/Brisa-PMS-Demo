using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Commands.CreateUser;
using BrisaPMS.Domain.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserUseCaseTests
{
  private readonly IUsersRepository _usersRepositoryMock;
  private readonly IHotelsRepository _hotelsRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly CreateUserUseCase _useCase;

  public CreateUserUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new CreateUserUseCase(_usersRepositoryMock, _hotelsRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_CreatesUserAndReturnsUserId()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
    await _usersRepositoryMock.Received(1).Create(Arg.Is<User>(user =>
        user.Role == Enum.Parse<UserRole>(command.Role) &&
        user.HotelId == command.HotelId &&
        user.FirstName == command.FirstName &&
        user.LastName == command.LastName &&
        user.Email.Value == command.Email &&
        user.PhoneNumber!.Value == command.PhoneNumber &&
        user.PreferredLanguage == Enum.Parse<UserPreferredLanguage>(command.PreferredLanguage)));

    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
    result.Should().NotBe(Guid.Empty);
  }

  [Fact]
  public async Task Handle_CreatesUser_WhenOptionalFieldsAreNotProvided()
  {
    // Arrange
    var command = CreateCommand
    (
      "Admin",
      null,
      "John",
      "Doe",
      "test@example.com",
      "Test@1234",
      null,
      "En"
    );

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _usersRepositoryMock.Received(1).Create(Arg.Is<User>(user =>
        user.HotelId == null &&
        user.PhoneNumber == null));

    await _unitOfWorkMock.Received(1).Persist();
  }

  [Fact]
  public async Task Handle_CreatesUser_WhenHotelIdIsProvided()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);

    // Act
    await _useCase.Handle(command);

    // Assert
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _usersRepositoryMock.DidNotReceive().Create(Arg.Any<User>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelIdIsInvalid()
  {
    // Arrange
    var command = CreateCommand(
        "Admin",
        Guid.NewGuid(),
        "John",
        "Doe",
        "test@example.com",
        "Test@1234",
        "+18095551234",
        "En");

    _hotelsRepositoryMock.Exists(command.HotelId!.Value).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _usersRepositoryMock.Create(Arg.Any<User>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  [Fact]
  public async Task Handle_ThrowsException_WhenPasswordIsInvalid()
  {
    // Arrange
    var command = CreateCommand(
        "Admin",
        Guid.NewGuid(),
        "John",
        "Doe",
        "test@example.com",
        "weak",
        "+18095551234",
        "En");

    _hotelsRepositoryMock.Exists(command.HotelId!.Value).Returns(true);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<Exception>();
  }

  private static CreateUserCommand CreateValidCommand(Guid hotelId)
  {
    return CreateCommand(
        "Admin",
        hotelId,
        "John",
        "Doe",
        "test@example.com",
        "Test@1234",
        "+18095551234",
        "En");
  }

  private static CreateUserCommand CreateCommand(
      string role,
      Guid? hotelId,
      string firstName,
      string lastName,
      string email,
      string password,
      string? phoneNumber,
      string preferredLanguage)
  {
    return new CreateUserCommand
    {
      Role = role,
      HotelId = hotelId,
      FirstName = firstName,
      LastName = lastName,
      Email = email,
      Password = password,
      PhoneNumber = phoneNumber,
      PreferredLanguage = preferredLanguage
    };
  }
}