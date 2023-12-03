using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberRepository
    {
        Task<bool> MemberExistsAsync(int? memberId);

        Task<MessageDto> validation(Member member);
        Task<Member> GetByID(int id);

        Task<ClientBranchMember> Edit(MemberDto memberDto);

    }
}
