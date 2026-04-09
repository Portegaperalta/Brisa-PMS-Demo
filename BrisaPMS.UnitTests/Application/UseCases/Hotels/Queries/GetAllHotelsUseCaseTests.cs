using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Queries;

public class GetAllHotelsUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly GetAllHotelsUseCase _useCase;

    public GetAllHotelsUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _useCase = new GetAllHotelsUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfHotelsDtos()
    {
        // Arrange
        var hotels = new List<Hotel>
        {
            CreateHotel(
                Guid.NewGuid(),
                "Brisa Hospitality SRL",
                "Hotel Brisa",
                "https://example.com/logo-1.png",
                "contact@hotelbrisa.com",
                "+18095551234",
                "123 Main Street",
                "Suite 4B",
                "Santo Domingo",
                "Distrito Nacional",
                "10101",
                new TimeOnly(10, 0),
                new TimeOnly(12, 0),
                CurrencyCode.USD,
                0.18m,
                0.10m,
                true),
            CreateHotel(
                Guid.NewGuid(),
                "Costa Azul Holdings SRL",
                "Costa Azul Hotel",
                "https://example.com/logo-2.png",
                "reservations@costaazul.com",
                "+18095559876",
                "456 Ocean Avenue",
                null,
                "Punta Cana",
                "La Altagracia",
                "23000",
                new TimeOnly(15, 0),
                new TimeOnly(11, 0),
                CurrencyCode.DOP,
                0.16m,
                0.12m,
                false)
        };

        var query = new GetAllHotelsQuery();
        _repositoryMock.GetAll().Returns(hotels);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(
            hotels.Select(h => new
            {
                h.Id,
                h.LegalName,
                h.CommercialName,
                LogoUrl = h.LogoUrl!.Value,
                BusinessEmail = h.BusinessEmail.Value,
                BusinessPhoneNumber = h.BusinessPhoneNumber.Value,
                Address1 = h.Address.Address1,
                Address2 = h.Address.Address2,
                City = h.Address.City,
                Province = h.Address.Province,
                ZipCode = h.Address.ZipCode,
                CheckInTime = h.CheckOutPolicy.CheckInTime,
                CheckOutTime = h.CheckOutPolicy.CheckOutTime,
                DefaultCurrencyCode = h.DefaultCurrencyCode.ToString(),
                ItbisRate = h.ItbisRate.Rate,
                ServiceChargeRate = h.ServiceChargeRate.Rate,
                h.IsActive
            }));

        await _repositoryMock.Received(1).GetAll();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenRepositoryHasNoHotels()
    {
        // Arrange
        var query = new GetAllHotelsQuery();
        _repositoryMock.GetAll().Returns(Enumerable.Empty<Hotel>());

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _repositoryMock.Received(1).GetAll();
    }

    private static Hotel CreateHotel(
        Guid id,
        string legalName,
        string commercialName,
        string logoUrl,
        string businessEmail,
        string businessPhoneNumber,
        string address1,
        string? address2,
        string city,
        string province,
        string zipCode,
        TimeOnly checkInTime,
        TimeOnly checkOutTime,
        CurrencyCode defaultCurrencyCode,
        decimal itbisRate,
        decimal serviceChargeRate,
        bool isActive)
    {
        return new Hotel(
            legalName,
            commercialName,
            new Email(businessEmail),
            new PhoneNumber(businessPhoneNumber),
            new Address(address1, address2, city, province, zipCode),
            new CheckOutPolicy(checkInTime, checkOutTime),
            new ItbisRate(itbisRate),
            new ServiceChargeRate(serviceChargeRate),
            isActive,
            new Url(logoUrl),
            defaultCurrencyCode)
        {
            Id = id
        };
    }
}
