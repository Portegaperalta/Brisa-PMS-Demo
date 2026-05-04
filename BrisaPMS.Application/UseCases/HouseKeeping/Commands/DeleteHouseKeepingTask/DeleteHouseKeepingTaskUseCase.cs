using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;

public class DeleteHouseKeepingTaskUseCase : IRequestHandler<DeleteHouseKeepingTaskCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHouseKeepingTaskUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository,
        IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteHouseKeepingTaskCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.Id) ??
                               throw new NotFoundException("HouseKeeping Task", command.Id);

        try
        {
            await _houseKeepingTasksRepository.Delete(houseKeepingTask);
            await _unitOfWork.Persist();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}