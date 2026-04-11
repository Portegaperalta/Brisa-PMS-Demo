using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetRoomById;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Queries;

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
    var query = new GetRoomByIdQuery() { RoomId = roomId };

    _repositoryMock.GetById(roomId).Returns(room);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(room.Id);
    result.HotelId.Should().Be(room.HotelId);
    result.Number.Should().Be(room.Number);
    result.Floor.Should().Be(room.Floor);
    result.Type.Should().Be(room.RoomType.Name);
    result.TotalBeds.Should().Be(room.RoomType.TotalBeds);
    result.MaxOccupancyAdults.Should().Be(room.RoomType.MaxOccupancyAdults);
    result.MaxOccupancyChildren.Should().Be(room.RoomType.MaxOccupancyChildren);
    result.BaseRate.Should().Be(room.RoomType.BaseRate);
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
    var query = new GetRoomByIdQuery() { RoomId = roomId };

    _repositoryMock.GetById(roomId).ReturnsNull();

    // Act
    var act = async () => await _useCase.Handle(query);

    // Asset
    await act.Should().ThrowAsync<NotFoundException>();

  }

  // Helper methods
  private static Room CreateRoom(Guid? roomId = null)
  {
    var room = new Room(
        Guid.NewGuid(),
        "201",
        2,
        RoomAvailabilityStatus.Available,
        RoomHygieneStatus.Clean,
        CreateRoomType());

    if (roomId.HasValue)
    {
      typeof(Room).GetProperty("Id")!.SetValue(room, roomId.Value);
    }

    return room;
  }

  private static RoomType CreateRoomType(string name = "Deluxe Suite")
  {
    return new RoomType(name, 25m, 2, BedType.Queen, 2, 1, "Spacious suite with ocean view");
  }
}
