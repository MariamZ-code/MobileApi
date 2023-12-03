using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    
    public class LoginUserDto
    {
      
        public string Id { get; set; }

        public string Password { get; set; }

        public string Firebase_token { get; set; }

    }
}
