using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        #endregion
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
            
            authDto.Message = " Login Successfully";

            return authDto;
        }

        #region MemberExists
        public bool MemberLoginExists(int? memberId)
        {
            return dbContext.logins.Any(m => m.member_id == memberId);

        }

        #endregion
    }
}
