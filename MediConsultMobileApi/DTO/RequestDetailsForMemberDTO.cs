using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class RequestDetailsForMemberDTO
    {
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public string ProviderName { get; set; }
        public string? Status { get; set; }
       
    }
}
