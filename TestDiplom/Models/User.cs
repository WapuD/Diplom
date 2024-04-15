using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("userId")]
        public int UserId { get; set; }
        [Column("userFirstName")]
        public string UserFirstName { get; set; }
        [Column("userSecondName")]
        public string UserSecondName { get; set; }
        [Column("userThirdName")]
        public string? UserThirdName { get; set; }
        [Column("userLogin")]
        public string UserLogin { get; set; }
        [Column("userPassword")]
        public string UserPassword { get; set; }
        [Column("userGender")]
        public bool? UserGender { get; set; }
        [Column("userAge")]
        public int? UserAge { get; set; }


        [Column("userRole")]
        [ForeignKey("Role")]
        public int UserRole { get; set; } = 1;
        public Role Role { get; set; }
        [Column("userImage")]
        [ForeignKey("Image")]
        public int UserImage { get; set; } = 1;
        public Image Image { get; set; }
        [Column("userCountry")]
        [ForeignKey("Country")]
        public int UserCountry { get; set; } = 1;
        public Country Country { get; set; }
    }
}
