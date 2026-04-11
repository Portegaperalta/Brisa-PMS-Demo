using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRooms;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Queries;

public class GetAllRoomsUseCaseTests
{
    private readonly IRoomsRepository _repositoryMock;
    private readonly GetAllRoomsUseCase _useCase;

    public GetAllRoomsUseCaseTests()
    {
        _repositoryMock = Substitute.For<IRoomsRepository>();
        _useCase = new GetAllRoomsUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfRoomDtos()
    {
        // Arrange
        var rooms = new List<Room>
        {
            CreateRoom("101"),
            CreateRoom("102"),
            CreateRoom("103")
        };
        var query = new GetAllRoomsQuery();

        _repositoryMock.GetAll().Returns(rooms);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Number.Should().Be("101");
        result[1].Number.Should().Be("102");
        result[2].Number.Should().Be("103");
    }

    // Helper methods
    private static Room CreateRoom(string roomNumber)
    {
        return new Room(
            Guid.NewGuid(),
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
