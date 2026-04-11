using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;
using BrisaPMS.Domain.Companies;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyRnc;

public class UpdateCompanyRncUseCaseTests
{
    private readonly ICompaniesRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateCompanyRncUseCase _useCase;

    public UpdateCompanyRncUseCaseTests()
    {
        _repositoryMock = Substitute.For<ICompaniesRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateCompanyRncUseCase(_repositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesCompanyRnc()
    {
        var companyId = Guid.NewGuid();
        var company = CreateCompany(companyId);
        var newRnc = "987654321";
        var command = CreateCommand(companyId, newRnc);

        _repositoryMock.GetById(companyId).Returns(company);

        var result = await _useCase.Handle(command);

        result.Should().BeTrue();
        company.Rnc.Value.Should().Be(newRnc);
        await _repositoryMock.Received(1).Update(company);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenCompanyDoesNotExist()
    {
        var companyId = Guid.NewGuid();
        var command = CreateCommand(companyId, CreateRnc());

        _repositoryMock.GetById(companyId).Returns((Company?)null);

        var act = async () => await _useCase.Handle(command);

        await act.Should().ThrowAsync<NotFoundException>();
        await _repositoryMock.DidNotReceive().Update(Arg.Any<Company>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        var companyId = Guid.NewGuid();
        var company = CreateCompany(companyId);
        var command = CreateCommand(companyId, CreateRnc());

        _repositoryMock.GetById(companyId).Returns(company);

        _repositoryMock
            .When(repository => repository.Update(Arg.Any<Company>()))
            .Do(_ => throw new InvalidOperationException("Update failed"));

        var act = async () => await _useCase.Handle(command);

        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateCompanyRncCommand CreateCommand(Guid companyId, string newRnc)
    {
        return new UpdateCompanyRncCommand
        {
            CompanyId = companyId,
            NewRnc = newRnc
        };
    }

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

    private static string CreateRnc() => "123456789";
}