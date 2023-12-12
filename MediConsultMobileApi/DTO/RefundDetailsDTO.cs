using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class RefundDetailsDTO
    {
        public int Id { get; set; }
        public int? ApprovalId { get; set; }
        public int? RefundId { get; set; }
        public string RefundType { get; set; }

        public string? Notes { get; set; }

    }
}
