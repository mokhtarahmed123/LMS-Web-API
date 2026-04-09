using FluentValidation;
using LMS.Core.Feature.QuestionOptions.Command.Model;

namespace LMS.Core.Feature.QuestionOptions.Command.Validator
{
    public class AddQuestionOptionsCommandValidator : AbstractValidator<AddQuestionOptionsCommand>
    {
        public AddQuestionOptionsCommandValidator()
        {
            ValidateRule();
        }
        public void ValidateRule()
        {
            RuleFor(x => x.OptionText)
         .NotEmpty().WithMessage("Option text is required.")
        .MaximumLength(500).WithMessage("Option text must not exceed 500 characters.");

            RuleFor(x => x.IsCorrect)
          .NotNull().WithMessage("IsCorrect is required.");
        }

    }
}
