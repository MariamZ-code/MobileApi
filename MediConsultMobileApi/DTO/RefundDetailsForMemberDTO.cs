namespace MediConsultMobileApi.DTO
{
    public class RefundDetailsForMemberDTO
    {
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public string RefundType { get; set; }
        public string? Status { get; set; }
        public string? RefundDate { get; set; }
        public double? Amount { get; set; }
        public string? Note { get; set; }

    }
}
