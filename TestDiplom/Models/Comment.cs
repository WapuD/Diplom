using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("commentId")]
        public int CommentId { get; set; }
        [Column("commentText")]
        public string CommentText { get; set; }
        [Column("commentCourse")]
        [ForeignKey("Course")]
        public int CommentCourse { get; set; }
        public Course Course { get; set; }
    }
}
