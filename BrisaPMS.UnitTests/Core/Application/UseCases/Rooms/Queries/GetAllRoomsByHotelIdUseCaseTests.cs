using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRoomsByHotelId;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Queries;

public class GetAllRoomsByHotelIdUseCaseTests
{
  private readonly IRoomsRepository _roomsRepositoryMock;
  private readonly IHotelsRepository _hotelsRepositoryMock;
  private readonly GetAllRoomsByHotelIdUseCase _useCase;

  public GetAllRoomsByHotelIdUseCaseTests()
  {
    _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
    _useCase = new GetAllRoomsByHotelIdUseCase(_roomsRepositoryMock, _hotelsRepositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsListOfRoomDtos_WhenHotelExists()
  {
    // Arrange
    var hotelId = Guid.NewGuid();

    var rooms = new List<Room>
        {
            CreateRoom(hotelId, "101"),
            CreateRoom(hotelId, "102"),
            CreateRoom(hotelId, "103")
        };

    var query = new GetAllRoomsByHotelIdQuery() { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _roomsRepositoryMock.GetAllByHotelId(hotelId).Returns(rooms);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(3);
    result[0].Number.Should().Be("101");
    result[1].Number.Should().Be("102");
    result[2].Number.Should().Be("103");
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var query = new GetAllRoomsByHotelIdQuery() { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(query);

    // Asset
    await act.Should().ThrowAsync<NotFoundException>();
  }

  // Helper methods
  private static Room CreateRoom(Guid hotelId, string roomNumber)
  {
    return new Room(
        hotelId,
        roomNumber,
        1,
        RoomAvailabilityStatus.Available,
        RoomHygieneStatus.Clean,
        CreateRoomType());
  }

  private static RoomType CreateRoomType(string name = "Deluxe Suite")
  {
    return new RoomType(
        name,
        25m,
        2,
        BedType.Queen,
        2,
        1,
        "Spacious suite with ocean view");
  }
}
