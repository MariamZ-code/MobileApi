using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class RefundTypeRepository : IRefundTypeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RefundTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ClientPriceList>> GetAllRefundType()
        { 
            return await dbContext.ClientPriceLists.Include(t => t.reimbursementType).AsNoTracking().ToListAsync();

        }
        public async Task<List<ClientPriceList>> GetRefundTypeByOnProgram(int? program_id)
        {
            return await dbContext.ClientPriceLists.Include(t => t.reimbursementType).Where(p=> p.is_on_program == 1 && p.program_id== program_id).AsNoTracking().ToListAsync();
        }

        public async Task<List<ClientPriceList>> GetRefundTypeByOnPolicy(int? policy_id)
        {
            return await dbContext.ClientPriceLists.Include(t => t.reimbursementType).Where(p => p.is_on_program == 0 && p.policy_id == policy_id).AsNoTracking().ToListAsync();
        }

    }
}
