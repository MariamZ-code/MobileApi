using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ITokenRepository
    {
        Task<Login> SaveToken(FirebaseTokenDTO tokenDto);
    }
}
