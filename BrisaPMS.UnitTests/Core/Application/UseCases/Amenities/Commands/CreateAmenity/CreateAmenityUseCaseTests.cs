using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;
using BrisaPMS.Domain.Amenities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.CreateAmenity;

public class CreateAmenityUseCaseTests
{
    private readonly IAmenitiesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateAmenityUseCase _useCase;

    public CreateAmenityUseCaseTests()
    {
        _repositoryMock = Substitute.For<IAmenitiesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateAmenityUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesAmenityAndReturnsAmenityDto()
    {
        // Arrange
        var command = CreateCommand("Pool Access", "Access to the swimming pool", true);
        Amenity? createdAmenity = null;

        _repositoryMock
            .Create(Arg.Do<Amenity>(amenity => createdAmenity = amenity))
            .Returns(callInfo => callInfo.Arg<Amenity>());

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _repositoryMock.Received(1).Create(Arg.Is<Amenity>(amenity =>
            amenity.Name == command.Name &&
            amenity.Description == command.Description &&
            amenity.IsActive == command.IsActive));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();

        result.Id.Should().Be(createdAmenity!.Id);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.IsActive.Should().Be(command.IsActive);
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
    {
        // Arrange
        var command = CreateCommand("Pool Access", "Access to the swimming pool", true);

        _repositoryMock.Create(Arg.Any<Amenity>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static CreateAmenityCommand CreateCommand(string name, string description, bool isActive)
    {
        return new CreateAmenityCommand
        {
            Name = name,
            Description = description,
            IsActive = isActive
        };
    }
}
