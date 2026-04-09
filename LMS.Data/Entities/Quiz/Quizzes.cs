using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities.Quiz
{
    public class Quizzes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int TotalMarks { get; set; }

        public int PassingScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsTimeBound { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public int CourseId { get; set; }


        [ForeignKey("CourseId")]
        public Courses Course { get; set; }

        public ICollection<QuizQuestions> Questions { get; set; } = new List<QuizQuestions>();
    }
}