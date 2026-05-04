using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Contracts.Services;
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
  private readonly IIdentityService _identityServiceMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly CreateUserUseCase _useCase;

  public CreateUserUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
    _identityServiceMock = Substitute.For<IIdentityService>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new CreateUserUseCase(_usersRepositoryMock, _hotelsRepositoryMock, _identityServiceMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_CreatesUserAndReturnsToken()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);
    var expectedToken = "created-user-token";
    User? createdUser = null;
    Guid? identityDomainUserId = null;

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);

    _usersRepositoryMock
      .Create(Arg.Do<User>(user => createdUser = user))
      .Returns(callInfo => callInfo.Arg<User>());

    _identityServiceMock
      .CreateUserAsync(command.Email, command.Password, Enum.Parse<UserRole>(command.Role), Arg.Any<Guid>())
      .Returns(callInfo =>
      {
        identityDomainUserId = callInfo.ArgAt<Guid>(3);
        return expectedToken;
      });

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
    createdUser.Should().NotBeNull();

    await _usersRepositoryMock.Received(1).Create(Arg.Any<User>());

    await _unitOfWorkMock.Received(1).Persist();

    await _identityServiceMock.Received(1).CreateUserAsync
    (
        command.Email,
        command.Password,
        Enum.Parse<UserRole>(command.Role),
        Arg.Any<Guid>()
    );

    await _unitOfWorkMock.DidNotReceive().Revert();
    result.Should().Be(expectedToken);
    identityDomainUserId.Should().Be(createdUser!.Id);
  }

  [Fact]
  public async Task Handle_CreatesUser_WhenOptionalFieldsAreNotProvided()
  {
    // Arrange
    var expectedToken = "created-user-token";
    User? createdUser = null;
    Guid? identityDomainUserId = null;
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

    _usersRepositoryMock
      .Create(Arg.Do<User>(user => createdUser = user))
      .Returns(callInfo => callInfo.Arg<User>());

    _identityServiceMock
      .CreateUserAsync(command.Email, command.Password, UserRole.Admin, Arg.Any<Guid>())
      .Returns(callInfo => { identityDomainUserId = callInfo.ArgAt<Guid>(3); return expectedToken; });

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _usersRepositoryMock.Received(1).Create(Arg.Is<User>(user => user.HotelId == null && user.PhoneNumber == null));

    await _identityServiceMock.Received(1).CreateUserAsync(command.Email, command.Password, UserRole.Admin, Arg.Any<Guid>());
    await _unitOfWorkMock.Received(1).Persist();
    result.Should().Be(expectedToken);
    identityDomainUserId.Should().Be(createdUser!.Id);
  }

  [Fact]
  public async Task Handle_CreatesUser_WhenHotelIdIsProvided()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _identityServiceMock
      .CreateUserAsync(command.Email, command.Password, UserRole.Admin, Arg.Any<Guid>())
      .Returns("created-user-token");

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
    await _identityServiceMock.DidNotReceive().CreateUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<UserRole>(), Arg.Any<Guid>());
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
    await _usersRepositoryMock.DidNotReceive().Create(Arg.Any<User>());
    await _identityServiceMock.DidNotReceive().CreateUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<UserRole>(), Arg.Any<Guid>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateValidCommand(hotelId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _identityServiceMock
      .CreateUserAsync(command.Email, command.Password, UserRole.Admin, Arg.Any<Guid>())
      .Returns("created-user-token");
    _usersRepositoryMock.Create(Arg.Any<User>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _identityServiceMock.Received(1).CreateUserAsync(
      command.Email,
      command.Password,
      UserRole.Admin,
      Arg.Any<Guid>());
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenIdentityUserCreationFails()
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

    _hotelsRepositoryMock.Exists(command.HotelId!.Value).Returns(true);
    _identityServiceMock.CreateUserAsync(
        command.Email,
        command.Password,
        UserRole.Admin,
        Arg.Any<Guid>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _usersRepositoryMock.DidNotReceive().Create(Arg.Any<User>());
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
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
