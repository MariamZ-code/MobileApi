using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Provider_Category")]
    public class Category
    {
        public int CategoryId { get; set; }
        public string Category_Name_En { get; set; }
    }
}
