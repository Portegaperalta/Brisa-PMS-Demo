using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly IValidator<UpdateHotelAddressInfoCommand> _validatorMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateHotelAddressInfoUseCase _useCase;

    public UpdateHotelAddressInfoUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _validatorMock = Substitute.For<IValidator<UpdateHotelAddressInfoCommand>>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateHotelAddressInfoUseCase(_repositoryMock, _unitOfWorkMock, _validatorMock);
    }

    [Fact]
    public async Task Handle_UpdatesHotelAddressInfo()
    {
        // Arrange
        var hotelId =  Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var newAddress1 = CreateAddress1();
        var newAddress2 = CreateAddress2();
        var newCity = CreateCity();
        var newProvince = CreateProvince();
        var newZipCode = CreateZipCode();

        var command = new UpdateHotelAddressInfoCommand
        {
            HotelId = hotelId,
            Address1 = newAddress1,
            Address2 = newAddress2,
            City = newCity,
            Province = newProvince,
            ZipCode = newZipCode
        };

        ArrangeSuccessfulValidation();
        
        _repositoryMock.GetById(hotelId).Returns(hotel);
        
        // Act
        var result = _useCase.Handle(command);
        
        // Assert
        hotel.Address.Address1.Should().Be(newAddress1);
        hotel.Address.Address2.Should().Be(newAddress2);
        hotel.Address.City.Should().Be(newCity);
        hotel.Address.Province.Should().Be(newProvince);
        hotel.Address.ZipCode.Should().Be(newZipCode);
        
        await _repositoryMock.Received(1).Update(hotel);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId =  Guid.NewGuid();
        var command = CreateUpdateHotelAddressInfoCommand
        (
            hotelId,
            CreateAddress1(), 
            CreateAddress2(),
            CreateCity(), 
            CreateProvince(), 
            CreateZipCode()
        );
        
        _repositoryMock.GetById(hotelId).Returns((Hotel?)null);
        
        // Act
        var act = async () => await _useCase.Handle(command);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Update(Arg.Any<Hotel>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }
    
    // Helper methods
    private void ArrangeSuccessfulValidation()
    {
        _validatorMock.ValidateAsync(Arg.Any<UpdateHotelAddressInfoCommand>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
    }

    private static UpdateHotelAddressInfoCommand 
        CreateUpdateHotelAddressInfoCommand(Guid hotelId, string address1, string address2, string city, 
            string province, string zipCode)
    {
        return new UpdateHotelAddressInfoCommand
        {
            HotelId = hotelId,
            Address1 = address1,
            Address2 = address2,
            City = city,
            Province = province,
            ZipCode = zipCode
        };
    }
    
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

    private static string CreateAddress1() => "223 North street";
    private static string CreateAddress2() => "Suite 2C";
    private static string CreateCity() => "Santiago";
    private static  string CreateProvince() => "Cibao";
    private static string CreateZipCode() => "105000";
}