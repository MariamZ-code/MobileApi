using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MediConsultMobileApi.Models
{
    [Table("client_portal_approval_request_table")]
    public class Request
    {
        public int ID { get; set; }
   
        public string? Notes { get; set; }

        public string? created_date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public int? Provider_location_id { get; set; } = 0;
        public int? Member_id { get; set; }

        [StringLength(100)]
        public string? Status { get; set; } = "Pending";
        public int? Is_pharma { get; set; } = 0;

        public string? Folder_path { get; set; }

        [StringLength(350)]
        public string? The_email { get; set; }
        public int? Is_notified { get; set; } = 0;

        public int? Mc_notified { get; set; } = 0;

        [MaxLength(50)]
        public string? Whatsapp_number { get; set; }

        public int? Client_user_id { get; set; } = 0;

        [ForeignKey("Provider")]
        public int? Provider_id { get; set; } 

        public ProviderData Provider { get; set; }

        [ForeignKey("Approval")]
        public int? Approval_id { get; set; } = 0;

        //public Approval Approval { get; set; }






    }
}
