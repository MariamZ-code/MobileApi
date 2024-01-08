using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ProviderDataRepository : IProviderDataRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProviderDataRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public  IQueryable<ProviderData> GetProviders()
        {

            return  dbContext.Providers.AsNoTracking().AsQueryable();


        }
        public async Task<bool> ProviderExistsAsync(int? providerId)
        {
            return await dbContext.Providers.AnyAsync(p => p.provider_id == providerId);
        }
    }
}
