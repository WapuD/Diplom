using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("imageId")]
        public int ImageId { get; set; }
        [Column("imageData")]
        public byte[] ImageData { get; set; }

    }
}
