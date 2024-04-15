using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CourseDTO
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseText { get; set; }
        public string CourseTextRecom { get; set; }
        public string CourseTextWarning { get; set; }
        public int CourseCategory { get; set; }
        public int CourseAuthor { get; set; }
    }
}
