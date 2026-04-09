using LMS.Data_.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LMS.Core.Feature.Courses.Command.Models.InstructorCommand
{
    public class UpdateCourseCommand : IRequest<Response<string>>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsFree { get; set; } = true;
        public CoursesLevelEnum Level { get; set; } = CoursesLevelEnum.Beginner;
        public CoursesLanguageEnum CourseLanguage { get; set; } = CoursesLanguageEnum.English;
        public IFormFile? ProfilePicture { get; set; }
        public int CategoryId { get; set; }
    }
}
