using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;
using BrisaPMS.Domain.Amenities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;

public class UpdateAmenityDetailsUseCaseTests
{
    private readonly IAmenitiesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateAmenityDetailsUseCase _useCase;

    public UpdateAmenityDetailsUseCaseTests()
    {
        _repositoryMock = Substitute.For<IAmenitiesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateAmenityDetailsUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesAmenityDetails()
    {
        // Arrange
        var amenityId = Guid.NewGuid();
        var amenity = CreateAmenity(amenityId);
        var command = CreateCommand(amenityId, "Gym Access", "Access to the gym area");

        _repositoryMock.GetById(amenityId).Returns(amenity);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        amenity.Name.Should().Be(command.Name);
        amenity.Description.Should().Be(command.Description);
        await _repositoryMock.Received(1).Update(amenity);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenAmenityDoesNotExist()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Gym Access", "Access to the gym area");

        _repositoryMock.GetById(command.AmenityId).Returns((Amenity?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Update(Arg.Any<Amenity>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        // Arrange
        var amenityId = Guid.NewGuid();
        var amenity = CreateAmenity(amenityId);
        var command = CreateCommand(amenityId, "Gym Access", "Access to the gym area");

        _repositoryMock.GetById(amenityId).Returns(amenity);
        _repositoryMock.Update(Arg.Any<Amenity>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateAmenityDetailsCommand CreateCommand(Guid amenityId, string name, string description)
    {
        return new UpdateAmenityDetailsCommand
        {
            AmenityId = amenityId,
            Name = name,
            Description = description
        };
    }

    private static Amenity CreateAmenity(Guid? amenityId = null, bool isActive = true)
    {
        return new Amenity("Pool Access", "Access to the swimming pool", isActive)
        {
            Id = amenityId ?? Guid.NewGuid()
        };
    }
}
