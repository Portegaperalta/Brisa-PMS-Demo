using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Amenities.Queries.GetAllAmenities;
using BrisaPMS.Domain.Amenities;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Queries.GetAllAmenities;

public class GetAllAmenitiesUseCaseTests
{
    private readonly IAmenitiesRepository _repositoryMock;
    private readonly GetAllAmenitiesUseCase _useCase;

    public GetAllAmenitiesUseCaseTests()
    {
        _repositoryMock = Substitute.For<IAmenitiesRepository>();
        _useCase = new GetAllAmenitiesUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfAmenityDtos()
    {
        // Arrange
        var amenities = new List<Amenity>
        {
            CreateAmenity(Guid.NewGuid(), "Pool Access", "Access to the swimming pool", true),
            CreateAmenity(Guid.NewGuid(), "Gym Access", "Access to the gym area", false)
        };
        var query = new GetAllAmenitiesQuery();

        _repositoryMock.GetAll().Returns(amenities);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(
            amenities.Select(amenity => new
            {
                amenity.Id,
                amenity.Name,
                amenity.Description,
                amenity.IsActive
            }));

        await _repositoryMock.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenRepositoryHasNoAmenities()
    {
        // Arrange
        var query = new GetAllAmenitiesQuery();

        _repositoryMock.GetAll().Returns(Enumerable.Empty<Amenity>());

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _repositoryMock.Received(1).GetAll();
    }

    private static Amenity CreateAmenity(Guid id, string name, string description, bool isActive)
    {
        return new Amenity(name, description, isActive)
        {
            Id = id
        };
    }
}
