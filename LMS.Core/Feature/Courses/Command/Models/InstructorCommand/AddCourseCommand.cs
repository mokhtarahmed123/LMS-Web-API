using LMS.Data_.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LMS.Core.Feature.Courses.Command.Models.InstructorCommand
{
    public class AddCourseCommand : IRequest<Response<string>>
    {
        public string Title { get; set; }
        public string? Description { get; set; }


        public CoursesLevelEnum Level { get; set; }
        public CoursesLanguageEnum CourseLanguage { get; set; }

        //    public string ThumbnailUrl { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        public int CategoryId { get; set; }
        //  public int InstructorProfileId { get; set; }  // Will Change When Authentication and Authorization Implemented We Take It From The Token Claims

    }
}
