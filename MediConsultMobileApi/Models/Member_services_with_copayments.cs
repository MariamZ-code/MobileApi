using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Keyless]
    [Table("app_en_get_services_with_copayment_view")]
    public class Member_services_with_copayments
    {
        [Key]
        public int program_id { get; set; }
        public string service_nameEn { get; set; }
        public string service_nameAr { get; set; }
        public int copayment { get; set; }
    }

}
