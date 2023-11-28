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


        public async Task<Member> GetByID(int id)
        {

            return await dbContext.Members.FirstOrDefaultAsync(m => m.member_id == id);

        }
        public async Task<MessageDto> validation(Member member)
        {
            var msg = new MessageDto();
            
            if (member is null)
            {
                msg.Message = "User not Found";
                return msg;
            }
            else
            {

                if (member.program_name is null)
                {
                    msg.Message = "User in Archive";
                    return msg;
                }
                if (member.member_status == "Deactivated")
                {
                    msg.Message = "User is Deactivated";

                    return msg;
                }
                else if (member.member_status == "Hold")
                {
                    msg.Message = "User is Hold";
                    return msg;

                }
                
            }

            return msg;
        }
    }
}
