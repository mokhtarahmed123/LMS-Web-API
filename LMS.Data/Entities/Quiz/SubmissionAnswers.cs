using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities.Quiz
{
    public class SubmissionAnswers
    {



        public int SubmissionId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }


        [ForeignKey("SubmissionId")]
        public QuizSubmissions Submission { get; set; }

        [ForeignKey("QuestionId")]
        public QuizQuestions Question { get; set; }

        [ForeignKey("SelectedOptionId")]
        public QuestionOptions SelectedOption { get; set; }
    }
}