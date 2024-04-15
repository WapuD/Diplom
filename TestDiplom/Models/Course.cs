using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("courseId")]
        public int CourseId { get; set; }
        [Column("courseName")]
        public string CourseName { get; set; }
        [Column("courseDescription")]
        public string CourseDescription { get; set; }
        [Column("courseDate")]
        public string CourseDate { get; set; }
        [Column("courseText")]
        public string CourseText { get; set; }
        [Column("courseTextRecom")]
        public string CourseTextRecom { get; set; }
        [Column("courseTextWarning")]
        public string CourseTextWarning { get; set; }


        [Column("courseCategory")]
        [ForeignKey("Category")]
        public int CourseCategory { get; set; }
        public Category Category { get; set; }
        [Column("courseAuthor")]
        [ForeignKey("Author")]
        public int CourseAuthor { get; set; }
        public User Author { get; set; }
    }
}
