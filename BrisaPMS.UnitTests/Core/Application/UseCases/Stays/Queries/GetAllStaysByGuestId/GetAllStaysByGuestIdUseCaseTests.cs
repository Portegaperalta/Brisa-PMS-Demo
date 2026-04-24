using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByGuestId;
using BrisaPMS.Domain.Stays;
using FluentAssertions;
using NSubstitute;
using BrisaPMS.UnitTests.Core.Application.UseCases.Stays;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Queries.GetAllStaysByGuestId;

public class GetAllStaysByGuestIdUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly GetAllStaysByGuestIdUseCase _useCase;

    public GetAllStaysByGuestIdUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _useCase = new GetAllStaysByGuestIdUseCase(_staysRepositoryMock, _guestsRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfStayDtos()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var stays = new List<Stay>
        {
            StayTestData.CreateStay(stayId: Guid.NewGuid(), guestId: guestId),
            StayTestData.CreateStay(stayId: Guid.NewGuid(), guestId: guestId)
        };

        var query = new GetAllStaysByGuestIdQuery { GuestId = guestId };

        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _staysRepositoryMock.GetAllByGuestIdAsync(guestId).Returns(stays);

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
        await _guestsRepositoryMock.Received(1).Exists(guestId);
        await _staysRepositoryMock.Received(1).GetAllByGuestIdAsync(guestId);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenGuestHasNoStays()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var query = new GetAllStaysByGuestIdQuery { GuestId = guestId };

        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _staysRepositoryMock.GetAllByGuestIdAsync(guestId).Returns([]);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _guestsRepositoryMock.Received(1).Exists(guestId);
        await _staysRepositoryMock.Received(1).GetAllByGuestIdAsync(guestId);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var query = new GetAllStaysByGuestIdQuery { GuestId = guestId };

        _guestsRepositoryMock.Exists(guestId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _staysRepositoryMock.DidNotReceive().GetAllByGuestIdAsync(Arg.Any<Guid>());
    }
}
