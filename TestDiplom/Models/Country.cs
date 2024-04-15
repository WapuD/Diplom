using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("countryId")]
        public int CountryId { get; set; }
        [Column("countryName")]
        public string CountryName { get; set; }
    }
}
