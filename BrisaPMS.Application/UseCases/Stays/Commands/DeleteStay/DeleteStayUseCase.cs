using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;

public class DeleteStayUseCase : IRequestHandler<DeleteStayCommand, bool>
{
    private readonly IStaysRepository _staysRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStayUseCase(IStaysRepository staysRepository, IUnitOfWork unitOfWork)
    {
        _staysRepository = staysRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteStayCommand command)
    {
        var stay = await _staysRepository.GetById(command.Id) ??
                   throw new NotFoundException("Stay", command.Id);

        try
        {
            await _staysRepository.Delete(stay);
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