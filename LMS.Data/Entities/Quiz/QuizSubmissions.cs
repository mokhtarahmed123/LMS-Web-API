using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities.Quiz
{
    public class QuizSubmissions
    {
        [Key]
        public int Id { get; set; }

        public int Score { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;


        public int QuizId { get; set; }
        public string UserId { get; set; }


        [ForeignKey("QuizId")]
        public Quizzes Quiz { get; set; }

        [ForeignKey("UserId")]
        public Users Student { get; set; }

        public ICollection<SubmissionAnswers> Answers { get; set; } = new List<SubmissionAnswers>();
    }
}