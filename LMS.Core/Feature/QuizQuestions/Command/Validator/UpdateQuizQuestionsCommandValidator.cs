using FluentValidation;
using LMS.Core.Feature.QuizQuestions.Command.Model;

namespace LMS.Core.Feature.QuizQuestions.Command.Validator
{
    public class UpdateQuizQuestionsCommandValidator : AbstractValidator<UpdateQuizQuestionsCommand>
    {
        public UpdateQuizQuestionsCommandValidator()
        {
            ValidateQuestionText();
            ValidateTypeOfQuestions();

        }

        public void ValidateQuestionText()
        {
            RuleFor(x => x.QuestionText)
                .NotEmpty().WithMessage("QuestionText is required.")
                .MaximumLength(500).WithMessage("QuestionText cannot exceed 500 characters.");
        }
        public void ValidateTypeOfQuestions()
        {
            RuleFor(x => x.TypeOfQuestions)
                .IsInEnum().WithMessage("TypeOfQuestions must be a valid enum value.");
        }
    }
}
