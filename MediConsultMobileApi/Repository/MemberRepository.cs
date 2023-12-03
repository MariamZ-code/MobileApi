using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MemberRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        

        public async Task<bool> MemberExistsAsync(int? memberId)
        {
            return await dbContext.Members.AnyAsync(m => m.member_id == memberId);

        }

        public async Task<Member> GetByID(int id)
        {

            return await dbContext.Members.FirstOrDefaultAsync(m => m.member_id == id);

        }
        public async Task<MessageDto> validation(Member member)
        {
            var msg = new MessageDto();

            
            

            return msg;
        }

        public Task<ClientBranchMember> Edit(MemberDto memberDto)
        {

        }
    }
}
