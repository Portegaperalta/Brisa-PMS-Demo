using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands;

public class UpdateHotelContactInfoUseCaseTests
{
    private readonly IHotelsRepository _repositoryMock;
    private readonly IValidator<UpdateHotelContactInfoCommand> _validatorMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateHotelContactInfoUseCase _useCase;

    public UpdateHotelContactInfoUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _validatorMock = Substitute.For<IValidator<UpdateHotelContactInfoCommand>>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateHotelContactInfoUseCase(_repositoryMock, _unitOfWorkMock, _validatorMock);
    }

    [Fact]
    public async Task Handle_UpdatesHotelContactInfo_WhenCommandIsValid()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = CreateHotel(hotelId);
        var newBusinessEmail = "newEmail@brisa.com";
        var newBusinessPhoneNumber = "+19817779999";
        var command = CreateCommand(hotelId, newBusinessEmail, newBusinessPhoneNumber);

        ArrangeSuccessfulValidation();

        _repositoryMock.GetById(hotelId).Returns(hotel);

        // Act
        await _useCase.Handle(command);

        // Assert
        hotel.BusinessEmail.Value.Should().Be(newBusinessEmail);
        hotel.BusinessPhoneNumber.Value.Should().Be(newBusinessPhoneNumber);
        await _repositoryMock.Received(1).Update(hotel);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var newInvalidEmail = "hotel.info";
        var newInvalidPhoneNumber = "a1235678910";
        var command = CreateCommand(Guid.Empty, newInvalidEmail, newInvalidPhoneNumber);
        var expectedErrors = new[]
        {
            "Field Id is required.",
            "Must be a valid email address",
            "Must be a valid phone number"
        };

        ArrangeValidationFailures(
            new ValidationFailure(nameof(UpdateHotelContactInfoCommand.HotelId), expectedErrors[0]),
            new ValidationFailure(nameof(UpdateHotelContactInfoCommand.BusinessEmail), expectedErrors[1]),
            new ValidationFailure(nameof(UpdateHotelContactInfoCommand.BusinessPhoneNumber), expectedErrors[2]));

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        var exception = await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
        exception.Which.ValidationErrors.Should().BeEquivalentTo(expectedErrors);
        await _repositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenCommandFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, "", "");
        var expectedErrors = new[]
        {
            "Field Id is required.",
            "Field Business Email is required.",
            "Field Business Phone Number is required"
        };

        ArrangeValidationFailures(
            new ValidationFailure(nameof(UpdateHotelContactInfoCommand.HotelId), expectedErrors[0]),
            new ValidationFailure(nameof(UpdateHotelContactInfoCommand.BusinessEmail), expectedErrors[1]),
            new ValidationFailure(nameof(UpdateHotelContactInfoCommand.BusinessPhoneNumber), expectedErrors[2]));

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        var exception = await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
        exception.Which.ValidationErrors.Should().BeEquivalentTo(expectedErrors);
        await _repositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_ThrowsHotelNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var command = CreateCommand(hotelId, CreateBusinessEmail(), CreateBusinessPhoneNumber());

        ArrangeSuccessfulValidation();

        _repositoryMock.GetById(hotelId).Returns((Hotel?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<HotelNotFoundException>();
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
        var command = CreateCommand(hotelId, CreateBusinessEmail(), CreateBusinessPhoneNumber());

        ArrangeSuccessfulValidation();

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
    private void ArrangeSuccessfulValidation()
    {
        _validatorMock.ValidateAsync(Arg.Any<UpdateHotelContactInfoCommand>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
    }

    private void ArrangeValidationFailures(params ValidationFailure[] validationFailures)
    {
        _validatorMock.ValidateAsync(Arg.Any<UpdateHotelContactInfoCommand>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
    }

    private static UpdateHotelContactInfoCommand CreateCommand(Guid hotelId, string newBusinessEmail,
    string newBusinessPhoneNumber)
    {
        return new UpdateHotelContactInfoCommand
        {
            HotelId = hotelId,
            BusinessEmail = newBusinessEmail,
            BusinessPhoneNumber = newBusinessPhoneNumber,
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

    private static string CreateBusinessEmail() => "hotel@brisa.com";

    private static string CreateBusinessPhoneNumber() => "+18295554321";
}