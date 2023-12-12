using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Net;
using MediConsultMobileApi.Services;

namespace MediConsultMobileApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AuthRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region Login

      
        public async Task<MessageDto> Login(LoginUserDto userDto)
        {
            var authDto = new MessageDto();
            var user = await dbContext.logins.FirstOrDefaultAsync(u => u.member_id == int.Parse(userDto.Id) && u.the_password == userDto.Password);
         
            if (user is null)
            {
               authDto.Message = "Id or Password is incorrect";
               
                return authDto;
            }
          
            if (user.is_enabled == 0)
            {
                authDto.Message = "Account Disabled";

                return authDto;
            }
            
            authDto.Message = "Login Successfully";

            return authDto;
        }
        #endregion

        #region MemberExists
        public bool MemberLoginExists(int? memberId)
        {
            return dbContext.logins.Any(m => m.member_id == memberId);

        }


        #endregion

        public Login ResetPassword(int memberId)
        {
           return dbContext.logins.FirstOrDefault(m => m.member_id == memberId);
        }




        #region SendOtp

        public  void SendOtp(string otp, int memberId)
        {
            var member = ResetPassword(memberId);
            member.Otp = otp;

            dbContext.logins.Update(member);
            dbContext.SaveChanges();

        }
        #endregion

        #region ChangePassword

        public void ChangePass(string otp,int id , ChangePasswordDTO changeDto)
        {
            var member = ResetPassword(id);

          
                member.the_password = changeDto.Password;

                dbContext.logins.Update(member);

                dbContext.SaveChanges();

        }

        #endregion


    }
}
