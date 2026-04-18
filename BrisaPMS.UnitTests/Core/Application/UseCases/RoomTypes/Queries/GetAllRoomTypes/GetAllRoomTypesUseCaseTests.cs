using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;

public class GetAllRoomTypesUseCaseTests
{
    private readonly IRoomTypesRepository _repositoryMock;
    private readonly GetAllRoomTypesUseCase _useCase;

    public GetAllRoomTypesUseCaseTests()
    {
        _repositoryMock = Substitute.For<IRoomTypesRepository>();
        _useCase = new GetAllRoomTypesUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfRoomTypeDtos()
    {
        // Arrange
        var roomTypes = new List<RoomType>
        {
            CreateRoomType(Guid.NewGuid(), "Deluxe Suite", "Ocean view room", 25m, BedType.Double, 2, 2, 1),
            CreateRoomType(Guid.NewGuid(), "Family Room", "Spacious room for families", 35m, BedType.Queen, 3, 4, 2)
        };
        var query = new GetAllRoomTypesQuery();

        _repositoryMock.GetAll().Returns(roomTypes);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(
            roomTypes.Select(roomType => new
            {
                roomType.Id,
                roomType.Name,
                roomType.Description,
                BaseRate = roomType.BaseRate.Rate,
                NumberOfBeds = roomType.Beds.NumberOfBeds,
                BedType = roomType.Beds.BedType.ToString(),
                roomType.OccupancyPolicy.MaxOccupancyAdults,
                roomType.OccupancyPolicy.MaxOccupancyChildren
            }));

        await _repositoryMock.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenRepositoryHasNoRoomTypes()
    {
        // Arrange
        var query = new GetAllRoomTypesQuery();

        _repositoryMock.GetAll().Returns(Enumerable.Empty<RoomType>());

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _repositoryMock.Received(1).GetAll();
    }

    private static RoomType CreateRoomType(
        Guid id,
        string name,
        string? description,
        decimal baseRate,
        BedType bedType,
        int numberOfBeds,
        int maxOccupancyAdults,
        int maxOccupancyChildren)
    {
        return new RoomType(
            name,
            new RoomBaseRate(baseRate),
            new RoomBed(bedType, numberOfBeds),
            new OccupancyPolicy(maxOccupancyAdults, maxOccupancyChildren),
            description)
        {
            Id = id
        };
    }
}
