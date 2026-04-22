using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Queries.GetUserById;
using BrisaPMS.Domain.Users;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using BrisaPMS.Application.UseCases.Users.Shared;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdUseCaseTests
{
  private readonly IUsersRepository _usersRepositoryMock;
  private readonly GetUserByIdUseCase _useCase;

  public GetUserByIdUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _useCase = new GetUserByIdUseCase(_usersRepositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsUserDto()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = CreateUser(userId);
    var query = new GetUserByIdQuery { UserId = userId };

    _usersRepositoryMock.GetById(userId).Returns(user);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<UserDto>();
    result.Id.Should().Be(user.Id);
    result.Role.Should().Be(user.Role.ToString());
    result.FirstName.Should().Be(user.FirstName);
    result.LastName.Should().Be(user.LastName);
    result.Email.Should().Be(user.Email.Value);
    result.PhoneNumber.Should().Be(user.PhoneNumber!.Value);
    result.PreferredLanguage.Should().Be(user.PreferredLanguage.ToString());
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExist()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var query = new GetUserByIdQuery { UserId = userId };

    _usersRepositoryMock.GetById(userId).ReturnsNull();

    // Act
    var act = async () => await _useCase.Handle(query);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task Handle_CallsUsersRepository()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var user = CreateUser(userId);
    var query = new GetUserByIdQuery { UserId = userId };

    _usersRepositoryMock.GetById(userId).Returns(user);

    // Act
    await _useCase.Handle(query);

    // Assert
    await _usersRepositoryMock.Received(1).GetById(userId);
  }

  private static User CreateUser(Guid? userId = null)
  {
    var user = new User.Builder(
        UserRole.Receptionist,
        "John",
        "Doe",
        new Email("test@example.com"),
        new Password("Test@1234"),
        UserPreferredLanguage.En)
    .WithHotelId(Guid.NewGuid())
    .WithPhoneNumber(new PhoneNumber("1234567891"))
    .Build();

    if (userId.HasValue)
    {
      typeof(User).GetProperty("Id")!.SetValue(user, userId.Value);
    }

    return user;
  }
}