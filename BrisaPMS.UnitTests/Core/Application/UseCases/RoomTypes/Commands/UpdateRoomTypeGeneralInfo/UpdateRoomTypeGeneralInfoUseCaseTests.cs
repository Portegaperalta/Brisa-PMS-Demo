using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;

public class UpdateRoomTypeGeneralInfoUseCaseTests
{
    private readonly IRoomTypesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateRoomTypeGeneralInfoUseCase _useCase;

    public UpdateRoomTypeGeneralInfoUseCaseTests()
    {
        _repositoryMock = Substitute.For<IRoomTypesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateRoomTypeGeneralInfoUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesRoomTypeGeneralInfo()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var command = CreateCommand(roomTypeId, "Premium Suite", "Updated description");
        var roomType = CreateRoomType(roomTypeId);

        _repositoryMock.GetById(roomTypeId).Returns(roomType);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        roomType.Name.Should().Be(command.Name);
        roomType.Description.Should().Be(command.Description);
        await _repositoryMock.Received(1).Update(roomType);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UpdatesOnlyName_WhenDescriptionIsNull()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var roomType = CreateRoomType(roomTypeId);
        var originalDescription = roomType.Description;
        var command = CreateCommand(roomTypeId, "Premium Suite", null);

        _repositoryMock.GetById(roomTypeId).Returns(roomType);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        roomType.Name.Should().Be(command.Name);
        roomType.Description.Should().Be(originalDescription);
        await _repositoryMock.Received(1).Update(roomType);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenRoomTypeDoesNotExist()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Premium Suite", "Updated description");

        _repositoryMock.GetById(command.RoomTypeId).Returns((RoomType?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Update(Arg.Any<RoomType>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var command = CreateCommand(roomTypeId, "Premium Suite", "Updated description");
        var roomType = CreateRoomType(roomTypeId);

        _repositoryMock.GetById(roomTypeId).Returns(roomType);
        _repositoryMock.Update(Arg.Any<RoomType>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateRoomTypeGeneralInfoCommand CreateCommand(Guid roomTypeId, string name, string? description)
    {
        return new UpdateRoomTypeGeneralInfoCommand
        {
            RoomTypeId = roomTypeId,
            Name = name,
            Description = description
        };
    }

    private static RoomType CreateRoomType(Guid? roomTypeId = null)
    {
        return new RoomType(
            "Deluxe Suite",
            new RoomBaseRate(25m),
            new RoomBed(BedType.Double, 2),
            new OccupancyPolicy(2, 1),
            "Spacious suite with ocean view")
        {
            Id = roomTypeId ?? Guid.NewGuid()
        };
    }
}
