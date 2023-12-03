using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ITokenRepository
    {
        Login SaveToken(FirebaseTokenDTO tokenDto);
    }
}
