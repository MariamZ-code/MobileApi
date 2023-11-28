using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    
    public class LoginUserDto
    {
       // [Required]
        public string Id { get; set; }

      //  [Required]
       // [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
