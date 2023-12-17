using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<MessageDto> Login(LoginUserDto userDto, string lang);

        bool MemberLoginExists(int? memberId);

        Login ResetPassword(int memberId);

        void SendOtp(string otp, int memberId);

        void ChangePass(string otp, int id, ChangePasswordDTO changeDto);


    }
}
