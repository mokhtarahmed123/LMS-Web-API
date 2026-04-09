using FluentValidation;
using LMS.Core.Feature.Quizzes.Command.Model;
using LMS.Service.Abstract;

namespace LMS.Core.Feature.Quizzes.Command.Validator
{
    public class AddQuizCommandValidator : AbstractValidator<AddQuizCommand>
    {
        private readonly ICoursesService coursesService;

        public AddQuizCommandValidator(ICoursesService coursesService)
        {
            this.coursesService = coursesService;
            ValidateRules();
        }

        private void ValidateRules()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
                .When(x => x.Description != null);

            RuleFor(x => x.TotalMarks)
                .GreaterThan(0).WithMessage("TotalMarks must be greater than 0");

            RuleFor(x => x.PassingScore)
                .GreaterThan(0).WithMessage("PassingScore must be greater than 0")
                .LessThanOrEqualTo(x => x.TotalMarks).WithMessage("PassingScore must not exceed TotalMarks");

            RuleFor(x => x.CourseId)
        .NotEmpty().WithMessage("CourseId is required")
           .MustAsync(async (courseId, cancellationToken) => await CourseExists(courseId, cancellationToken))
              .WithMessage("Course not found");

            When(x => x.IsTimeBound, () =>
            {
                RuleFor(x => x.StartDate)
                    .NotNull().WithMessage("StartDate is required when IsTimeBound is true");

                RuleFor(x => x.EndDate)
                    .NotNull().WithMessage("EndDate is required when IsTimeBound is true")
                    .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");
            });
        }

        private async Task<bool> CourseExists(int courseId, CancellationToken cancellationToken)
        {
            var result = await coursesService.GetCourseById(courseId);
            return result != null;
        }
    }
}
