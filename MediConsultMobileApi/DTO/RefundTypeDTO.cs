using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class RefundTypeDTO
    {
        public string? en_name { get; set; }

        public string? ar_name { get; set; }
        public int? is_on_program { get; set; }

        public int? program_id { get; set; }
    }
}
