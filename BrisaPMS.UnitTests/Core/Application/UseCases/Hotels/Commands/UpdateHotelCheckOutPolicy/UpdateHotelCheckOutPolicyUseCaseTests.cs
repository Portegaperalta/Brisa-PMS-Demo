using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyUseCaseTests
{
  private readonly IHotelsRepository _repositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly UpdateHotelCheckOutPolicyUseCase _useCase;

  public UpdateHotelCheckOutPolicyUseCaseTests()
  {
    _repositoryMock = Substitute.For<IHotelsRepository>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new UpdateHotelCheckOutPolicyUseCase(_repositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_UpdatesHotelCheckOutPolicy()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var hotel = CreateHotel(hotelId);
    var newCheckInTime = new TimeOnly(11, 0);
    var newCheckOutTime = new TimeOnly(13, 0);
    var command = CreateCommand(hotelId, newCheckInTime, newCheckOutTime);

    _repositoryMock.GetById(hotelId).Returns(hotel);

    // Act
    await _useCase.Handle(command);

    // Assert
    hotel.CheckOutPolicy.CheckInTime.Should().Be(newCheckInTime);
    hotel.CheckOutPolicy.CheckOutTime.Should().Be(newCheckOutTime);
    await _repositoryMock.Received(1).Update(hotel);
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var command = CreateCommand(hotelId, CreateCheckInTime(), CreateCheckOutTime());

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
    var command = CreateCommand(hotelId, CreateCheckInTime(), CreateCheckOutTime());

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
  private static UpdateHotelCheckOutPolicyCommand CreateCommand(
      Guid hotelId,
      TimeOnly checkInTime,
      TimeOnly checkOutTime)
  {
    return new UpdateHotelCheckOutPolicyCommand
    {
      HotelId = hotelId,
      CheckInTime = checkInTime,
      CheckOutTime = checkOutTime
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

  private static TimeOnly CreateCheckInTime() => new(11, 0);

  private static TimeOnly CreateCheckOutTime() => new(13, 0);
}
