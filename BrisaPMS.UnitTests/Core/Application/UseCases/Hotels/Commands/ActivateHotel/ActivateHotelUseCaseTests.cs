using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.ActivateHotel;

public class ActivateHotelUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ActivateHotelUseCase _useCase;

    public ActivateHotelUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new ActivateHotelUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ActivatesHotel()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId, isActive: false);
        var command = CreateCommand(hotelId);

        _repositoryMock.GetById(hotelId).Returns(hotel);

        // Act
        await _useCase.Handle(command);

        // Assert
        hotel.IsActive.Should().BeTrue();
        await _repositoryMock.Received(1).Update(hotel);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var command = CreateCommand(hotelId);

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
        var hotel = CreateHotel(hotelId, isActive: false);
        var command = CreateCommand(hotelId);

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

    private static ActivateHotelCommand CreateCommand(Guid hotelId)
    {
        return new ActivateHotelCommand
        {
            HotelId = hotelId
        };
    }

    private static Hotel CreateHotel(Guid? hotelId = null, bool isActive = true)
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
            isActive,
            new Url("https://example.com/logo.png"))
        {
            Id = hotelId ?? Guid.NewGuid()
        };
    }
}
