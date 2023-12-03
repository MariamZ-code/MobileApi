using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("client_portal_users_table")]
    public class Login
    {
        [Key]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "Id must be a valid integer.")]
        public int member_id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string the_password { get; set; }
        public int is_enabled { get; set; }

        // TODO : Add Column to DB
        public string firebase_token { get; set; }

    }

}
