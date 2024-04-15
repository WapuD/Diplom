using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Favorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("favoriteId")]
        public int FavoriteId { get; set; }
        [Column("favoriteUser")]
        [ForeignKey("User")]
        public int FavoriteUser { get; set; }
        public User User { get; set; }
        [Column("favoriteCourse")]
        [ForeignKey("Course")]
        public int FavoriteCourse { get; set; }
        public Course Course { get; set; }
    }
}
