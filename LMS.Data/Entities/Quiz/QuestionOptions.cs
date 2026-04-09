using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities.Quiz
{
    public class QuestionOptions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }


        public int QuizQuestionId { get; set; }


        [ForeignKey("QuizQuestionId")]
        public QuizQuestions Question { get; set; }
    }
}