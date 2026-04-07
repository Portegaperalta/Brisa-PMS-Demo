using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using FluentValidation;
using NSubstitute;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands;

public class UpdateHotelContactInfoUseCaseTests
{
    private IHotelsRepository _repositoryMock;
    private IValidator<UpdateHotelContactInfoCommand> _validatorMock;
    private IUnitOfWork _unitOfWorkMock;
    private UpdateHotelContactInfoUseCase _useCase;

    public UpdateHotelContactInfoUseCaseTests()
    {
        _repositoryMock = Substitute.For<IHotelsRepository>();
        _validatorMock = Substitute.For<IValidator<UpdateHotelContactInfoCommand>>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateHotelContactInfoUseCase(_repositoryMock, _unitOfWorkMock, _validatorMock);
    }
}