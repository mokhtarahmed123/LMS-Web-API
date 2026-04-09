using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class LessonFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }


        [MaxLength(500)]
        public string? FileUrl { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public Lessons Lesson { get; set; }
    }
}