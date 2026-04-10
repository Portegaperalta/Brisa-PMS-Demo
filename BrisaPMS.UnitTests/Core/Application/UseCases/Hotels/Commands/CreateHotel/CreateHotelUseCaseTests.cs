using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using BrisaPMS.Domain.Hotels;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelUseCaseTests
{
    private IHotelsRepository _repositoryMock;
    private IUnitOfWork _unitOfWorkMock;
    private CreateHotelUseCase _useCase;

    public CreateHotelUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateHotelUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesHotelAndReturnsHotelId()
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

        _repositoryMock.Create(Arg.Any<Hotel>())
                       .Returns(callInfo => callInfo.Arg<Hotel>());

        // Act
        var result = await _useCase.Handle(createHotelCommand);

        // Assert
        await _repositoryMock.Received(1).Create(Arg.Any<Hotel>());
        await _unitOfWorkMock.Received(1).Persist();
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_MakesRollBack_WhenExceptionIsThrown()
    {
        // Arrange
        var command = CreateValidCommand();
        _repositoryMock.Create(Arg.Any<Hotel>()).Throws<Exception>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWorkMock.Received(1).Revert();
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