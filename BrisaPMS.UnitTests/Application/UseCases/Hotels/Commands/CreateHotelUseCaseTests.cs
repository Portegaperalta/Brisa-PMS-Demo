using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using BrisaPMS.Domain.Hotels;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands;

public class CreateHotelUseCaseTests
{
    private IHotelsRepository _repositoryMock;
    private IValidator<CreateHotelCommand> _validatorMock;
    private IUnitOfWork _unitOfWorkMock;
    private CreateHotelUseCase _useCase;

    public CreateHotelUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _validatorMock = Substitute.For<IValidator<CreateHotelCommand>>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateHotelUseCase(_repositoryMock, _unitOfWorkMock, _validatorMock);
    }

    [Fact]
    public async Task Handle_ReturnsHotelId_WhenCommandIsValid()
    {
        // Arrange
        var createHotelCommand = new CreateHotelCommand
        {
            LegalName = "Brisa Hotel S.R.L",
            CommercialName = "Brisa Hotel",
            LogoUrl = "https://testlogourl.jpg",
            BusinessEmail = "brisaHotel@test.com",
            BusinessPhoneNumber = "1234567891",
            Address1 = "test address 1",
            Address2 = "test address 2",
            City = "test city",
            Province = "test province",
            ZipCode = "12345",
            CheckInTime = new TimeOnly(12, 0, 0),
            CheckOutTime = new TimeOnly(15, 0, 0),
            DefaultCurrencyCode = "USD",
            ItbisRate = 0.18m,
            ServiceChargeRate = 0.10m,
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateHotelCommand>(), Arg.Any<CancellationToken>())
                      .Returns(new ValidationResult());

        _repositoryMock.Create(Arg.Any<Hotel>())
                       .Returns(callInfo => callInfo.Arg<Hotel>());

        // Act
        var result = await _useCase.Handle(createHotelCommand);

        // Assert
        await _repositoryMock.Received(1).Create(Arg.Any<Hotel>());
        await _unitOfWorkMock.Received(1).Persist();
        result.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [MemberData(nameof(GetRequiredFieldValidationCases))]
    public async Task Handle_ThrowsValidationException_WhenRequiredFieldIsEmpty(
        Action<CreateHotelCommand> arrangeInvalidField,
        string propertyName,
        string errorMessage)
    {
        // Arrange
        var createHotelCommand = CreateValidCommand();
        arrangeInvalidField(createHotelCommand);

        var validationFailures = new List<ValidationFailure>
        {
            new(propertyName, errorMessage)
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateHotelCommand>(), Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        // Act
        var act = async () => await _useCase.Handle(createHotelCommand);

        // Assert
        await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
    }

    [Theory]
    [MemberData(nameof(GetLengthValidationCases))]
    public async Task Handle_ThrowsValidationException_WhenStringFieldExceedsMaxLength(
        Action<CreateHotelCommand> arrangeInvalidField,
        string propertyName,
        string errorMessage)
    {
        // Arrange
        var createHotelCommand = CreateValidCommand();
        arrangeInvalidField(createHotelCommand);

        var validationFailures = new List<ValidationFailure>
        {
            new(propertyName, errorMessage)
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateHotelCommand>(), Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        // Act
        var act = async () => await _useCase.Handle(createHotelCommand);

        // Assert
        await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
    }

    [Theory]
    [MemberData(nameof(GetFormatAndRangeValidationCases))]
    public async Task Handle_ThrowsValidationException_WhenFieldFormatOrRangeIsInvalid(
        Action<CreateHotelCommand> arrangeInvalidField,
        string propertyName,
        string errorMessage)
    {
        // Arrange
        var createHotelCommand = CreateValidCommand();
        arrangeInvalidField(createHotelCommand);

        var validationFailures = new List<ValidationFailure>
        {
            new(propertyName, errorMessage)
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateHotelCommand>(), Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult(validationFailures)));

        // Act
        var act = async () => await _useCase.Handle(createHotelCommand);

        // Assert
        await act.Should().ThrowAsync<BrisaPMS.Application.Exceptions.ValidationException>();
    }

    // Helper functions
    private static CreateHotelCommand CreateValidCommand()
    {
        return new CreateHotelCommand
        {
            LegalName = CreateLegalName(),
            CommercialName = CreateCommercialName(),
            LogoUrl = CreateLogoUrl(),
            BusinessEmail = CreateBusinessEmail(),
            BusinessPhoneNumber = CreateBusinessPhoneNumber(),
            Address1 = CreateAddress1(),
            Address2 = CreateAddress2(),
            City = CreateCity(),
            Province = CreateProvince(),
            ZipCode = CreateZipCode(),
            CheckInTime = CreateCheckInTime(),
            CheckOutTime = CreateCheckOutTime(),
            DefaultCurrencyCode = CreateDefaultCurrencyCode(),
            ItbisRate = CreateItbisRate(),
            ServiceChargeRate = CreateServiceChargeRate(),
        };
    }

    public static IEnumerable<object[]> GetRequiredFieldValidationCases()
    {
        yield return new object[] { new Action<CreateHotelCommand>(command => command.CommercialName = string.Empty), nameof(CreateHotelCommand.LegalName), "The field Legal Name is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.CommercialName = string.Empty), nameof(CreateHotelCommand.CommercialName), "The field Commercial Name is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.BusinessEmail = string.Empty), nameof(CreateHotelCommand.BusinessEmail), "The field Business Email is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.BusinessPhoneNumber = string.Empty), nameof(CreateHotelCommand.BusinessPhoneNumber), "The field Business Phone Number is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.Address1 = string.Empty), nameof(CreateHotelCommand.Address1), "The field Address is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.City = string.Empty), nameof(CreateHotelCommand.City), "The field City is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.Province = string.Empty), nameof(CreateHotelCommand.Province), "The field Province is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ZipCode = string.Empty), nameof(CreateHotelCommand.ZipCode), "The field ZipCode is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.CheckInTime = default), nameof(CreateHotelCommand.CheckInTime), "The field Check-In Time is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.CheckOutTime = default), nameof(CreateHotelCommand.CheckOutTime), "The field Check-Out Time is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ItbisRate = default), nameof(CreateHotelCommand.ItbisRate), "The field Itbis Rate is required" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ServiceChargeRate = default), nameof(CreateHotelCommand.ServiceChargeRate), "The field Service Charge Rate is required" };
    }

    public static IEnumerable<object[]> GetLengthValidationCases()
    {
        yield return new object[] { new Action<CreateHotelCommand>(command => command.LegalName = new string('L', 251)), nameof(CreateHotelCommand.LegalName), "The field Legal Name can't exceed 250 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.CommercialName = new string('C', 251)), nameof(CreateHotelCommand.CommercialName), "The field Commercial Name can't exceed 250 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.BusinessEmail = $"{new string('a', 246)}@test.com"), nameof(CreateHotelCommand.BusinessEmail), "The field Business Email can't exceed 254 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.BusinessPhoneNumber = new string('1', 26)), nameof(CreateHotelCommand.BusinessPhoneNumber), "The field Business Phone Number can't exceed 25 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.Address1 = new string('A', 201)), nameof(CreateHotelCommand.Address1), "The field Address 1 can't exceed 200 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.Address2 = new string('B', 201)), nameof(CreateHotelCommand.Address2), "The field Address 2 can't exceed 200 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.City = new string('C', 101)), nameof(CreateHotelCommand.City), "The field City can't exceed 100 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.Province = new string('P', 101)), nameof(CreateHotelCommand.Province), "The field Province can't exceed 100 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ZipCode = new string('1', 11)), nameof(CreateHotelCommand.ZipCode), "The field ZipCode can't exceed 10 characters" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.DefaultCurrencyCode = "USDD"), nameof(CreateHotelCommand.DefaultCurrencyCode), "The field Default CurrencyCode can't exceed 3 characters" };
    }

    public static IEnumerable<object[]> GetFormatAndRangeValidationCases()
    {
        yield return new object[] { new Action<CreateHotelCommand>(command => command.BusinessEmail = "invalid-email"), nameof(CreateHotelCommand.BusinessEmail), "The email must be a valid email address" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.BusinessPhoneNumber = "invalid-phone"), nameof(CreateHotelCommand.BusinessPhoneNumber), "Must be a valid phone number" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ZipCode = "12A45"), nameof(CreateHotelCommand.ZipCode), "Zip code must contain only numbers." };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.DefaultCurrencyCode = "ZZZ"), nameof(CreateHotelCommand.DefaultCurrencyCode), "Currency code not supported" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ItbisRate = -1m), nameof(CreateHotelCommand.ItbisRate), "The field Itbis Rate can't be negative" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ItbisRate = 100m), nameof(CreateHotelCommand.ItbisRate), "The field Itbis Rate can't be greater than 100" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ServiceChargeRate = -1m), nameof(CreateHotelCommand.ServiceChargeRate), "The field Service Charge Rate can't be negative" };
        yield return new object[] { new Action<CreateHotelCommand>(command => command.ServiceChargeRate = 100m), nameof(CreateHotelCommand.ServiceChargeRate), "The field Service Charge Rate can't be greater than 100" };
    }

    private static string CreateLegalName() => "Brisa S.R.L";
    private static string CreateCommercialName() => "Brisa Hotel";
    private static string CreateLogoUrl() => "https://testlogourl.jpg";
    private static string CreateBusinessEmail() => "brisaHotel@test.com";
    private static string CreateBusinessPhoneNumber() => "1234567891";
    private static string CreateAddress1() => "Address 1";
    private static string CreateAddress2() => "Address 2";
    private static string CreateCity() => "test city";
    private static string CreateProvince() => "test province";
    private static string CreateZipCode() => "12345";
    private static TimeOnly CreateCheckInTime() => new(12, 0, 0);
    private static TimeOnly CreateCheckOutTime() => new(17, 0, 0);
    private static string CreateDefaultCurrencyCode() => "USD";
    private static decimal CreateItbisRate() => 0.18m;
    private static decimal CreateServiceChargeRate() => 0.10m;
}
