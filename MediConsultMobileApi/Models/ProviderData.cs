using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Provider_Data")]

    public class ProviderData
    {
        [Key]
        public int Provider_id { get; set; }

        [Required]
        [StringLength(200)]  
        public string Provider_name_en { get; set; }

        public List<Request> Requests { get; set; }
    }
}
