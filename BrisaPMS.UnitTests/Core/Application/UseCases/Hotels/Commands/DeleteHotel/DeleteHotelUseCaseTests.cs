using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.DeleteHotel;

public class DeleteHotelUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteHotelUseCase _useCase;

    public DeleteHotelUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new DeleteHotelUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_DeletesHotel()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var command = new DeleteHotelCommand { Id = hotelId };

        _repositoryMock.GetById(hotelId).Returns(hotel);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _repositoryMock.Received(1).Delete(hotel);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var command = new DeleteHotelCommand { Id = Guid.NewGuid() };

        _repositoryMock.GetById(command.Id).Returns((Hotel?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Delete(Arg.Any<Hotel>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryDeleteFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var command = new DeleteHotelCommand { Id = hotelId };

        _repositoryMock.GetById(hotelId).Returns(hotel);
        _repositoryMock.Delete(Arg.Any<Hotel>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static Hotel CreateHotel(Guid? hotelId = null, bool isActive = true)
    {
        return new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            new Rnc("12345678901"),
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