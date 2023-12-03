using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TokenRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Login SaveToken(FirebaseTokenDTO tokenDto)
        {
           
                var token = new Login()
                {
                    member_id = tokenDto.MemberId,
                    firebase_token = tokenDto.Firebase_token
                };

                dbContext.Update(token);
                dbContext.SaveChanges();
                return token;
            
            
        }
    }
}
