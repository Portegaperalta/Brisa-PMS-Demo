using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateHotelRatesUseCase _useCase;

    public UpdateHotelRatesUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateHotelRatesUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesHotelRates()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var newItbisRate = 0.19m;
        var newServiceChargeRate = 0.12m;
        var command = CreateCommand(hotelId, newItbisRate, newServiceChargeRate);

        _repositoryMock.GetById(hotelId).Returns(hotel);

        // Act
        await _useCase.Handle(command);

        // Assert
        hotel.ItbisRate.Rate.Should().Be(newItbisRate);
        hotel.ServiceChargeRate.Rate.Should().Be(newServiceChargeRate);
        await _repositoryMock.Received(1).Update(hotel);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var command = CreateCommand(hotelId, CreateItbisRate(), CreateServiceChargeRate());

        _repositoryMock.GetById(hotelId).Returns((Hotel?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Update(Arg.Any<Hotel>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var command = CreateCommand(hotelId, CreateItbisRate(), CreateServiceChargeRate());

        _repositoryMock.GetById(hotelId).Returns(hotel);

        _repositoryMock
            .When(repository => repository.Update(Arg.Any<Hotel>()))
            .Do(_ => throw new InvalidOperationException("Update failed"));

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    // Helper functions
    private static UpdateHotelRatesCommand CreateCommand(Guid hotelId, decimal itbisRate, decimal serviceChargeRate)
    {
        return new UpdateHotelRatesCommand
        {
            HotelId = hotelId,
            ItbisRate = itbisRate,
            ServiceChargeRate = serviceChargeRate
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

    private static decimal CreateItbisRate() => 0.18m;

    private static decimal CreateServiceChargeRate() => 0.10m;
}
