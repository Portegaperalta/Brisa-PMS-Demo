using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Companies;

namespace BrisaPMS.Persistence.Repositories;

public class CompaniesRepository : Repository<Company>, ICompaniesRepository
{
    public CompaniesRepository(BrisaPmsDbContext context) 
        : base(context)
    {
    }
}