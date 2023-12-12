using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Service_Class")]

    public class Service
    {
        [Key]
        public int Service_Class_id { get; set; }
        public string Service_Class_Name_En { get; set; }
        public string Service_Class_Name_Ar { get; set; }
    }
}
