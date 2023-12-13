using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("client_price_list_table")]
    public class ClientPriceList
    {
        public int id { get; set; }

        public int client_id { get; set; }
        public int price_list_id { get; set; }
        public double? reimbursement_percent { get; set; }
        public int? is_on_program { get; set; }

        // TODO : Cahnge DataType to  int  
        public int? program_id { get; set; }
        public string? notes { get; set; }

        [ForeignKey("reimbursementType")]
        public int? type_id { get; set; }

        public ReimbursementType reimbursementType { get; set; }
        public int? policy_id { get; set; }
        public int? is_on_pricelist { get; set; }
        public double? max_value { get; set; }
    }
}
