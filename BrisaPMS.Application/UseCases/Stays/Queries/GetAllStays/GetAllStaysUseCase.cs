using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetAllStays;

public class GetAllStaysUseCase : IRequestHandler<GetAllStaysQuery, List<StayDto>>
{
    private readonly IStaysRepository _staysRepository;

    public GetAllStaysUseCase(IStaysRepository staysRepository) { _staysRepository = staysRepository; }

    public async Task<List<StayDto>> Handle(GetAllStaysQuery query)
    {
        var stays = await _staysRepository.GetAll();
        var staysDtos = stays.Select(s => s.ToDto()).ToList();
        return staysDtos;
    }
}