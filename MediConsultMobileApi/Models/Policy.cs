using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Policy_Program_Limit")]

    public class Policy
    {
        [Key]
        public int Child_id { get; set; }

        public int Policy_id { get; set; }

        public int Program_id { get; set; }

        public int Service_Class_Id { get; set; }

        public Service Service { get; set; }
        public double SL_Limit  { get; set; }
        public int SL_Copayment { get; set; }

    }
}
