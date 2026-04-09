using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class Reviews
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [ForeignKey("LessonId")]
        public virtual Lessons Lesson { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}
