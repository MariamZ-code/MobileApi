using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IProviderDataRepository
    {
        IQueryable<ProviderData> GetProviders();
        Task<bool> ProviderExistsAsync(int? providerId);

    }
}
