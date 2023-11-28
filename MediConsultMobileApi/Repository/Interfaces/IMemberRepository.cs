using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberRepository
    {
        Task<MessageDto> validation(Member member);
        Task<Member> GetByID(int id);
    }
}
