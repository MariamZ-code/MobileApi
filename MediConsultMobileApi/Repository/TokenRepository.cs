using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TokenRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Login> SaveToken(FirebaseTokenDTO tokenDto)
        {
            var token = new Login()
            {
                firebase_token = tokenDto.Firebase_token
            };
            
            await dbContext.AddAsync(token);
            dbContext.SaveChanges();
            return token;
        }
    }
}
