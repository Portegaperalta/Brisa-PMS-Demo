using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands;

public class UpdateHotelBrandInfoUseCaseTests
{
  private readonly IHotelsRepository _repositoryMock;
  private readonly IValidator<UpdateHotelBrandInfoCommand> _validatorMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly UpdateHotelBrandInfoUseCase _useCase;

  public UpdateHotelBrandInfoUseCaseTests()
  {
    _repositoryMock = Substitute.For<IHotelsRepository>();
    _validatorMock = Substitute.For<IValidator<UpdateHotelBrandInfoCommand>>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new UpdateHotelBrandInfoUseCase(_repositoryMock, _unitOfWorkMock, _validatorMock);
  }

  [Fact]
  public async Task Handle_UpdatesHotelBrandInfo_WhenCommandIsValid()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var hotel = CreateHotel(hotelId);
    var newLegalName = "Brisa Resorts S.R.L";
    var newCommercialName = "Brisa Beach Hotel";
    var newLogoUrl = "https://example.com/new-logo.png";
    var command = CreateCommand(hotelId, newLegalName, newCommercialName, newLogoUrl);

    ArrangeSuccessfulValidation();

    _repositoryMock.GetById(hotelId).Returns(hotel);

    // Act
    await _useCase.Handle(command);

    // Assert
    hotel.LegalName.Should().Be(newLegalName);
    hotel.CommercialName.Should().Be(newCommercialName);
    hotel.LogoUrl!.Value.Should().Be(newLogoUrl);

    await _repositoryMock.Received(1).Update(hotel);
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_ThrowsValidationException_WhenCommandUrlFieldIsInvalid()
  {
    // Arrange
    var invalidLogoUrl = "newlogo.png";
    var command = CreateCommand(Guid.Empty, CreateLegalName(), CreateCommercialName(), invalidLogoUrl);
    var expectedErrors = new[]
    {
      "A valid HTTP/HTTPS URL is required",
    };

    ArrangeValidationFailures(new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.LogoUrl), expectedErrors[0]));

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    var exception = await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
    exception.Which.ValidationErrors.Should().BeEquivalentTo(expectedErrors);
    await _repositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  [Fact]
  public async Task Handle_ThrowsValidationException_WhenCommandFiledsAreEmpty()
  {
    // Arrange
    var command = CreateCommand(Guid.Empty, string.Empty, string.Empty, null);
    var expectedErrors = new[]
    {
      "Field Id is required",
      "Field LegalName is required",
      "Field Commercial Name is required",
    };

    ArrangeValidationFailures(
        new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.HotelId), expectedErrors[0]),
        new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.LegalName), expectedErrors[1]),
        new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.CommercialName), expectedErrors[2]));

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    var exception = await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
    exception.Which.ValidationErrors.Should().BeEquivalentTo(expectedErrors);
    await _repositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  [Fact]
  public async Task Handle_ThrowsValidationException_WhenCommandFieldsExceedCharacterLimit()
  {
    // Arrange
    var command = CreateCommand
    (
      Guid.NewGuid(),
      new string('L', 251),
      new string('C', 251),
      $"https://example.com/{new string('L', 2048)}.png"
    );

    var expectedErrors = new[]
    {
      "Field Legal Name must not exceed 250 characters",
      "Field Commercial Name must not exceed 250 characters",
      "Field Logo Url must not exceed 2048 characters",
    };

    ArrangeValidationFailures(
        new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.LegalName), expectedErrors[0]),
        new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.CommercialName), expectedErrors[1]),
        new ValidationFailure(nameof(UpdateHotelBrandInfoCommand.LogoUrl), expectedErrors[2]));

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
    var command = CreateCommand(hotelId, CreateLegalName(), CreateCommercialName(), CreateLogoUrl());

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
    var command = CreateCommand(hotelId, CreateLegalName(), CreateCommercialName(), CreateLogoUrl());

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
    _validatorMock.ValidateAsync(Arg.Any<UpdateHotelBrandInfoCommand>(), Arg.Any<CancellationToken>())
        .Returns(new ValidationResult());
  }

  private void ArrangeValidationFailures(params ValidationFailure[] validationFailures)
  {
    _validatorMock.ValidateAsync(Arg.Any<UpdateHotelBrandInfoCommand>(), Arg.Any<CancellationToken>())
        .Returns(new ValidationResult(validationFailures));
  }

  private static UpdateHotelBrandInfoCommand CreateCommand(Guid hotelId, string legalName,
      string commercialName, string? logoUrl)
  {
    return new UpdateHotelBrandInfoCommand
    {
      HotelId = hotelId,
      LegalName = legalName,
      CommercialName = commercialName,
      LogoUrl = logoUrl,
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

  private static string CreateLegalName() => "Brisa Hospitality SRL";

  private static string CreateCommercialName() => "Hotel Brisa";

  private static string CreateLogoUrl() => "https://example.com/logo.png";
}
