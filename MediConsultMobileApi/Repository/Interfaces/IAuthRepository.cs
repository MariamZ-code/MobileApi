using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<MessageDto> Login(LoginUserDto userDto);
    }
}
