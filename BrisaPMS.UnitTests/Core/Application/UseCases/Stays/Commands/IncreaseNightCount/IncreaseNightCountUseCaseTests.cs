using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;
using BrisaPMS.Domain.Stays;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.IncreaseNightCount;

public class IncreaseNightCountUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IncreaseNightCountUseCase _useCase;

    public IncreaseNightCountUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new IncreaseNightCountUseCase(_staysRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_IncreasesNightCountAndPersistsChanges()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var command = new IncreaseNightCountCommand { StayId = stayId };
        var stay = StayTestData.CreateStay(stayId: stayId);

        _staysRepositoryMock.GetById(stayId).Returns(stay);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        stay.NightCount.Should().Be(1);
        await _staysRepositoryMock.Received(1).Update(Arg.Is<Stay>(updatedStay =>
            updatedStay.Id == stayId &&
            updatedStay.NightCount == 1));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenStayDoesNotExist()
    {
        // Arrange
        var command = new IncreaseNightCountCommand { StayId = Guid.NewGuid() };

        _staysRepositoryMock.GetById(command.StayId).Returns((Stay?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _staysRepositoryMock.DidNotReceive().Update(Arg.Any<Stay>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var command = new IncreaseNightCountCommand { StayId = stayId };
        var stay = StayTestData.CreateStay(stayId: stayId);

        _staysRepositoryMock.GetById(stayId).Returns(stay);
        _staysRepositoryMock.Update(Arg.Any<Stay>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
