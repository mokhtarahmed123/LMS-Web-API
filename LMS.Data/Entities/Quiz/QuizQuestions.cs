using LMS.Data_.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities.Quiz
{
    public class QuizQuestions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string QuestionText { get; set; }

        public TypeOfQuestions QuestionsType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }


        public int QuizId { get; set; }


        [ForeignKey("QuizId")]
        public Quizzes Quiz { get; set; }

        public ICollection<QuestionOptions> Options { get; set; } = new List<QuestionOptions>();
    }
}