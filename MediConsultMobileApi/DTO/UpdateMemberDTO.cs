using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class UpdateMemberDTO
    {
        
        //[RegularExpression(@"^01[01235][0-9]{8}$", ErrorMessage = "Invalid phone number, please enter valid phone number")]
        public string? Mobile { get; set; }
        public string? SSN { get; set; }
        public string? Email { get; set; }
        public IFormFile? Photo { get; set; }
     

    }
}
