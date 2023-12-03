using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetRequestsByMemberId(int memberId) ;
        Request AddRequest(RequestDTO requestDto);

        
        
    }
}
