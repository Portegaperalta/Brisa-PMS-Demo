using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetStayById;

public class GetStayByIdUseCase : IRequestHandler<GetStayByIdQuery, StayDto>
{
    private readonly IStaysRepository _staysRepository;

    public GetStayByIdUseCase(IStaysRepository staysRepository){ _staysRepository = staysRepository; }

    public async Task<StayDto> Handle(GetStayByIdQuery query)
    {
        var stay = await _staysRepository.GetById(query.StayId);

        if (stay is null)
            throw new NotFoundException("Stay", query.StayId);

        return stay.ToDto();
    }
}