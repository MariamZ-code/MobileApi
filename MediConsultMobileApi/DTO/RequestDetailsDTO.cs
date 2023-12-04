using MediConsultMobileApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Mail;

namespace MediConsultMobileApi.DTO
{
    public class RequestDetailsDTO
    {
        public int Id { get; set; }
        public Approval? Approval { get; set; }
        public string? CreatedDate { get; set; }
        public int? ApprovalId { get; set; } 
        public string? Status { get; set; } 
        public string? Notes { get; set; }



    }
}
