using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRequestRepository
    {
        List<Request> GetRequestsByMemberId(int id);
        Request AddRequest(RequestDTO requestDto);

        
        
    }
}
