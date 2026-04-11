using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;
using BrisaPMS.Domain.Companies;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;

public class UpdateCompanyAddressInfoUseCaseTests
{
    private readonly ICompaniesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateCompanyAddressInfoUseCase _useCase;

    public UpdateCompanyAddressInfoUseCaseTests()
    {
        _repositoryMock = Substitute.For<ICompaniesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateCompanyAddressInfoUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesCompanyAddressInfo()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = CreateCompany(companyId);
        var newAddress1 = CreateAddress1();
        var newAddress2 = CreateAddress2();
        var newCity = CreateCity();
        var newProvince = CreateProvince();
        var newZipCode = CreateZipCode();

        var command = new UpdateCompanyAddressInfoCommand
        {
            CompanyId = companyId,
            NewAddress1 = newAddress1,
            NewAddress2 = newAddress2,
            NewCity = newCity,
            NewProvince = newProvince,
            NewZipCode = newZipCode
        };

        _repositoryMock.GetById(companyId).Returns(company);

        // Act
        await _useCase.Handle(command);

        // Assert
        company.Address.Address1.Should().Be(newAddress1);
        company.Address.Address2.Should().Be(newAddress2);
        company.Address.City.Should().Be(newCity);
        company.Address.Province.Should().Be(newProvince);
        company.Address.ZipCode.Should().Be(newZipCode);

        await _repositoryMock.Received(1).Update(company);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var command = CreateUpdateCompanyAddressInfoCommand
        (
            companyId,
            CreateAddress1(),
            CreateAddress2(),
            CreateCity(),
            CreateProvince(),
            CreateZipCode()
        );

        _repositoryMock.GetById(companyId).Returns((Company?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Update(Arg.Any<Company>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    // Helper methods
    private static UpdateCompanyAddressInfoCommand
        CreateUpdateCompanyAddressInfoCommand(Guid companyId, string address1, string address2, string city,
            string province, string zipCode)
    {
        return new UpdateCompanyAddressInfoCommand
        {
            CompanyId = companyId,
            NewAddress1 = address1,
            NewAddress2 = address2,
            NewCity = city,
            NewProvince = province,
            NewZipCode = zipCode
        };
    }

    private static Company CreateCompany(Guid? companyId = null)
    {
        return new Company(
            "Brisa Hospitality SRL",
            "Brisa PMS",
            new Rnc("132456789"),
            new Email("contact@brisapms.com"),
            new PhoneNumber("+18095551234"),
            new Url("https://example.com/logo.png"),
            new Address("123 Main Street", "Suite 4B", "Santo Domingo", "Distrito Nacional", "10101"))
        {
            Id = companyId ?? Guid.NewGuid()
        };
    }

    private static string CreateAddress1() => "223 North street";
    private static string CreateAddress2() => "Suite 2C";
    private static string CreateCity() => "Santiago";
    private static string CreateProvince() => "Cibao";
    private static string CreateZipCode() => "105000";
}
