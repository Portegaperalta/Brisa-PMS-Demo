using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Amenities.Queries.GetAmenityById;
using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Domain.Amenities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Queries.GetAmenityById;

public class GetAmenityByIdUseCaseTests
{
    private readonly IAmenitiesRepository _repositoryMock;
    private readonly GetAmenityByIdUseCase _useCase;

    public GetAmenityByIdUseCaseTests()
    {
        _repositoryMock = Substitute.For<IAmenitiesRepository>();
        _useCase = new GetAmenityByIdUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsAmenityDto()
    {
        // Arrange
        var amenityId = Guid.NewGuid();
        var amenity = CreateAmenity(amenityId);
        var query = new GetAmenityByIdQuery { AmenityId = amenityId };

        _repositoryMock.GetById(amenityId).Returns(amenity);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<AmenityDto>();
        result.Id.Should().Be(amenity.Id);
        result.Name.Should().Be(amenity.Name);
        result.Description.Should().Be(amenity.Description);
        result.IsActive.Should().Be(amenity.IsActive);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenAmenityDoesNotExist()
    {
        // Arrange
        var amenityId = Guid.NewGuid();
        var query = new GetAmenityByIdQuery { AmenityId = amenityId };

        _repositoryMock.GetById(amenityId).ReturnsNull();

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private static Amenity CreateAmenity(Guid? amenityId = null, bool isActive = true)
    {
        return new Amenity("Pool Access", "Access to the swimming pool", isActive)
        {
            Id = amenityId ?? Guid.NewGuid()
        };
    }
}
