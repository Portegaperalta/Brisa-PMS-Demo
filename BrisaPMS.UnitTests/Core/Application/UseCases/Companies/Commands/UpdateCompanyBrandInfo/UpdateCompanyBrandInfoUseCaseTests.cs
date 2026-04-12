using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;
using BrisaPMS.Domain.Companies;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;

public class UpdateCompanyBrandInfoUseCaseTests
{
    private readonly ICompaniesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateCompanyBrandInfoUseCase _useCase;

    public UpdateCompanyBrandInfoUseCaseTests()
    {
        _repositoryMock = Substitute.For<ICompaniesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateCompanyBrandInfoUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesCompanyBrandInfo()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = CreateCompany(companyId);
        var newLegalName = CreateLegalName();
        var newCommercialName = CreateCommercialName();
        var newLogoUrl = CreateLogoUrl();

        var command = new UpdateCompanyBrandInfoCommand
        {
            CompanyId = companyId,
            NewLegalName = newLegalName,
            NewCommercialName = newCommercialName,
            NewLogoUrl = newLogoUrl
        };

        _repositoryMock.GetById(companyId).Returns(company);

        // Act
        await _useCase.Handle(command);

        // Assert
        company.LegalName.Should().Be(newLegalName);
        company.CommercialName.Should().Be(newCommercialName);
        company.LogoUrl.Should().NotBeNull();
        company.LogoUrl!.Value.ToString().Should().Be(newLogoUrl);

        await _repositoryMock.Received(1).Update(company);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_UpdatesCompanyBrandInfo_WithoutLogoUrl()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = CreateCompany(companyId);
        var newLegalName = CreateLegalName();
        var newCommercialName = CreateCommercialName();

        var command = new UpdateCompanyBrandInfoCommand
        {
            CompanyId = companyId,
            NewLegalName = newLegalName,
            NewCommercialName = newCommercialName,
            NewLogoUrl = null
        };

        _repositoryMock.GetById(companyId).Returns(company);

        // Act
        await _useCase.Handle(command);

        // Assert
        company.LegalName.Should().Be(newLegalName);
        company.CommercialName.Should().Be(newCommercialName);

        await _repositoryMock.Received(1).Update(company);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var command = CreateUpdateCompanyBrandInfoCommand
        (
            companyId,
            CreateLegalName(),
            CreateCommercialName(),
            CreateLogoUrl()
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
    private static UpdateCompanyBrandInfoCommand
        CreateUpdateCompanyBrandInfoCommand(Guid companyId, string newLegalName, string newCommercialName,
            string? newLogoUrl)
    {
        return new UpdateCompanyBrandInfoCommand
        {
            CompanyId = companyId,
            NewLegalName = newLegalName,
            NewCommercialName = newCommercialName,
            NewLogoUrl = newLogoUrl
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

    private static string CreateLegalName() => "New Legal Name SRL";
    private static string CreateCommercialName() => "New Brand Name";
    private static string CreateLogoUrl() => "https://example.com/new-logo.png";
}
