﻿using MediConsultMobileApi.DTO;
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

        #region MemberExist
        public bool MemberExists(int? memberId)
        {
            return dbContext.Members.Any(m => m.member_id == memberId);

        }

        #endregion


        #region GetMemberbyId
        public async Task<Member> GetByID(int id)
        {

            return await dbContext.Members.FirstOrDefaultAsync(m => m.member_id == id);

        }

        #endregion


        #region ValidationMember
        public async Task<MessageDto> validation(Member member)
        {
            var msg = new MessageDto();

            if (member is null)
            {
                msg.Message = "User in Archive";
                return msg;

            }
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
            if (member.member_status == "Hold")
            {
                msg.Message = "User is Hold";
                return msg;
            }



            return msg;
        }

        #endregion

        #region MemberFamily

        public async Task<List<ClientBranchMember>> MemberFamily(int id)
        {
            return await dbContext.clientBranchMembers.Where(f=>f.member_HOF_id== id).AsNoTracking().ToListAsync();
        }
        #endregion


        #region MemberDetails
        public async Task<ClientBranchMember> MemberDetails(int memberId)
        {
            return await dbContext.clientBranchMembers.FirstOrDefaultAsync(m => m.member_id == memberId); 
        }
        #endregion

        #region UpdateMember
        public async void UpdateMember(UpdateMemberDTO memberDTO , int id)
        {
            var member = await MemberDetails(id);

            member.email = memberDTO.Email;
            member.mobile = memberDTO.Mobile;
            member.member_birthday = memberDTO.Birthday;
            member.member_nid = memberDTO.SSN;
            member.member_photo = memberDTO.Photo;
                
            
            dbContext.clientBranchMembers.Update(member);
            

        }
        #endregion

        public void SaveDatabase()
        {
            dbContext.SaveChanges();
        }
    }
}
