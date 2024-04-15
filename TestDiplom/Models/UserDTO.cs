using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserDTO
    {
        [RegularExpression(@"^[a-zA-Zа-яА-Я]*$", ErrorMessage = "Допускаются только латинские и кириллические символы.")]
        public string UserFirstName { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-Я]*$", ErrorMessage = "Допускаются только латинские и кириллические символы.")]
        public string UserSecondName { get; set; }
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Допускаются только латинские символы.")]
        public string UserLogin { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Допускаются только латинские символы и цифры.")]
        public string UserPassword { get; set; }
    }
}
