using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Queries;

public class GetHotelByIdUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly GetHotelByIdUseCase _useCase;

    public GetHotelByIdUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _useCase = new GetHotelByIdUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsHotelDto()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var query = new GetHotelByIdQuery() { HotelId = hotelId };
        
        _repositoryMock.GetById(hotelId).Returns(hotel);
        
        // Act
        var result = await _useCase.Handle(query);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(hotel.Id);
        result.LegalName.Should().Be(hotel.LegalName);
        result.CommercialName.Should().Be(hotel.CommercialName);
        result.LogoUrl.Should().Be(hotel.LogoUrl!.Value);
        result.BusinessEmail.Should().Be(hotel.BusinessEmail.Value);
        result.BusinessPhoneNumber.Should().Be(hotel.BusinessPhoneNumber.Value);
        result.Address1.Should().Be(hotel.Address.Address1);
        result.Address2.Should().Be(hotel.Address.Address2);
        result.City.Should().Be(hotel.Address.City);
        result.Province.Should().Be(hotel.Address.Province);
        result.ZipCode.Should().Be(hotel.Address.ZipCode);
        result.CheckInTime.Should().Be(hotel.CheckOutPolicy.CheckInTime);
        result.CheckOutTime.Should().Be(hotel.CheckOutPolicy.CheckOutTime);
        result.DefaultCurrencyCode.Should().Be(hotel.DefaultCurrencyCode.ToString());
        result.ItbisRate.Should().Be(hotel.ItbisRate.Rate);
        result.ServiceChargeRate.Should().Be(hotel.ServiceChargeRate.Rate);
        result.IsActive.Should().Be(hotel.IsActive);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var query = new GetHotelByIdQuery() { HotelId = hotelId };
        
        _repositoryMock.GetById(hotelId).ReturnsNull();
        
        // Act
        var act = async () => await _useCase.Handle(query);
        
        // Asset
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    // Helper methods
    private static Hotel CreateHotel(Guid? hotelId = null)
    {
        return new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            new Email("contact@hotelbrisa.com"),
            new PhoneNumber("+18095551234"),
            new Address("123 Main Street", "Suite 4B", "Santo Domingo", "Distrito Nacional", "10101"),
            new CheckOutPolicy(new TimeOnly(10, 0), new TimeOnly(12, 0)),
            new ItbisRate(0.18m),
            new ServiceChargeRate(0.10m),
            true,
            new Url("https://example.com/logo.png"))
        {
            Id = hotelId ?? Guid.NewGuid()
        };
    }
}