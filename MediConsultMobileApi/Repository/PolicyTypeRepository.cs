using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class PolicyTypeRepository : IPolicyTypeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PolicyTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ClientPriceList>> GetRefundType()
        {
            return await dbContext.ClientPriceLists.Include(t => t.reimbursementType).AsNoTracking().ToListAsync();
        }
    }
}
