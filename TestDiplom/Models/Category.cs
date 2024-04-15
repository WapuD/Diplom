using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("categoryId")]
        public int CategoryId { get; set; }
        [Column("categoryName")]
        public string CategoryName { get; set; }
    }
}
