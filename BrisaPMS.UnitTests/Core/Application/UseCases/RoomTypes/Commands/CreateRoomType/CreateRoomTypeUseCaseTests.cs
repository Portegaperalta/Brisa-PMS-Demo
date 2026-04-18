using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeUseCaseTests
{
    private readonly IRoomTypesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateRoomTypeUseCase _useCase;

    public CreateRoomTypeUseCaseTests()
    {
        _repositoryMock = Substitute.For<IRoomTypesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateRoomTypeUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesRoomTypeAndReturnsRoomTypeId()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock.Create(Arg.Any<RoomType>())
            .Returns(callInfo => callInfo.Arg<RoomType>());

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _repositoryMock.Received(1).Create(Arg.Is<RoomType>(roomType =>
            roomType.Name == command.Name &&
            roomType.Description == command.Description &&
            roomType.BaseRate.Rate == command.BaseRate &&
            roomType.Beds.NumberOfBeds == command.TotalBeds &&
            roomType.Beds.BedType == Enum.Parse<BedType>(command.BedType) &&
            roomType.OccupancyPolicy.MaxOccupancyAdults == command.MaxOccupancyAdults &&
            roomType.OccupancyPolicy.MaxOccupancyChildren == command.MaxOccupancyChildren));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_MakesRollBack_WhenExceptionIsThrown()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock.Create(Arg.Any<RoomType>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static CreateRoomTypeCommand CreateValidCommand()
    {
        return new CreateRoomTypeCommand
        {
            Name = "Deluxe Suite",
            Description = "Spacious suite with ocean view",
            BaseRate = 25m,
            TotalBeds = 2,
            BedType = "Double",
            MaxOccupancyAdults = 2,
            MaxOccupancyChildren = 1
        };
    }
}
