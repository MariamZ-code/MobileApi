using MediConsultMobileApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Mail;

namespace MediConsultMobileApi.DTO
{
    public class RequestDetailsDTO
    {
        public int Id { get; set; }
        public int? ApprovalId { get; set; } 
        public int? ProviderId { get; set; }
        public string ProviderName { get; set; }
      
        public string? Notes { get; set; }
        public List<string> FolderPath { get; set; }

        public Approval? Approval { get; set; }


    }
}
