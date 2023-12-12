using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Provider_Category")]
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }

        public string Category_Name_En { get; set; }
    }
}
