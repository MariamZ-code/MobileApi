using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_reimbursement_type")]
    public class ReimbursementType
    {
        public int id { get; set; }

        [MaxLength(400)]
        public string? en_name { get; set; }

        [MaxLength(400)]
        public string? ar_name { get; set; }
        public int? is_deleted { get; set; }

    }
}
