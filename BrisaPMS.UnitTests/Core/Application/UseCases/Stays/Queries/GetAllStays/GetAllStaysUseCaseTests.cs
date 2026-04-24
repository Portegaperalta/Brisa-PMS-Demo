using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStays;
using BrisaPMS.Domain.Stays;
using FluentAssertions;
using NSubstitute;
using BrisaPMS.UnitTests.Core.Application.UseCases.Stays;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Queries.GetAllStays;

public class GetAllStaysUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly GetAllStaysUseCase _useCase;

    public GetAllStaysUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _useCase = new GetAllStaysUseCase(_staysRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfStayDtos()
    {
        // Arrange
        var stays = new List<Stay>
        {
            StayTestData.CreateStay(stayId: Guid.NewGuid()),
            StayTestData.CreateStay(stayId: Guid.NewGuid())
        };
        var query = new GetAllStaysQuery();

        _staysRepositoryMock.GetAll().Returns(stays);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(
            stays.Select(stay => new
            {
                stay.Id,
                stay.GuestId,
                stay.BookingId,
                ActualCheckIn = stay.TimeInterval.ActualCheckIn,
                ActualCheckOut = stay.TimeInterval.ActualCheckOut,
                stay.NightCount,
                Status = stay.Status.ToString()
            }));

        await _staysRepositoryMock.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoStaysExist()
    {
        // Arrange
        var query = new GetAllStaysQuery();

        _staysRepositoryMock.GetAll().Returns(Enumerable.Empty<Stay>());

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _staysRepositoryMock.Received(1).GetAll();
    }
}
