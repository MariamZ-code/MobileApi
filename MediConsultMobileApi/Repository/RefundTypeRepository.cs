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
        public async Task<List<ClientPriceList>> GetRefundTypeByOnProgram()
        {
            return await dbContext.ClientPriceLists.Include(t => t.reimbursementType).Where(p=> p.is_on_program == 0).AsNoTracking().ToListAsync();
        }
    }
}
