using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Provider_Category")]
    public class ProviderCategory
    {
        [Key]
        public int category_id { get; set; }
        public string category_name_en { get; set; }
    }
}
