using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetAllStays;

public class GetAllStaysQuery : IRequest<List<StayDto>>
{
}