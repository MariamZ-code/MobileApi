using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Client_Branch_Member")]
    public class ClientBranchMember
    {
        public int Id { get; set; }
        public string Photo { get; set; }

        public string Email { get; set; }
        public string mobile { get; set; }
    }
}
