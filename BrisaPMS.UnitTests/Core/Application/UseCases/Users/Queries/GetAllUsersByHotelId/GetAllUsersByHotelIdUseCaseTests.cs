using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Queries.GetAllUsersByHotelId;
using BrisaPMS.Domain.Users;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Queries.GetAllUsersByHotelId;

public class GetAllUsersByHotelIdUseCaseTests
{
  private readonly IUsersRepository _usersRepositoryMock;
  private readonly IHotelsRepository _hotelsRepositoryMock;
  private readonly GetAllUsersByHotelIdUseCase _useCase;

  public GetAllUsersByHotelIdUseCaseTests()
  {
    _usersRepositoryMock = Substitute.For<IUsersRepository>();
    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
    _useCase = new GetAllUsersByHotelIdUseCase(_usersRepositoryMock, _hotelsRepositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsListOfUserDtos()
  {
    var hotelId = Guid.NewGuid();
    var users = new List<User>
        {
            CreateUser(Guid.NewGuid(), hotelId, "John", "Doe", UserRole.Manager),
            CreateUser(Guid.NewGuid(), hotelId, "Jane", "Smith", UserRole.Receptionist)
        };
    var query = new GetAllUsersByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _usersRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns(users);

    var result = await _useCase.Handle(query);

    result.Should().HaveCount(2);
    result.Should().BeEquivalentTo(
        users.Select(user => new
        {
          user.Id,
          user.HotelId,
          user.FirstName,
          user.LastName,
          Email = user.Email.Value,
          PhoneNumber = user.PhoneNumber?.Value,
          PreferredLanguage = user.PreferredLanguage.ToString(),
          Role = user.Role.ToString()
        }));
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
    await _usersRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
  }

  [Fact]
  public async Task Handle_ReturnsEmptyList_WhenHotelHasNoUsers()
  {
    var hotelId = Guid.NewGuid();
    var query = new GetAllUsersByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _usersRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns([]);

    var result = await _useCase.Handle(query);

    result.Should().NotBeNull();
    result.Should().BeEmpty();
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
    await _usersRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    var hotelId = Guid.NewGuid();
    var query = new GetAllUsersByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(false);

    var act = async () => await _useCase.Handle(query);

    await act.Should().ThrowAsync<NotFoundException>();
    await _usersRepositoryMock.DidNotReceive().GetAllByHotelIdAsync(Arg.Any<Guid>());
  }

  private static User CreateUser(Guid userId, Guid hotelId, string firstName, string lastName, UserRole role)
  {
    var user = new User.Builder
    (
      role,
      firstName,
      lastName,
      new Email($"{firstName.ToLower()}.{lastName.ToLower()}@example.com"),
      new Password("Hashed#password1"),
      UserPreferredLanguage.En)
      .WithHotelId(hotelId)
      .WithPhoneNumber(new PhoneNumber("+18095551234"))
      .Build();

    typeof(User).GetProperty("Id")!.SetValue(user, userId);

    return user;
  }
}