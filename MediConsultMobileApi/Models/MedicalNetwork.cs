using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_en_get_medical_network_map_view")]
    [Keyless]
    public class MedicalNetwork
    {
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Email { get; set; }
        public string? Hotline { get; set; }
        public string? Mobile { get; set; }
        public string? Telephone { get; set; }
        public string Provider_name { get; set; }
        public string provider_name_en { get; set; }
        public string? Speciality { get; set; }
        public string Category { get; set; }
    }
}
