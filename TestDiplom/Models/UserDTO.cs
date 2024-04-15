using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserDTO
    {
        public string UserFirstName { get; set; }
        public string UserSecondName { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
    }
}
