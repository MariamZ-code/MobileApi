using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRefundTypeRepository
    {
        Task<List<ClientPriceList>> GetAllRefundType();
        Task<List<ClientPriceList>> GetRefundTypeByOnProgram(int? program_id);
        Task<List<ClientPriceList>> GetRefundTypeByOnPolicy(int? policy_id);
    }
}
 