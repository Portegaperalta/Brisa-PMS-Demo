using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;
using BrisaPMS.Domain.Stays;
using BrisaPMS.UnitTests.Core.Application.UseCases.Stays;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.DeleteStay;

public class DeleteStayUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteStayUseCase _useCase;

    public DeleteStayUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new DeleteStayUseCase(_staysRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_DeletesStay()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var stay = StayTestData.CreateStay(stayId);
        var command = new DeleteStayCommand { Id = stayId };

        _staysRepositoryMock.GetById(stayId).Returns(stay);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _staysRepositoryMock.Received(1).Delete(stay);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenStayDoesNotExist()
    {
        // Arrange
        var command = new DeleteStayCommand { Id = Guid.NewGuid() };

        _staysRepositoryMock.GetById(command.Id).Returns((Stay?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _staysRepositoryMock.DidNotReceive().Delete(Arg.Any<Stay>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryDeleteFails()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var stay = StayTestData.CreateStay(stayId);
        var command = new DeleteStayCommand { Id = stayId };

        _staysRepositoryMock.GetById(stayId).Returns(stay);
        _staysRepositoryMock.Delete(Arg.Any<Stay>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
