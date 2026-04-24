using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;

public class IncreaseNightCountUseCase : IRequestHandler<IncreaseNightCountCommand, bool>
{
    private readonly IStaysRepository _staysRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IncreaseNightCountUseCase(IStaysRepository staysRepository, IUnitOfWork unitOfWork)
    {
        _staysRepository = staysRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(IncreaseNightCountCommand command)
    {
        var stay = await _staysRepository.GetById(command.StayId);

        if (stay is null)
            throw new NotFoundException("Stay", command.StayId);
        
        stay.IncreaseNightCount();

        try
        {
            await _staysRepository.Update(stay);
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