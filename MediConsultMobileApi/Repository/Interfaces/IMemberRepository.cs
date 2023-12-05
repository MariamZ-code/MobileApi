using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberRepository
    {
        bool MemberExists(int? memberId);

        Task<MessageDto> validation(Member member);
        Task<Member> GetByID(int id);

        Task<List<ClientBranchMember>> MemberFamily(int id);

        Task<ClientBranchMember> MemberDetails(int memberId);

        void UpdateMember(UpdateMemberDTO memberDTO, int id);
        void SaveDatabase();

    }
}
