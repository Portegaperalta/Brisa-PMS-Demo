using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Queries.GetStayById;
using BrisaPMS.Application.UseCases.Stays.Shared;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Queries.GetStayById;

public class GetStayByIdUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly GetStayByIdUseCase _useCase;

    public GetStayByIdUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _useCase = new GetStayByIdUseCase(_staysRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsStayDto()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var stay = StayTestData.CreateStay(stayId: stayId);
        var query = new GetStayByIdQuery { StayId = stayId };

        _staysRepositoryMock.GetById(stayId).Returns(stay);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<StayDto>();
        result.Id.Should().Be(stay.Id);
        result.GuestId.Should().Be(stay.GuestId);
        result.BookingId.Should().Be(stay.BookingId);
        result.ActualCheckIn.Should().Be(stay.TimeInterval.ActualCheckIn);
        result.ActualCheckOut.Should().Be(stay.TimeInterval.ActualCheckOut);
        result.NightCount.Should().Be(stay.NightCount);
        result.Status.Should().Be(stay.Status.ToString());
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenStayDoesNotExist()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var query = new GetStayByIdQuery { StayId = stayId };

        _staysRepositoryMock.GetById(stayId).ReturnsNull();

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_CallsStaysRepository()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var stay = StayTestData.CreateStay(stayId: stayId);
        var query = new GetStayByIdQuery { StayId = stayId };

        _staysRepositoryMock.GetById(stayId).Returns(stay);

        // Act
        await _useCase.Handle(query);

        // Assert
        await _staysRepositoryMock.Received(1).GetById(stayId);
    }
}
