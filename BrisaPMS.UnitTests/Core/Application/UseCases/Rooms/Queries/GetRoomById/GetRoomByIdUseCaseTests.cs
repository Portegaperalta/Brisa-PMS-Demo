using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetRoomById;
using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Queries.GetRoomById;

public class GetRoomByIdUseCaseTests
{
  private readonly IRoomsRepository _repositoryMock;
  private readonly GetRoomByIdUseCase _useCase;

  public GetRoomByIdUseCaseTests()
  {
    _repositoryMock = Substitute.For<IRoomsRepository>();
    _useCase = new GetRoomByIdUseCase(_repositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsRoomDto()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var room = CreateRoom(roomId);
    var query = new GetRoomByIdQuery { RoomId = roomId };

    _repositoryMock.GetById(roomId).Returns(room);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<RoomDto>();
    result.Id.Should().Be(room.Id);
    result.RoomTypeId.Should().Be(room.RoomTypeId);
    result.HotelId.Should().Be(room.HotelId);
    result.Number.Should().Be(room.Number);
    result.Floor.Should().Be(room.Floor);
    result.AvailabilityStatus.Should().Be(room.AvailabilityStatus.ToString());
    result.HygieneStatus.Should().Be(room.HygieneStatus.ToString());
    result.LastCleanedAt.Should().Be(room.LastCleanedAt);
    result.LastCleanedBy.Should().Be(room.LastCleanedBy);
    result.NeedsRestocking.Should().Be(room.NeedsRestocking);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var query = new GetRoomByIdQuery { RoomId = roomId };

    _repositoryMock.GetById(roomId).ReturnsNull();

    // Act
    var act = async () => await _useCase.Handle(query);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  private static Room CreateRoom(Guid? roomId = null)
  {
    var room = new Room
    (
        Guid.NewGuid(),
        Guid.NewGuid(),
        "201",
        2,
        RoomAvailabilityStatus.Available,
        RoomHygieneStatus.Clean
      );

    if (roomId.HasValue)
    {
      typeof(Room).GetProperty("Id")!.SetValue(room, roomId.Value);
    }

    return room;
  }
}
