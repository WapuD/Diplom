using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSecondName { get; set; }
        public string? UserThirdName { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public bool? UserGender { get; set; }
        public int? UserAge { get; set; }
        public int UserRole { get; set; }
        public int UserImage { get; set; }
        public int UserCountry { get; set; }
    }
}
