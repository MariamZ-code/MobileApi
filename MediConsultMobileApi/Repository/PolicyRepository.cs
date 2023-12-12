using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PolicyRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Policy>> GetByProgramId(int progId)
        {
            return await dbContext.Policies.Where(p => p.Program_id == progId).AsNoTracking().ToListAsync();
        }
    }
}
