using FluentValidation;
using LMS.Core.Feature.QuizQuestions.Command.Model;
using LMS.Service.Abstract.QuizzesAbstract;

namespace LMS.Core.Feature.QuizQuestions.Command.Validator
{
    public class AddQuizQuestionsCommandValidator : AbstractValidator<AddQuizQuestionsCommand>
    {
        private readonly IQuizService quizService;

        public AddQuizQuestionsCommandValidator(IQuizService quizService)
        {
            this.quizService = quizService;
            ValidateRules();
        }

        private void ValidateRules()
        {
            RuleFor(x => x.QuestionText)
                .NotEmpty().WithMessage("QuestionText is required")
                .MaximumLength(1000).WithMessage("QuestionText must not exceed 1000 characters");

            RuleFor(x => x.TypeOfQuestions)
                .IsInEnum().WithMessage("Invalid question type");

            RuleFor(x => x.QuizId)
                .GreaterThan(0).WithMessage("QuizId is required")
                .MustAsync(async (quizId, cancellationToken) => await QuizExists(quizId, cancellationToken))
                .WithMessage("Quiz not found");
        }

        private async Task<bool> QuizExists(int quizId, CancellationToken cancellationToken)
        {
            var result = await quizService.GetQuizByIdAsync(quizId);
            return result != null;
        }
    }
}
