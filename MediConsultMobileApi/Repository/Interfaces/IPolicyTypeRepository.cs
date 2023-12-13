using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IPolicyTypeRepository
    {
        Task<List<ClientPriceList>> GetRefundType();   
    }
}
 