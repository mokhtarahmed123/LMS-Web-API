using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class UserCourses
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public int CourseId { get; set; }



        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public int? Rating { get; set; } = 0;

        public bool IsFavorite { get; set; } = false;



        [ForeignKey("CourseId")]
        public virtual Courses Course { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}
