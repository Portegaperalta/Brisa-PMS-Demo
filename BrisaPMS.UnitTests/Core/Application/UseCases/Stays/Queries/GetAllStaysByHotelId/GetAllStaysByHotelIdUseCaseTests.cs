using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByHotelId;
using BrisaPMS.Domain.Stays;
using FluentAssertions;
using NSubstitute;
using BrisaPMS.UnitTests.Core.Application.UseCases.Stays;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Queries.GetAllStaysByHotelId;

public class GetAllStaysByHotelIdUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly IHotelsRepository _hotelsRepositoryMock;
    private readonly GetAllStaysByHotelIdUseCase _useCase;

    public GetAllStaysByHotelIdUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
        _useCase = new GetAllStaysByHotelIdUseCase(_staysRepositoryMock, _hotelsRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfStayDtos()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var stays = new List<Stay>
        {
            StayTestData.CreateStay(stayId: Guid.NewGuid()),
            StayTestData.CreateStay(stayId: Guid.NewGuid())
        };
        var query = new GetAllStaysByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _staysRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns(stays);

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
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _staysRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenHotelHasNoStays()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var query = new GetAllStaysByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _staysRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns([]);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _staysRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var query = new GetAllStaysByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _staysRepositoryMock.DidNotReceive().GetAllByHotelIdAsync(Arg.Any<Guid>());
    }
}
