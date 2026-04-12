using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Companies.Queries.GetCompanyInfo;
using BrisaPMS.Domain.Companies;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Queries;

public class GetCompanyInfoUseCaseTests
{
  private readonly ICompaniesRepository _repositoryMock;
  private readonly GetCompanyInfoUseCase _useCase;

  public GetCompanyInfoUseCaseTests()
  {
    _repositoryMock = Substitute.For<ICompaniesRepository>();
    _useCase = new GetCompanyInfoUseCase(_repositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsCompanyDto()
  {
    // Arrange
    var companyId = Guid.NewGuid();
    var company = CreateCompany(companyId);
    var query = new GetCompanyInfoQuery() { CompanyId = companyId };

    _repositoryMock.GetById(companyId).Returns(company);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(company.Id);
    result.LegalName.Should().Be(company.LegalName);
    result.CommercialName.Should().Be(company.CommercialName);
    result.Rnc.Should().Be(company.Rnc.Value);
    result.Email.Should().Be(company.BusinessEmail.Value);
    result.BusinessPhone.Should().Be(company.BusinessPhone.Value);
    result.LogoUrl.Should().Be(company.LogoUrl!.Value);
    result.Address1.Should().Be(company.Address.Address1);
    result.Address2.Should().Be(company.Address.Address2);
    result.City.Should().Be(company.Address.City);
    result.Province.Should().Be(company.Address.Province);
    result.ZipCode.Should().Be(company.Address.ZipCode);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenCompanyDoesNotExist()
  {
    // Arrange
    var companyId = Guid.NewGuid();
    var query = new GetCompanyInfoQuery() { CompanyId = companyId };

    _repositoryMock.GetById(companyId).ReturnsNull();

    // Act
    var act = async () => await _useCase.Handle(query);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  // Helper methods
  private static Company CreateCompany(Guid? companyId = null)
  {
    return new Company(
        "Brisa PMS SRL",
        "Brisa PMS",
        new Rnc("123456789"),
        new Email("contact@brisapms.com"),
        new PhoneNumber("+18095551234"),
        new Url("https://example.com/logo.png"),
        new Address("123 Main Street", "Suite 4B", "Santo Domingo", "Distrito Nacional", "10101"))
    {
      Id = companyId ?? Guid.NewGuid()
    };
  }
}
