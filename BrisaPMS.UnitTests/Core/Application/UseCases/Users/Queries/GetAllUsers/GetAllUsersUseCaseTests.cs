using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Users.Queries.GetAllUsers;
using BrisaPMS.Domain.Users;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersUseCaseTests
{
    private readonly IUsersRepository _repositoryMock;
    private readonly GetAllUsersUseCase _useCase;

    public GetAllUsersUseCaseTests()
    {
        _repositoryMock = Substitute.For<IUsersRepository>();
        _useCase = new GetAllUsersUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            CreateUser(Guid.NewGuid(), "John", "Doe", UserRole.Manager),
            CreateUser(Guid.NewGuid(), "Jane", "Smith", UserRole.Receptionist)
        };
        var query = new GetAllUsersQuery();
        _repositoryMock.GetAll().Returns(users);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
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

        await _repositoryMock.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenRepositoryHasNoUsers()
    {
        // Arrange
        var query = new GetAllUsersQuery();
        _repositoryMock.GetAll().Returns(Enumerable.Empty<User>());

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _repositoryMock.Received(1).GetAll();
    }

    private static User CreateUser(Guid userId, string firstName, string lastName, UserRole role)
    {
        var user = new User.Builder(
                role,
                firstName,
                lastName,
                new Email($"{firstName.ToLower()}.{lastName.ToLower()}@example.com"),
                UserPreferredLanguage.En)
            .WithHotelId(Guid.NewGuid())
            .WithPhoneNumber(new PhoneNumber("+18095551234"))
            .Build();

        typeof(User).GetProperty("Id")!.SetValue(user, userId);

        return user;
    }
}
