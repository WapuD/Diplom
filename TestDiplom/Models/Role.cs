using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("roleId")]
        public int RoleId { get; set; }
        [Column("roleName")]
        public string RoleName { get; set; }
    }
}
