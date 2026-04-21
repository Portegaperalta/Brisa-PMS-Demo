using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Queries.GetHotelById;

public class GetHotelByIdUseCaseTests
{
  private readonly IHotelsRepository _hotelsRepositoryMock;
  private readonly GetHotelByIdUseCase _useCase;

  public GetHotelByIdUseCaseTests()
  {
    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
    _useCase = new GetHotelByIdUseCase(_hotelsRepositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsHotelDto()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var hotel = CreateHotel(hotelId);
    var query = new GetHotelByIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.GetById(hotelId).Returns(hotel);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(hotel.Id);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var query = new GetHotelByIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.GetById(hotelId).ReturnsNull();

    // Act
    var act = async () => await _useCase.Handle(query);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task Handle_CallsHotelsRepository()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var hotel = CreateHotel(hotelId);
    var query = new GetHotelByIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.GetById(hotelId).Returns(hotel);

    // Act
    await _useCase.Handle(query);

    // Assert
    await _hotelsRepositoryMock.Received(1).GetById(hotelId);
  }

  private static Hotel CreateHotel(Guid? hotelId = null)
  {
    var hotel = new Hotel(
        "Grand Plaza Hotels SRL",
        "Grand Plaza Hotel",
        new Rnc("12345678901"),
        new Email("info@grandplaza.com"),
        new PhoneNumber("+1-555-123-4567"),
        new Address("123 Main Street", null, "New York", "NY", "10001"),
        new CheckOutPolicy(new TimeOnly(15, 0), new TimeOnly(11, 0)),
        new ItbisRate(0.18m),
        new ServiceChargeRate(0.10m),
        true,
        new Url("https://example.com/logo.png"),
        CurrencyCode.USD);

    if (hotelId.HasValue)
    {
      typeof(Hotel).GetProperty("Id")!.SetValue(hotel, hotelId.Value);
    }

    return hotel;
  }
}