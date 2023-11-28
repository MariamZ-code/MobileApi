using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_en_get_medical_network_map_view")]
    [Keyless]
    public class MedicalNetwork
    {
        public string? latitude { get; set; }
        public string? longitude { get; set; }
        public string? email { get; set; }
        public string? hotline { get; set; }
        public string? mobile { get; set; }
        public string? telephone { get; set; }
        public string provider_name { get; set; }
        public string? speciality { get; set; }
        public string category { get; set; }
    }
}
