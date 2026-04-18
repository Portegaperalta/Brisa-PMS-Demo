using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;

public class GetRoomTypeByIdUseCaseTests
{
    private readonly IRoomTypesRepository _repositoryMock;
    private readonly GetRoomTypeByIdUseCase _useCase;

    public GetRoomTypeByIdUseCaseTests()
    {
        _repositoryMock = Substitute.For<IRoomTypesRepository>();
        _useCase = new GetRoomTypeByIdUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsRoomTypeDto()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var roomType = CreateRoomType(roomTypeId);
        var query = new GetRoomTypeByIdQuery { RoomTypeId = roomTypeId };

        _repositoryMock.GetById(roomTypeId).Returns(roomType);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(roomType.Id);
        result.Name.Should().Be(roomType.Name);
        result.Description.Should().Be(roomType.Description);
        result.BaseRate.Should().Be(roomType.BaseRate.Rate);
        result.NumberOfBeds.Should().Be(roomType.Beds.NumberOfBeds);
        result.BedType.Should().Be(roomType.Beds.BedType.ToString());
        result.MaxOccupancyAdults.Should().Be(roomType.OccupancyPolicy.MaxOccupancyAdults);
        result.MaxOccupancyChildren.Should().Be(roomType.OccupancyPolicy.MaxOccupancyChildren);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenRoomTypeDoesNotExist()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var query = new GetRoomTypeByIdQuery { RoomTypeId = roomTypeId };

        _repositoryMock.GetById(roomTypeId).ReturnsNull();

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private static RoomType CreateRoomType(Guid id)
    {
        return new RoomType(
            "Deluxe Suite",
            new RoomBaseRate(25m),
            new RoomBed(BedType.Double, 2),
            new OccupancyPolicy(2, 1),
            "Spacious suite with ocean view")
        {
            Id = id
        };
    }
}
